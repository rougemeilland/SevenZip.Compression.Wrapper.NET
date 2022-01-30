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
            ref class ManagedCompressSetCoderPropertiesOpt
                : public ManagedCompressSetCoderPropertiesBase<ICompressSetCoderPropertiesOpt, &__UUIDOF(ICompressSetCoderPropertiesOpt)>, public NativeInterface::Compression::ICompressSetCoderPropertiesOpt
            {
            public:
                static ManagedCompressSetCoderPropertiesOpt^ Create(IUnknown* sevenZipObject);
                virtual void SetCoderPropertiesOpt(SevenZip::NativeInterface::ICoderPropertyGetter^ propertiesGetter);
            protected:
                virtual HRESULT SetNativeCoderProperties(const PROPID* propIDs, const PROPVARIANT* props, UInt32 numProps) override;
            };
        }
    }
}
