#pragma once

#include "ManagedCompressSetCoderPropertiesBase.h"
#include "SevenZipInterface.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            using namespace SevenZip::NativeWrapper::SevenZipInterface;
            ref class ManagedCompressSetCoderProperties
                : public ManagedCompressSetCoderPropertiesBase<ICompressSetCoderProperties, &__UUIDOF(ICompressSetCoderProperties)>, public SevenZip::NativeInterface::Compression::ICompressSetCoderProperties
            {
            public:
                static ManagedCompressSetCoderProperties^ Create(IUnknown* sevenZipObject);
                virtual void SetCoderProperties(SevenZip::NativeInterface::ICoderPropertyGetter^ propertiesGetter);
            protected:
                virtual HRESULT SetNativeCoderProperties(const PROPID* propIDs, const PROPVARIANT* props, UInt32 numProps) override;
            };
        }
    }
}
