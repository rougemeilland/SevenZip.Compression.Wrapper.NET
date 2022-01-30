#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressReadUnusedFromInBuf
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressReadUnusedFromInBuf, ICompressReadUnusedFromInBuf)
            {
            public:
                static ManagedCompressReadUnusedFromInBuf^ Create(IUnknown * nativeUnknownObject);
                virtual UInt32 ReadUnusedFromInBuf(System::IntPtr data, UInt32 size);
            };
        }
    }
}
