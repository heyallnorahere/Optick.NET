#include <optick.h>

#ifdef OPTICK_MSVC
#define COPTICK_API __declspec(dllexport)
#else
#define COPTICK_API
#endif

#define COPTICK_DECL extern "C"
#define COPTICK_EXPORT COPTICK_DECL COPTICK_API

COPTICK_EXPORT int64_t Optick_GetHighPrecisionTime() { return Optick::GetHighPrecisionTime(); }
COPTICK_EXPORT int64_t Optick_GetHighPrecisionFrequency() { return Optick::GetHighPrecisionFrequency(); }
COPTICK_EXPORT void Optick_Update() { Optick::Update(); }
COPTICK_EXPORT uint32_t Optick_BeginFrame(Optick::FrameType::Type type, int64_t timestamp, uint64_t threadID) { return Optick::BeginFrame(type, timestamp, threadID); }
COPTICK_EXPORT uint32_t Optick_EndFrame(Optick::FrameType::Type type, int64_t timestamp, uint64_t threadID) { return Optick::EndFrame(type, timestamp, threadID); }
COPTICK_EXPORT bool Optick_IsActive(Optick::Mode::Type mode) { return Optick::IsActive(mode); }

COPTICK_EXPORT bool Optick_RegisterFiber(uint64_t fiberId, Optick::EventStorage** slot) { return Optick::RegisterFiber(fiberId, slot); }
COPTICK_EXPORT bool Optick_RegisterThread_LPStr(const char* name) { return Optick::RegisterThread(name); }
COPTICK_EXPORT bool Optick_RegisterThread_LPWStr(const wchar_t* name) { return Optick::RegisterThread(name); }
COPTICK_EXPORT bool Optick_UnRegisterThread(bool keepAlive) { return Optick::UnRegisterThread(keepAlive); }
COPTICK_EXPORT Optick::EventStorage** Optick_GetEventStorageSlotForCurrentThread() { return Optick::GetEventStorageSlotForCurrentThread(); }
COPTICK_EXPORT bool Optick_IsFiberStorage(Optick::EventStorage* fiberStorage) { return Optick::IsFiberStorage(fiberStorage); }

COPTICK_EXPORT Optick::EventStorage* Optick_RegisterStorage(const char* name, uint64_t threadID, Optick::ThreadMask::Type type) {
    return Optick::RegisterStorage(name, threadID, type);
}

COPTICK_EXPORT bool Optick_SetStateChangedCallback(Optick::StateCallback cb) { return Optick::SetStateChangedCallback(cb); }

COPTICK_EXPORT bool Optick_AttachSummary(const char* key, const char* value) { return Optick::AttachSummary(key, value); }

COPTICK_EXPORT bool Optick_AttachFile_Data(Optick::File::Type type, const char* name, const uint8_t* data, uint32_t size) { return Optick::AttachFile(type, name, data, size); }
COPTICK_EXPORT bool Optick_AttachFile_LPStr(Optick::File::Type type, const char* name, const char* path) { return Optick::AttachFile(type, name, path); }
COPTICK_EXPORT bool Optick_AttachFile_LPWStr(Optick::File::Type type, const char* name, const wchar_t* path) { return Optick::AttachFile(type, name, path); }

COPTICK_EXPORT void Optick_FiberSyncData_AttachToThread(Optick::EventStorage* storage, uint64_t threadId) { Optick::FiberSyncData::AttachToThread(storage, threadId); }
COPTICK_EXPORT void Optick_FiberSyncData_DetachFromThread(Optick::EventStorage* storage) { Optick::FiberSyncData::DetachFromThread(storage); }

COPTICK_EXPORT Optick::EventDescription* Optick_EventDescription_Create(const char* eventName, const char* fileName, const uint32_t fileLine, const uint32_t eventColor, const uint32_t filter, const uint8_t eventFlags) {
    return Optick::EventDescription::Create(eventName, fileName, (unsigned long)fileLine, (unsigned long)eventColor, (unsigned long)filter, eventFlags);
}

COPTICK_EXPORT Optick::EventDescription* Optick_EventDescription_CreateShared(const char* eventName, const char* fileName, const uint32_t fileLine, const uint32_t eventColor, const uint32_t filter) {
    return Optick::EventDescription::CreateShared(eventName, fileName, fileLine, eventColor, filter);
}

COPTICK_EXPORT Optick::EventData* Optick_Event_Start(const Optick::EventDescription* description) { return Optick::Event::Start(*description); }
COPTICK_EXPORT void Optick_Event_Stop(Optick::EventData* data) { Optick::Event::Stop(*data); }

