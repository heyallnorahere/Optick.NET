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

        public void Begin() => Start = Optick.GetHighPrecisionTime();
        public void End() => Finish = Optick.GetHighPrecisionTime();

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

        [DllImport(Optick.LibraryName, EntryPoint = "Optick::FiberSyncData::AttachToThread", CallingConvention = Optick.Convention)]
        public static extern void AttachToThread(nint storage, ulong threadId);
        [DllImport(Optick.LibraryName, EntryPoint = "Optick::FiberSyncData::DetachFromThread", CallingConvention = Optick.Convention)]
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

        public static unsafe EventDescription Create()
        {
            var description = new EventDescription();
            Constructor(&description);

            return description;
        }

        [DllImport(Optick.LibraryName, EntryPoint = "Optick::EventDescription::EventDescription", CallingConvention = Optick.Convention)]
        private static extern unsafe void Constructor(EventDescription* description);

        [DllImport(Optick.LibraryName, EntryPoint = "Optick::EventDescription::Create", CallingConvention = Optick.Convention)]
        public static extern unsafe EventDescription* Create([MarshalAs(UnmanagedType.LPStr)] string eventName, [MarshalAs(UnmanagedType.LPStr)] string fileName, uint fileLine, Color eventColor = Color.Null, Filter filter = Filter.None, Flags flags = 0);
        [DllImport(Optick.LibraryName, EntryPoint = "Optick::EventDescription::CreateShared", CallingConvention = Optick.Convention)]
        public static extern unsafe EventDescription* Create([MarshalAs(UnmanagedType.LPStr)] string eventName, [MarshalAs(UnmanagedType.LPStr)] string? fileName = null, uint fileLine = 0, Color eventColor = Color.Null, Filter filter = Filter.None);
    }
}