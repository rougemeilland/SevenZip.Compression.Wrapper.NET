#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressCoder
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressCoder, ICompressCoder)
            {
            public:
                static ManagedCompressCoder^ Create(IUnknown * nativeUnknownObject);
                virtual void Code(SevenZip::NativeInterface::IO::SequentialInStreamReader ^ sequentialInStreamReader, SevenZip::NativeInterface::IO::SequentialOutStreamWriter ^ sequentialOutStreamWriter, System::Nullable<UInt64> inSize, System::Nullable<UInt64> outSize, SevenZip::NativeInterface::Compression::CompressProgressInfoReporter ^ progressReporter);
            };
        }
    }
}
