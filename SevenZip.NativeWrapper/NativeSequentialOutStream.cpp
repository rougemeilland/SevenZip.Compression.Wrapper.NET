#include "NativeSequentialOutStream.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace IO
        {
            static GUID supportedInterfaceIds[] =
            {
                IID_IUnknown,
                __UUIDOF(SevenZipInterface::ISequentialOutStream),
            };

            NativeSequentialOutStream::NativeSequentialOutStream(NativeInterface::IO::SequentialOutStreamWriter^ outStreamWriter)
                : NativeUnknown(supportedInterfaceIds, sizeof(supportedInterfaceIds) / sizeof(supportedInterfaceIds[0]))
            {
                _delegateHandle = System::Runtime::InteropServices::GCHandle::Alloc(outStreamWriter);
                _writer = static_cast<SequentialOutStreamWriter>(System::Runtime::InteropServices::Marshal::GetFunctionPointerForDelegate(outStreamWriter).ToPointer());
                AddRef();
            }

            NativeSequentialOutStream::~NativeSequentialOutStream()
            {
                _delegateHandle.Free();
            }

            HRESULT STDMETHODCALLTYPE NativeSequentialOutStream::Write(const void* data, UInt32 size, UInt32* processedSize)
            {
                if (data == nullptr)
                    return E_INVALIDARG;
                if ((Int32)size < 0)
                    return (HRESULT)(0x80131502); // COR_E_ARGUMENTOUTOFRANGE
                if (processedSize == nullptr)
                    return E_INVALIDARG;
                Int32 length;
                HRESULT result = _writer(data, (Int32)size, &length);
                if (result == S_OK)
                    *processedSize = (UInt32)length;
                else
                    *processedSize = 0;
                return result;
            }

            HRESULT STDMETHODCALLTYPE NativeSequentialOutStream::QueryInterface(REFIID riid, void** ppvObject)
            {
                return NativeUnknown::QueryInterface(riid, ppvObject);
            }

            ULONG STDMETHODCALLTYPE NativeSequentialOutStream::AddRef(void)
            {
                return NativeUnknown::AddRef();
            }

            ULONG STDMETHODCALLTYPE NativeSequentialOutStream::Release(void)
            {
                return NativeUnknown::Release();
            }
        }
    }
}
