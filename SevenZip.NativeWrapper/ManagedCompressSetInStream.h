#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace IO
        {
            struct NativeSequentialInStream;
        }

        namespace Compression
        {
            ref class ManagedCompressSetInStream
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressSetInStream, ICompressSetInStream)
            {
            private:
                IO::NativeSequentialInStream* _inStream;
            public:
                ManagedCompressSetInStream();
                ManagedCompressSetInStream(const ManagedCompressSetInStream% p);
                ~ManagedCompressSetInStream();
                !ManagedCompressSetInStream();
                static ManagedCompressSetInStream^ Create(IUnknown * nativeUnknownObject);
                virtual void SetInStream(NativeInterface::IO::SequentialInStreamReader^ sequentialInStreamReader);
                virtual void ReleaseInStream();
            };
        }
    }
}
