#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressSetMemLimit
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressSetMemLimit, ICompressSetMemLimit)
            {
            public:
                static ManagedCompressSetMemLimit^ Create(IUnknown * nativeUnknownObject);
                virtual void SetMemLimit(UInt64 memUsage);
            };
        }
    }
}
