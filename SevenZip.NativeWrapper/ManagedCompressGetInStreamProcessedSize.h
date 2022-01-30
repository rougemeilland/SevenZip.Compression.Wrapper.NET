#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressGetInStreamProcessedSize
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressGetInStreamProcessedSize, ICompressGetInStreamProcessedSize)
            {
            public:
                static ManagedCompressGetInStreamProcessedSize^ Create(IUnknown * nativeUnknownObject);
                virtual property UInt64 InStreamProcessedSize { UInt64 get(); };
            };
        }
    }
}
