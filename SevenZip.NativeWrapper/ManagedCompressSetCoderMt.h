#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressSetCoderMt
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressSetCoderMt, ICompressSetCoderMt)
            {
            public:
                static ManagedCompressSetCoderMt^ Create(IUnknown * nativeUnknownObject);
                virtual void SetNumberOfThreads(UInt32 numThreads);
            };
        }
    }
}
