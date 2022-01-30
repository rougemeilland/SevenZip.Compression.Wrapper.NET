#pragma once

#include "ManagedUnknown.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ref class ManagedCompressSetFinishMode
                : __DEFINE_DEFAULT_MANAGED_INTERFACE(Compression::ICompressSetFinishMode, ICompressSetFinishMode)
            {
            public:
                static ManagedCompressSetFinishMode^ Create(IUnknown * nativeUnknownObject);
                virtual void SetFinishMode(bool finishMode);
            };
        }
    }
}
