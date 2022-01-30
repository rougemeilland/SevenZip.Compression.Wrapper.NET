#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace IO
        {
            ref class ManagedSequentialInStream
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(IO::ISequentialInStream, ISequentialInStream)
            {
            public:
                static ManagedSequentialInStream^ Create(IUnknown * nativeUnknownObject);
                virtual UInt32 Read(System::IntPtr data, UInt32 size);
            };
        }
    }
}
