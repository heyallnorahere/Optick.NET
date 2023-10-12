using System.Runtime.InteropServices;

namespace Optick.NET
{
    /// <summary>
    /// Imported functions directly from Optick. Do keep in mind that if <see cref="OptickMacros.IsOptickEnabled"/> is not true, calling any of these functions may result in an error
    /// </summary>
    public static class OptickImports
    {
        public const string LibraryName = "coptick";
        public const CallingConvention ImportConvention = CallingConvention.Cdecl;
        public const CallingConvention CallbackConvention = CallingConvention.StdCall;

        [DllImport(LibraryName, EntryPoint = "Optick_GetHighPrecisionTime", CallingConvention = ImportConvention)]
        public static extern long GetHighPrecisionTime();
        [DllImport(LibraryName, EntryPoint = "Optick_GetHighPrecisionFrequency", CallingConvention = ImportConvention)]
        public static extern long GetHighPrecisionFrequency();

        [DllImport(LibraryName, EntryPoint = "Optick_Update", CallingConvention = ImportConvention)]
        public static extern void Update();
        [DllImport(LibraryName, EntryPoint = "Optick_BeginFrame", CallingConvention = ImportConvention)]
        public static extern uint BeginFrame(FrameType type = FrameType.CPU, long timestamp = -1, ulong threadId = ulong.MaxValue);
        [DllImport(LibraryName, EntryPoint = "Optick_EndFrame", CallingConvention = ImportConvention)]
        public static extern uint EndFrame(FrameType type = FrameType.CPU, long timestamp = -1, ulong threadId = ulong.MaxValue);
        [DllImport(LibraryName, EntryPoint = "Optick_IsActive", CallingConvention = ImportConvention)]
        public static extern bool IsActive(Mode mode = Mode.INSTRUMENTATION_EVENTS);

        [DllImport(LibraryName, EntryPoint = "Optick_RegisterFiber", CallingConvention = ImportConvention)]
        public static extern unsafe bool RegisterFiber(ulong fiberId, nint* slot);
        [DllImport(LibraryName, EntryPoint = "Optick_RegisterThread_LPWStr", CallingConvention = ImportConvention)]
        public static extern bool RegisterThread([MarshalAs(UnmanagedType.LPWStr)] string name);
        [DllImport(LibraryName, EntryPoint = "Optick_UnRegisterThread", CallingConvention = ImportConvention)]
        public static extern bool UnRegisterThread(bool keepAlive);
        [DllImport(LibraryName, EntryPoint = "Optick_GetEventStorageSlotForCurrentThread", CallingConvention = ImportConvention)]
        public static extern unsafe nint* GetEventStorageSlotForCurrentThread();
        [DllImport(LibraryName, EntryPoint = "Optick_IsFiberStorage", CallingConvention = ImportConvention)]
        public static extern bool IsFiberStorage(nint fiberStorage);
        [DllImport(LibraryName, EntryPoint = "Optick_RegisterStorage", CallingConvention = ImportConvention)]
        public static extern nint RegisterStorage([MarshalAs(UnmanagedType.LPStr)] string name, ulong threadId = ulong.MaxValue, ThreadMask mask = ThreadMask.None);

        [DllImport(LibraryName, EntryPoint = "Optick_SetStateChangedCallback", CallingConvention = ImportConvention)]
        public static extern bool SetStateChangedCallback(StateCallback callback); // implicitly marshals

        [DllImport(LibraryName, EntryPoint = "Optick_AttachSummary", CallingConvention = ImportConvention)]
        public static extern bool AttachSummary([MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);
        [DllImport(LibraryName, EntryPoint = "Optick_AttachFile_Data", CallingConvention = ImportConvention)]
        public static extern unsafe bool AttachFile(FileType type, [MarshalAs(UnmanagedType.LPStr)] string name, void* data, uint size);
        [DllImport(LibraryName, EntryPoint = "Optick_AttachFile_LPWStr", CallingConvention = ImportConvention)]
        public static extern bool AttachFile(FileType type, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string path);

        [DllImport(LibraryName, EntryPoint = "Optick_InitGpuD3D12", CallingConvention = ImportConvention)]
        public static extern unsafe void InitGpuD3D12(nint device, nint* cmdQueues, uint numQueues);
        [DllImport(LibraryName, EntryPoint = "Optick_InitGpuVulkan", CallingConvention = ImportConvention)]
        public static extern unsafe void InitGpuVulkan(nint* devices, nint* physicalDevices, nint* queues, uint* queueFamilies, uint numQueues, VulkanFunctions* functions);
        [DllImport(LibraryName, EntryPoint = "Optick_GpuFlip", CallingConvention = ImportConvention)]
        public static extern void GpuFlip(nint swapChain);
        [DllImport(LibraryName, EntryPoint = "Optick_SetGpuContext", CallingConvention = ImportConvention)]
        public static extern unsafe void SetGpuContext(GPUContext* newContext, GPUContext* oldContext);

        [DllImport(LibraryName, EntryPoint = "Optick_GetFrameDescription", CallingConvention = ImportConvention)]
        public static extern unsafe EventDescription* GetFrameDescription(FrameType frame = FrameType.CPU);

        [DllImport(LibraryName, EntryPoint = "Optick_SetAllocator", CallingConvention = ImportConvention)]
        public static extern void SetAllocator(AllocateFunction allocateFunction, DeallocateFunction deallocateFunction, InitThreadCallback initThreadCallback);
        [DllImport(LibraryName, EntryPoint = "Optick_Shutdown", CallingConvention = ImportConvention)]
        public static extern void Shutdown();

        [DllImport(LibraryName, EntryPoint = "Optick_StartCapture", CallingConvention = ImportConvention)]
        public static extern bool StartCapture(Mode mode = Mode.DEFAULT, int samplingFrequency = 1000, bool force = true);
        [DllImport(LibraryName, EntryPoint = "Optick_StopCapture", CallingConvention = ImportConvention)]
        public static extern bool StopCapture(bool force = true);
        [DllImport(LibraryName, EntryPoint = "Optick_SaveCapture_Callback", CallingConvention = ImportConvention)]
        public static extern bool SaveCapture(CaptureSaveChunkCallback dataCallback, bool force = true);
        [DllImport(LibraryName, EntryPoint = "Optick_SaveCapture_LPStr", CallingConvention = ImportConvention)]
        public static extern bool SaveCapture([MarshalAs(UnmanagedType.LPStr)] string path, bool force = true);
    }
}