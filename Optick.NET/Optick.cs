using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Optick.NET
{
    public static partial class Optick
    {
        private static readonly Dictionary<long, nint> sEventDescriptions;
        static Optick()
        {
            sEventDescriptions = new Dictionary<long, nint>();
        }

        public static Category MakeCategory(Filter filter, Color color) => new Category((((ulong)(1)) << ((int)filter + 32)) | (ulong)color);

        // CreateDescription(const char*, const char*, int, const ::Optick::Category::Type) is redundant
        public static unsafe EventDescription* CreateDescription(string functionName, string fileName, int fileLine, string? eventName = null, Category? category = null, EventDescription.Flags flags = 0)
        {
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

            var fileName = stackFrame.GetFileName();
            var usedFileName = string.IsNullOrEmpty(fileName) ? "<unknown>" : fileName;

            int lineNumber = stackFrame.GetFileLineNumber();
            int usedLineNumber = lineNumber > 0 ? lineNumber : stackFrame.GetILOffset();

            long id = (((long)method.GetHashCode()) << 32) | (long)lineNumber;
            if (!sEventDescriptions.TryGetValue(id, out nint description))
            {
                var eventDescription = CreateDescription(method.Name, usedFileName, usedLineNumber, name, category, flags);
                description = sEventDescriptions[id] = (nint)eventDescription;
            }

            return (EventDescription*)description;
        }

        // if only we had c macros...
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe Event Event(string? name = null, Category? category = null, EventDescription.Flags flags = 0, int frameSkip = 1)
        {
            var usedFlags = flags;
            if (!string.IsNullOrEmpty(name))
            {
                usedFlags |= EventDescription.Flags.IS_CUSTOM_NAME;
            }

            var description = GetEventDescription(frameSkip, name, category, usedFlags);
            return new Event(ref *description);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Event Category(string name, Category category, int frameSkip = 1) => Event(name, category, frameSkip: frameSkip + 1);

        /// <summary>
        /// <b>IMPORTANT:</b> user will need to register the thread manually
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe Event Frame(string name, FrameType type = FrameType.CPU, int frameSkip = 1)
        {
            EndFrame(type);
            Update();

            uint frameNumber = BeginFrame(type);
            var resultEvent = new Event(ref *GetFrameDescription(type));

            Tag("Frame", new object[] { frameNumber }, frameSkip: 2);
            return resultEvent;
        }

        public static void FrameFlip(FrameType type = FrameType.CPU, long timestamp = -1, ulong threadID = ulong.MaxValue)
        {
            EndFrame(type, timestamp, threadID);
            BeginFrame(type, timestamp, threadID);
        }

        public static unsafe Event FrameEvent(FrameType type)
        {
            EndFrame(type);
            if (type == FrameType.CPU)
            {
                Update();
            }

            BeginFrame(type);
            return new Event(ref *GetFrameDescription(type));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Tag(string name, params object?[] args) => Tag(name, args, frameSkip: 1);

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void Tag(string name, object?[] args, int frameSkip = 1)
        {
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
        public static unsafe GPUEvent GPUEvent(string name, int frameSkip = 1)
        {
            var description = GetEventDescription(frameSkip, name: name);
            return new GPUEvent(ref *description);
        }
    }
}