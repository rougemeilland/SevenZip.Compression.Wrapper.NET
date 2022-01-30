#pragma once

#include "NativeUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            struct NativeCompressProgressInfo
                : public NativeUnknown, public SevenZipInterface::ICompressProgressInfo
            {
            private:
                typedef void(STDMETHODCALLTYPE* CompressProgressInfoReporter)(const UInt64* inSize, const UInt64* outSize);
                System::Runtime::InteropServices::GCHandle _delegateHandle;
                CompressProgressInfoReporter  _progressReporter;
                NativeCompressProgressInfo(const NativeCompressProgressInfo& p); // unused
            public:
                NativeCompressProgressInfo(SevenZip::NativeInterface::Compression::CompressProgressInfoReporter^ progressReporter);
                virtual ~NativeCompressProgressInfo();
                virtual HRESULT STDMETHODCALLTYPE SetRatioInfo(const UInt64* inSize, const UInt64* outSize) override;
                virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
                virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
                virtual ULONG STDMETHODCALLTYPE Release(void) override;
            };
        }
    }
}
