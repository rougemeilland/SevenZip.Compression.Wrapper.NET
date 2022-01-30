#include "NativeSequentialInStream.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace IO
        {
            static GUID supportedInterfaceIds[] =
            {
                IID_IUnknown,
                __UUIDOF(SevenZipInterface::ISequentialInStream),
            };

            NativeSequentialInStream::NativeSequentialInStream(NativeInterface::IO::SequentialInStreamReader^ inStreamReader)
                :NativeUnknown(supportedInterfaceIds, sizeof(supportedInterfaceIds) / sizeof(supportedInterfaceIds[0]))
            {
                _delegateHandle = System::Runtime::InteropServices::GCHandle::Alloc(inStreamReader);
                _reader = static_cast<SequentialInStreamReader>(System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(inStreamReader).ToPointer());
                AddRef();
            }

            NativeSequentialInStream::~NativeSequentialInStream()
            {
                _delegateHandle.Free();
            }

            HRESULT STDMETHODCALLTYPE NativeSequentialInStream::Read(void* data, UInt32 size, UInt32* processedSize)
            {
                if (data == nullptr)
                    return E_INVALIDARG;
                if ((Int32)size < 0)
                    return (HRESULT)(0x80131502); // COR_E_ARGUMENTOUTOFRANGE
                if (processedSize == nullptr)
                    return E_INVALIDARG;
                Int32 length;
                HRESULT result = _reader(data, (Int32)size, &length);
                if (result == S_OK)
                    *processedSize = (UInt32)length;
                else
                    *processedSize = 0;
                return result;
            }

            HRESULT STDMETHODCALLTYPE NativeSequentialInStream::QueryInterface(REFIID riid, void** ppvObject)
            {
                return NativeUnknown::QueryInterface(riid, ppvObject);
            }

            ULONG STDMETHODCALLTYPE NativeSequentialInStream::AddRef(void)
            {
                return NativeUnknown::AddRef();
            }

            ULONG STDMETHODCALLTYPE NativeSequentialInStream::Release(void)
            {
                return NativeUnknown::Release();
            }
        }
    }
}
