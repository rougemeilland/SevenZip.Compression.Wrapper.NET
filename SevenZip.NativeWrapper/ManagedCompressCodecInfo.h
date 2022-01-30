#pragma once

#include "Platform.h"
#include "CoderType.h"
#include "NativeEntryPoint.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressCodecInfo
                : SevenZip::NativeInterface::Compression::ICompressCodecInfo
            {
            private:
                NativeEntryPoint* _nativeResource;
            public:
                ManagedCompressCodecInfo(NativeEntryPoint* nativeResource, Int32 index, UInt64 id, System::String^ coderName, System::Guid coderClassId, NativeInterface::CoderType coderType, bool isSupportedICompressCoder);
                ManagedCompressCodecInfo(ManagedCompressCodecInfo% p);
                ~ManagedCompressCodecInfo();
                !ManagedCompressCodecInfo();
                virtual property Int32 Index;
                virtual property UInt64 ID;
                virtual property System::String^ CodecName;
                virtual property System::Guid CoderClassId;
                virtual property SevenZip::NativeInterface::CoderType CoderType;
                virtual property bool IsSupportedICompressCoder;
                virtual SevenZip::NativeInterface::Compression::ICompressCoder^ CreateCompressCoder();
            };
        }
    }
}
