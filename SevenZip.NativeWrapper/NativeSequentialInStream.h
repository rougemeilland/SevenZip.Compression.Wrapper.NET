#pragma once

#include "NativeUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace IO
        {
            struct NativeSequentialInStream
                : public NativeUnknown, public SevenZipInterface::ISequentialInStream
            {
            private:
                typedef HRESULT(STDMETHODCALLTYPE* SequentialInStreamReader)(void* buffer, Int32 size, Int32* processedSize);
                System::Runtime::InteropServices::GCHandle _delegateHandle;
                SequentialInStreamReader _reader;
                NativeSequentialInStream(const NativeSequentialInStream& p); // unused
            public:
                NativeSequentialInStream(SevenZip::NativeInterface::IO::SequentialInStreamReader^ inStreamReader);
                virtual ~NativeSequentialInStream();
                virtual HRESULT STDMETHODCALLTYPE Read(void* data, UInt32 size, UInt32* processedSize) override;
                virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
                virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
                virtual ULONG STDMETHODCALLTYPE Release(void) override;
            };
        }
    }
}