COPTICK_EXPORT void Optick_Event_Push_LPStr(const char* name) { Optick::Event::Push(name); }
COPTICK_EXPORT void Optick_Event_Push_Description(const Optick::EventDescription* description) { Optick::Event::Push(*description); }
COPTICK_EXPORT void Optick_Event_Pop() { Optick::Event::Pop(); }

COPTICK_EXPORT void Optick_Event_Add(Optick::EventStorage* storage, const Optick::EventDescription* description, int64_t timestampStart, int64_t timestampFinish) {
    Optick::Event::Add(storage, description, timestampStart, timestampFinish);
}

COPTICK_EXPORT void Optick_Event_Push_Storage(Optick::EventStorage* storage, const Optick::EventDescription* description, int64_t timestampStart) {
    Optick::Event::Push(storage, description, timestampStart);
}

COPTICK_EXPORT void Optick_Event_Pop_Storage(Optick::EventStorage* storage, int64_t timestampStart) { Optick::Event::Pop(storage, timestampStart); }

COPTICK_EXPORT Optick::EventData* Optick_GPUEvent_Start(const Optick::EventDescription* description) { return Optick::GPUEvent::Start(*description); }
COPTICK_EXPORT void Optick_GPUEvent_Stop(Optick::EventData* data) { Optick::GPUEvent::Stop(*data); }

COPTICK_EXPORT void Optick_Tag_Attach_Float(const Optick::EventDescription* description, float val) { Optick::Tag::Attach(*description, val); }
COPTICK_EXPORT void Optick_Tag_Attach_Int32(const Optick::EventDescription* description, int32_t val) { Optick::Tag::Attach(*description, val); }
COPTICK_EXPORT void Optick_Tag_Attach_UInt32(const Optick::EventDescription* description, uint32_t val) { Optick::Tag::Attach(*description, val); }
COPTICK_EXPORT void Optick_Tag_Attach_UInt64(const Optick::EventDescription* description, uint64_t val) { Optick::Tag::Attach(*description, val); }
COPTICK_EXPORT void Optick_Tag_Attach_LPStr(const Optick::EventDescription* description, const char* val) { Optick::Tag::Attach(*description, val); }

COPTICK_EXPORT void Optick_InitGpuD3D12(ID3D12Device* device, ID3D12CommandQueue** cmdQueues, uint32_t numQueues) { Optick::InitGpuD3D12(device, cmdQueues, numQueues); }
COPTICK_EXPORT void Optick_InitGpuVulkan(VkDevice* vkDevices, VkPhysicalDevice* vkPhysicalDevices, VkQueue* vkQueues, uint32_t* cmdQueuesFamily, uint32_t numQueues, const Optick::VulkanFunctions* functions) { Optick::InitGpuVulkan(vkDevices, vkPhysicalDevices, vkQueues, cmdQueuesFamily, numQueues, functions); }
COPTICK_EXPORT void Optick_GpuFlip(void* swapChain) { Optick::GpuFlip(swapChain); }

COPTICK_EXPORT void Optick_SetGpuContext(const Optick::GPUContext* newContext, Optick::GPUContext* oldContext) {
    Optick::GPUContext old = Optick::SetGpuContext(*newContext);
    if (oldContext != nullptr) {
        *oldContext = old;
    }
}

COPTICK_EXPORT const Optick::EventDescription* Optick_GetFrameDescription(Optick::FrameType::Type frame) { return Optick::GetFrameDescription(frame); }

COPTICK_EXPORT void Optick_SetAllocator(Optick::AllocateFn allocateFn, Optick::DeallocateFn deallocateFn, Optick::InitThreadCb initThreadCb) {
    Optick::SetAllocator(allocateFn, deallocateFn, initThreadCb);
}

COPTICK_EXPORT void Optick_Shutdown() { Optick::Shutdown(); }

COPTICK_EXPORT bool Optick_StartCapture(Optick::Mode::Type mode, int32_t samplingFrequency, bool force) { return Optick::StartCapture(mode, samplingFrequency, force); }
COPTICK_EXPORT bool Optick_StopCapture(bool force) { return Optick::StopCapture(force); }
COPTICK_EXPORT bool Optick_SaveCapture_Callback(Optick::CaptureSaveChunkCb dataCb, bool force) { return Optick::SaveCapture(dataCb, force); }
COPTICK_EXPORT bool Optick_SaveCapture_LPStr(const char* path, bool force) { return Optick::SaveCapture(path, force); }