using System;
using System.Runtime.InteropServices;

namespace Optick.NET
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EventTime
    {
        public const long InvalidTimestamp = -1;

        public EventTime()
        {
            Start = Finish = InvalidTimestamp;
        }

        public long Start, Finish;

        public void Begin() => Start = OptickImports.GetHighPrecisionTime();
        public void End() => Finish = OptickImports.GetHighPrecisionTime();

        /* not defining a matching operator <3
        public static bool operator <(EventTime lhs, EventTime rhs)
        {
            if (lhs.Start != rhs.Start)
            {
                return lhs.Start < rhs.Start;
            }

            return lhs.Finish > rhs.Finish;
        }
        */

        public bool IsValid => Start < Finish && Start != InvalidTimestamp && Finish != InvalidTimestamp;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EventData
    {
        public EventTime Timing;
        public unsafe EventDescription* Description;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SyncData
    {
        public EventTime Timing;
        public ulong NewThreadID, OldThreadID;
        public byte Core;
        public sbyte Reason;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FiberSyncData
    {
        public EventTime Timing;
        public ulong ThreadID;

        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_FiberSyncData_AttachToThread", CallingConvention = OptickImports.ImportConvention)]
        public static extern void AttachToThread(nint storage, ulong threadId);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_FiberSyncData_DetachFromThread", CallingConvention = OptickImports.ImportConvention)]
        public static extern void DetachFromThread(nint storage);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TagData<T> where T : unmanaged
    {
        public unsafe EventDescription* Description;
        public long Timestamp;
        public T Data;

        public unsafe TagData()
        {
            Description = null;
            Timestamp = 0;
            Data = default;
        }

        // todo: when EventDescription is defined
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EventDescription
    {
        [Flags]
        public enum Flags : byte
        {
            IS_CUSTOM_NAME = 1 << 0,
            COPY_NAME_STRING = 1 << 1,
            COPY_FILENAME_STRING = 1 << 2,
        }

        public unsafe byte* Name, File;
        public uint Line, Index;
        public Color EventColor;
        public Filter EventFilter;
        public Flags EventFlags;

        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_EventDescription_Create", CallingConvention = OptickImports.ImportConvention)]
        public static extern unsafe EventDescription* Create([MarshalAs(UnmanagedType.LPStr)] string eventName, [MarshalAs(UnmanagedType.LPStr)] string fileName, uint fileLine, Color eventColor = Color.Null, Filter filter = Filter.None, Flags flags = 0);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_EventDescription_CreateShared", CallingConvention = OptickImports.ImportConvention)]
        public static extern unsafe EventDescription* CreateShared([MarshalAs(UnmanagedType.LPStr)] string eventName, [MarshalAs(UnmanagedType.LPStr)] string? fileName = null, uint fileLine = 0, Color eventColor = Color.Null, Filter filter = Filter.None);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Event : IDisposable
    {
        public unsafe EventData* Data;

        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Start", CallingConvention = OptickImports.ImportConvention)]
        public static extern unsafe EventData* Start(ref EventDescription description);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Stop", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Stop(ref EventData data);

        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Push_LPStr", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Push([MarshalAs(UnmanagedType.LPStr)] string name);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Push_Description", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Push(ref EventDescription description);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Pop", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Pop();

        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Add", CallingConvention = OptickImports.ImportConvention)]
        public static extern unsafe void Add(nint storage, EventDescription* description, long timestampStart, long timestampFinish);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Push_Storage", CallingConvention = OptickImports.ImportConvention)]
        public static extern unsafe void Push(nint storage, EventDescription* description, long timestampStart);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Event_Pop_Storage", CallingConvention = OptickImports.ImportConvention)]
        public static extern unsafe void Pop(nint storage, long timestampStart);

        public unsafe Event(ref EventDescription description)
        {
            Data = Start(ref description);
        }

        public unsafe void Dispose()
        {
            if (Data is not null)
            {
                Stop(ref *Data);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GPUEvent : IDisposable
    {
        public unsafe EventData* Data;

        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_GPUEvent_Start", CallingConvention = OptickImports.ImportConvention)]
        public static extern unsafe EventData* Start(ref EventDescription description);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_GPUEvent_Stop", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Stop(ref EventData data);

        public unsafe GPUEvent(ref EventDescription description)
        {
            Data = Start(ref description);
        }

        public unsafe void Dispose()
        {
            if (Data is not null)
            {
                Stop(ref *Data);
            }
        }
    }

    public static class Tag
    {
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Tag_Attach_Float", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Attach(nint description, float value);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Tag_Attach_Int32", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Attach(nint description, int value);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Tag_Attach_UInt32", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Attach(nint description, uint value);
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Tag_Attach_UInt64", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Attach(nint description, ulong value);
        public static void Attach(nint description, object? value) => Attach(description, value?.ToString() ?? "null");
        [DllImport(OptickImports.LibraryName, EntryPoint = "Optick_Tag_Attach_LPStr", CallingConvention = OptickImports.ImportConvention)]
        public static extern void Attach(nint description, [MarshalAs(UnmanagedType.LPStr)] string value);
    }

    public struct ThreadScope : IDisposable
    {
        public ThreadScope(string name)
        {
            OptickImports.RegisterThread(name);
        }

        public void Dispose()
        {
            OptickImports.UnRegisterThread(false);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GPUContext
    {
        public nint CommandList;
        public GPUQueueType QueueType;
        public int Node;
    }

    public readonly struct GPUContextScope : IDisposable
    {
        public readonly GPUContext PreviousContext;

        public unsafe GPUContextScope(nint commandList, GPUQueueType queueType = GPUQueueType.Graphics, int node = 0)
        {
            var context = new GPUContext
            {
                CommandList = commandList,
                QueueType = queueType,
                Node = node
            };

            fixed (GPUContext* previous = &PreviousContext)
            {
                OptickImports.SetGpuContext(&context, previous);
            }
        }

        public unsafe void Dispose()
        {
            fixed (GPUContext* previous = &PreviousContext)
            {
                OptickImports.SetGpuContext(previous, null);
            }
        }
    }

    public struct OptickApp : IDisposable
    {
        public string Name { get; set; }

        public OptickApp(string name)
        {
            Name = name;

            // see OPTICK_APP(NAME)
            OptickImports.RegisterThread(name);
            OptickImports.StartCapture();
        }

        public void Dispose()
        {
            OptickImports.StopCapture();
            OptickImports.SaveCapture(Name);
            OptickImports.UnRegisterThread(false);
        }
    }
}