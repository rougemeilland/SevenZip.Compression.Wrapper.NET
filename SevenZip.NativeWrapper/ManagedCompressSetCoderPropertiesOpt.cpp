#include "ManagedCompressSetCoderPropertiesOpt.h"
#include "ManagedCompressSetInStream.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetCoderPropertiesOpt^ ManagedCompressSetCoderPropertiesOpt::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetCoderPropertiesOpt^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetCoderPropertiesOpt();
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
                            managedInterfaceObject->~ManagedCompressSetCoderPropertiesOpt();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetCoderPropertiesOpt::SetCoderPropertiesOpt(SevenZip::NativeInterface::ICoderPropertyGetter^ propertiesGetter)
            {
                SetCommonCoderProperties(propertiesGetter, SevenZip::NativeInterface::CoderPropertyId::ExpectedDataSize);
            }

            HRESULT ManagedCompressSetCoderPropertiesOpt::SetNativeCoderProperties(const PROPID* propIDs, const PROPVARIANT* props, UInt32 numProps)
            {
                return GetNativeInterfaceObject()->SetCoderPropertiesOpt(propIDs, props, numProps);
            }
        }
    }
}
