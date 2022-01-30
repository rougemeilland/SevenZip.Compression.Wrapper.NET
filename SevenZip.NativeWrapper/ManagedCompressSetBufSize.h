#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressSetBufSize
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressSetBufSize, ICompressSetBufSize)
            {
            public:
                static ManagedCompressSetBufSize^ Create(IUnknown * nativeUnknownObject);
                virtual void SetInBufSize(UInt32 streamIndex, UInt32 size);
                virtual void SetOutBufSize(UInt32 streamIndex, UInt32 size);
            };
        }
    }
}
