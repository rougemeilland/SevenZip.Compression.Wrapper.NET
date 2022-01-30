#include "NativeCompressProgressInfo.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            static GUID supportedInterfaceIds[] =
            {
                IID_IUnknown,
                __UUIDOF(SevenZipInterface::ICompressProgressInfo),
            };

            NativeCompressProgressInfo::NativeCompressProgressInfo(NativeInterface::Compression::CompressProgressInfoReporter^ progressReporter)
                : NativeUnknown(supportedInterfaceIds, sizeof(supportedInterfaceIds) / sizeof(supportedInterfaceIds[0]))
            {
                _delegateHandle = System::Runtime::InteropServices::GCHandle::Alloc(progressReporter);
                _progressReporter = static_cast<CompressProgressInfoReporter>(System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(progressReporter).ToPointer());
            }

            NativeCompressProgressInfo::~NativeCompressProgressInfo()
            {
                _delegateHandle.Free();
            }

            HRESULT STDMETHODCALLTYPE NativeCompressProgressInfo::SetRatioInfo(const UInt64* inSize, const UInt64* outSize)
            {
                _progressReporter(inSize, outSize);
                return S_OK;
            }

            HRESULT STDMETHODCALLTYPE NativeCompressProgressInfo::QueryInterface(REFIID riid, void** ppvObject)
            {
                return NativeUnknown::QueryInterface(riid, ppvObject);
            }

            ULONG STDMETHODCALLTYPE NativeCompressProgressInfo::AddRef(void)
            {
                return NativeUnknown::AddRef();
            }

            ULONG STDMETHODCALLTYPE NativeCompressProgressInfo::Release(void)
            {
                return NativeUnknown::Release();
            }
        }
    }
}
