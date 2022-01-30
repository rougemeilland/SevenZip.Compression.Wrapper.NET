#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressWriteCoderProperties
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressWriteCoderProperties, ICompressWriteCoderProperties)
            {
            public:
                static ManagedCompressWriteCoderProperties^ Create(IUnknown * nativeUnknownObject);
                virtual void WriteCoderProperties(SevenZip::NativeInterface::IO::SequentialOutStreamWriter^ outStreamWriter);
            };
        }
    }
}
