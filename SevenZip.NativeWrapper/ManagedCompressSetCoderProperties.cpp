#include "ManagedCompressSetCoderProperties.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetCoderProperties^ ManagedCompressSetCoderProperties::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetCoderProperties^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetCoderProperties();
                    if (!managedInterfaceObject->AttatchNativeInterfaceObject(nativeUnknownObject))
                        return nullptr;
                    success = true;
                    return managedInterfaceObject;
                }
                catch (System::Exception^ ex)
                {
                    if (!success)
                    {
                        if (managedInterfaceObject != nullptr)
                            managedInterfaceObject->~ManagedCompressSetCoderProperties();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetCoderProperties::SetCoderProperties(SevenZip::NativeInterface::ICoderPropertyGetter^ propertiesGetter)
            {
                SetCommonCoderProperties(propertiesGetter, ~SevenZip::NativeInterface::CoderPropertyId::ExpectedDataSize);
            }

            HRESULT ManagedCompressSetCoderProperties::SetNativeCoderProperties(const PROPID* propIDs, const PROPVARIANT* props, UInt32 numProps)
            {
                return GetNativeInterfaceObject()->SetCoderProperties(propIDs, props, numProps);
            }
        }
    }
}
