using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Optick.NET
{
    internal sealed class DummyDisposable : IDisposable
    {
        public void Dispose()
        {
            // does nothing
        }
    }

    public static class OptickMacros
    {
        private static readonly Dictionary<long, nint> sEventDescriptions;
        private static readonly bool sEnabled;
        static OptickMacros()
        {
            sEventDescriptions = new Dictionary<long, nint>();
            sEnabled = true;

            var disabledPlatforms = new HashSet<string>
            {
                "ios",
                "maccatalyst",
                "android"
            };

            foreach (var platformName in disabledPlatforms)
            {
                var osplatform = OSPlatform.Create(platformName);
                if (!RuntimeInformation.IsOSPlatform(osplatform))
                {
                    sEnabled = false;
                    break;
                }
            }
        }

        public static bool IsOptickEnabled => sEnabled;

        public static Category MakeCategory(Filter filter, Color color) => new Category((((ulong)(1)) << ((int)filter + 32)) | (ulong)color);

        // CreateDescription(const char*, const char*, int, const ::Optick::Category::Type) is redundant
        public static unsafe EventDescription* CreateDescription(string functionName, string fileName, int fileLine, string? eventName = null, Category? category = null, EventDescription.Flags flags = 0)
        {
            if (!sEnabled)
            {
                return null;
            }

            // why do i need the null-forgiving operator?
            var usedEventName = string.IsNullOrEmpty(eventName) ? functionName : eventName!;

            var usedCategory = category ?? NET.Category.None;
            return EventDescription.Create(usedEventName, fileName, (uint)fileLine, usedCategory.CategoryColor, usedCategory.CategoryMask, flags | EventDescription.Flags.COPY_NAME_STRING | EventDescription.Flags.COPY_FILENAME_STRING);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe EventDescription* GetEventDescription(int frameSkip, string? name = null, Category? category = null, EventDescription.Flags flags = 0)
        {
            var stackFrame = new StackFrame(frameSkip + 1, true);
            var method = stackFrame.GetMethod();

            int lineNumber = stackFrame.GetFileLineNumber();
            int usedLineNumber = lineNumber > 0 ? lineNumber : stackFrame.GetILOffset();

            long id = (((long)method.GetHashCode()) << 32) | (long)lineNumber;
            lock (sEventDescriptions)
            {
                if (!sEventDescriptions.TryGetValue(id, out nint description))
                {
                    string parameterString = string.Empty;
                    var parameters = method.GetParameters();

                    foreach (var parameter in parameters)
                    {
                        if (parameterString.Length > 0)
                        {
                            parameterString += ", ";
                        }

                        var parameterType = parameter.ParameterType;
                        parameterString += parameterType.FullName ?? parameterType.Name;
                    }

                    var declaringType = method.DeclaringType;
                    var methodName = $"{declaringType}.{method.Name}";
                    var methodSignature = $"{methodName}({parameterString})";

                    var fileName = stackFrame.GetFileName();
                    var usedFileName = string.IsNullOrEmpty(fileName) ? "<unknown>" : fileName;

                    var eventDescription = CreateDescription(methodSignature, usedFileName, usedLineNumber, name, category, flags);
                    description = sEventDescriptions[id] = (nint)eventDescription;
                }

                return (EventDescription*)description;
            }
        }

        // if only we had c macros...
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe IDisposable Event(string? name = null, Category? category = null, EventDescription.Flags flags = 0, int frameSkip = 1)
        {
            if (!sEnabled)
            {
                return new DummyDisposable();
            }

            var usedFlags = flags;
            if (!string.IsNullOrEmpty(name))
            {
                usedFlags |= EventDescription.Flags.IS_CUSTOM_NAME;
            }

            var description = GetEventDescription(frameSkip, name, category, usedFlags);
            return new Event(ref *description);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IDisposable Category(string name, Category category, int frameSkip = 1) => Event(name, category, frameSkip: frameSkip + 1);

        /// <summary>
        /// <b>IMPORTANT:</b> user will need to register the thread manually
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe IDisposable Frame(string name, FrameType type = FrameType.CPU, int frameSkip = 1)
        {
            if (!sEnabled)
            {
                return new DummyDisposable();
            }

            OptickImports.EndFrame(type);
            OptickImports.Update();

            uint frameNumber = OptickImports.BeginFrame(type);
            var resultEvent = new Event(ref *OptickImports.GetFrameDescription(type));

            Tag("Frame", new object[] { frameNumber }, frameSkip: 2);
            return resultEvent;
        }

        public static void FrameFlip(FrameType type = FrameType.CPU, long timestamp = -1, ulong threadID = ulong.MaxValue)
        {
            if (!sEnabled)
            {
                return;
            }

            OptickImports.EndFrame(type, timestamp, threadID);
            OptickImports.BeginFrame(type, timestamp, threadID);
        }

        public static unsafe IDisposable FrameEvent(FrameType type)
        {
            if (!sEnabled)
            {
                return new DummyDisposable();
            }

            OptickImports.EndFrame(type);
            if (type == FrameType.CPU)
            {
                OptickImports.Update();
            }

            OptickImports.BeginFrame(type);
            return new Event(ref *OptickImports.GetFrameDescription(type));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Tag(string name, params object?[] args) => Tag(name, args, frameSkip: 1);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void Tag(string name, object?[] args, int frameSkip = 1)
        {
            if (!sEnabled)
            {
                return;
            }

            var description = GetEventDescription(frameSkip, name: name);

            var tagType = typeof(Tag);
            var methods = tagType.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != args.Length + 1)
                {
                    continue;
                }

                for (int i = 0; i < args.Length; i++)
                {
                    var parameterType = parameters[i + 1].ParameterType;
                    var argumentType = args[i]?.GetType();

                    if (argumentType is null ? (parameterType.IsClass || Nullable.GetUnderlyingType(parameterType) is not null) : parameterType.IsAssignableFrom(argumentType))
                    {
                        method.Invoke(null, new object[] { (nint)description }.Concat(args).ToArray());
                        return;
                    }
                }
            }

            object? value = args.Length > 1 ? args : args.FirstOrDefault();
            NET.Tag.Attach((nint)description, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe IDisposable GPUEvent(string name, int frameSkip = 1)
        {
            if (!sEnabled)
            {
                return new DummyDisposable();
            }

            var description = GetEventDescription(frameSkip, name: name);
            return new GPUEvent(ref *description);
        }

        public static void Shutdown()
        {
            if (!sEnabled)
            {
                return;
            }

            OptickImports.Shutdown();
        }
    }
}