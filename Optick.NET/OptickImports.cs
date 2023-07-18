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
        public static extern bool SetStateChangedCallback(StateCallback callback); // implicitly marshals

        [DllImport(LibraryName, EntryPoint = "Optick::AttachSummary", CallingConvention = Convention)]
        public static extern bool AttachSummary([MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);
        [DllImport(LibraryName, EntryPoint = "Optick::AttachFile", CallingConvention = Convention)]
        public static extern unsafe bool AttachFile(FileType type, [MarshalAs(UnmanagedType.LPStr)] string name, void* data, uint size);
        [DllImport(LibraryName, EntryPoint = "Optick::AttachFile", CallingConvention = Convention)]
        public static extern bool AttachFile(FileType type, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string path);

        [DllImport(LibraryName, EntryPoint = "Optick::InitGpuD3D12", CallingConvention = Convention)]
        public static extern unsafe void InitGpuD3D12(nint device, nint* cmdQueues, uint numQueues);
        [DllImport(LibraryName, EntryPoint = "Optick::InitGpuVulkan", CallingConvention = Convention)]
        public static extern unsafe void InitGpuVulkan(nint* devices, nint* physicalDevices, nint* queues, uint* queueFamilies, uint numQueues, VulkanFunctions* functions);
        [DllImport(LibraryName, EntryPoint = "Optick::GpuFlip", CallingConvention = Convention)]
        public static extern void GpuFlip(nint swapChain);
        [DllImport(LibraryName, EntryPoint = "Optick::SetGpuContext", CallingConvention = Convention)]
        public static extern GPUContext SetGpuContext(GPUContext context);

        [DllImport(LibraryName, EntryPoint = "Optick::GetFrameDescription", CallingConvention = Convention)]
        public static extern unsafe EventDescription* GetFrameDescription(FrameType frame = FrameType.CPU);

        [DllImport(LibraryName, EntryPoint = "Optick::SetAllocator", CallingConvention = Convention)]
        public static extern void SetAllocator(AllocateFunction allocateFunction, DeallocateFunction deallocateFunction, InitThreadCallback initThreadCallback);
        [DllImport(LibraryName, EntryPoint = "Optick::Shutdown", CallingConvention = Convention)]
        public static extern void Shutdown();

        [DllImport(LibraryName, EntryPoint = "Optick::StartCapture", CallingConvention = Convention)]
        public static extern bool StartCapture(Mode mode = Mode.DEFAULT, int samplingFrequency = 1000, bool force = true);
        [DllImport(LibraryName, EntryPoint = "Optick::StopCapture", CallingConvention = Convention)]
        public static extern bool StopCapture(bool force = true);
        [DllImport(LibraryName, EntryPoint = "Optick::SaveCapture", CallingConvention = Convention)]
        public static extern bool SaveCapture(CaptureSaveChunkCallback dataCallback, bool force = true);
        [DllImport(LibraryName, EntryPoint = "Optick::SaveCapture", CallingConvention = Convention)]
        public static extern bool SaveCapture([MarshalAs(UnmanagedType.LPStr)] string path, bool force = true);
    }
}