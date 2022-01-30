#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressSetDecoderProperties2
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressSetDecoderProperties2, ICompressSetDecoderProperties2)
            {
            public:
                static ManagedCompressSetDecoderProperties2^ Create(IUnknown * nativeUnknownObject);
                virtual void SetDecoderProperties2(System::IntPtr data, Int32 length);
            };
        }
    }
}
