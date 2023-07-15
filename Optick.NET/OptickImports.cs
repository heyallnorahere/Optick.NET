using System.Runtime.InteropServices;

namespace Optick.NET
{
    public static partial class Optick
    {
        public const string LibraryName = "OptickCore";
        public const CallingConvention Convention = CallingConvention.Cdecl;

        [DllImport(LibraryName, EntryPoint = "Optick::GetHighPrecisionTime", CallingConvention = Convention)]
        public static extern long GetHighPrecisionTime();
        [DllImport(LibraryName, EntryPoint = "Optick::GetHighPrecisionFrequency", CallingConvention = Convention)]
        public static extern long GetHighPrecisionFrequency();

        [DllImport(LibraryName, EntryPoint = "Optick::Update", CallingConvention = Convention)]
        public static extern void Update();
        [DllImport(LibraryName, EntryPoint = "Optick::BeginFrame", CallingConvention = Convention)]
        public static extern uint BeginFrame(FrameType type = FrameType.CPU, long timestamp = -1, ulong threadId = ulong.MaxValue);
        [DllImport(LibraryName, EntryPoint = "Optick::EndFrame", CallingConvention = Convention)]
        public static extern uint EndFrame(FrameType type = FrameType.CPU, long timestamp = -1, ulong threadId = ulong.MaxValue);
        [DllImport(LibraryName, EntryPoint = "Optick::IsActive", CallingConvention = Convention)]
        public static extern bool IsActive(Mode mode = Mode.INSTRUMENTATION_EVENTS);

        [DllImport(LibraryName, EntryPoint = "Optick::RegisterFiber", CallingConvention = Convention)]
        public static extern unsafe bool RegisterFiber(ulong fiberId, nint* slot);
        [DllImport(LibraryName, EntryPoint = "Optick::RegisterThread", CallingConvention = Convention)]
        public static extern bool RegisterThread([MarshalAs(UnmanagedType.LPWStr)] string name);
        [DllImport(LibraryName, EntryPoint = "Optick::UnRegisterThread", CallingConvention = Convention)]
        public static extern bool UnRegisterThread(bool keepAlive);
        [DllImport(LibraryName, EntryPoint = "Optick::GetEventStorageSlotForCurrentThread", CallingConvention = Convention)]
        public static extern unsafe nint* GetEventStorageSlotForCurrentThread();
        [DllImport(LibraryName, EntryPoint = "Optick::IsFiberStorage", CallingConvention = Convention)]
        public static extern bool IsFiberStorage(nint fiberStorage);
        [DllImport(LibraryName, EntryPoint = "Optick::RegisterStorage", CallingConvention = Convention)]
        public static extern nint RegisterStorage([MarshalAs(UnmanagedType.LPStr)] string name, ulong threadId = ulong.MaxValue, ThreadMask mask = ThreadMask.None);

        [DllImport(LibraryName, EntryPoint = "Optick::SetStateChangedCallback", CallingConvention = Convention)]
        public static extern bool SetStateChangedCallback(nint callback); // use StateCallback, and add a GCHandle to the delegate

        [DllImport(LibraryName, EntryPoint = "Optick::AttachSummary", CallingConvention = Convention)]
        public static extern bool AttachSummary([MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);
        [DllImport(LibraryName, EntryPoint = "Optick::AttachFile", CallingConvention = Convention)]
        public static extern unsafe bool AttachFile(FileType type, [MarshalAs(UnmanagedType.LPStr)] string name, void* data, uint size);
        [DllImport(LibraryName, EntryPoint = "Optick::AttachFile", CallingConvention = Convention)]
        public static extern bool AttachFile(FileType type, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string path);
    }
}