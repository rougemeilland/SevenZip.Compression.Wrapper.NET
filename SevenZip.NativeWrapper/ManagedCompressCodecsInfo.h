#pragma once

#include "SevenZipInterface.h"
#include "NativeEntryPoint.h"
#include "ManagedCompressCoder.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            public ref class ManagedCompressCodecsInfo
                : public SevenZip::NativeInterface::Compression::ICompressCodecsInfo
            {
            private:
                NativeEntryPoint* _entryPoint;
            public:
                ManagedCompressCodecsInfo();
                ManagedCompressCodecsInfo(const ManagedCompressCodecsInfo% p);
                ~ManagedCompressCodecsInfo();
                !ManagedCompressCodecsInfo();
                virtual void Initialize();
                virtual SevenZip::NativeInterface::IUnknown^ QueryInterface(System::Type^ interfaceType);
                virtual void EnumerateCodecs(SevenZip::NativeInterface::Compression::CompressCodecInfoGetter^ compressCodecInfoGetter);
            };
        }
    }
}
