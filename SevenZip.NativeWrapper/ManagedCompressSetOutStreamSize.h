#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressSetOutStreamSize
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressSetOutStreamSize, ICompressSetOutStreamSize)
            {
            public:
                static ManagedCompressSetOutStreamSize^ Create(IUnknown * nativeUnknownObject);
                virtual void SetOutStreamSize(System::Nullable<UInt64> outSize);
            };
        }
    }
}
