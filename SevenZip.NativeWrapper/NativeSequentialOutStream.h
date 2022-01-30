#pragma once

#include "NativeUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace IO
        {
            struct NativeSequentialOutStream
                : public NativeUnknown, public SevenZipInterface::ISequentialOutStream
            {
            private:
                typedef HRESULT(STDMETHODCALLTYPE* SequentialOutStreamWriter)(const void* buffer, Int32 size, Int32* processedSize);
                System::Runtime::InteropServices::GCHandle _delegateHandle;
                SequentialOutStreamWriter _writer;
                NativeSequentialOutStream(const NativeSequentialOutStream& p); // unused
            public:
                NativeSequentialOutStream(SevenZip::NativeInterface::IO::SequentialOutStreamWriter^ outStreamWriter);
                virtual ~NativeSequentialOutStream();
                virtual HRESULT STDMETHODCALLTYPE Write(const void* data, UInt32 size, UInt32* processedSize) override;
                virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
                virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
                virtual ULONG STDMETHODCALLTYPE Release(void) override;
            };
        }
    }
}
