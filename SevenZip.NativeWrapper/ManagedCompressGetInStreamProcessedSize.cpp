#include "ManagedCompressGetInStreamProcessedSize.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressGetInStreamProcessedSize^ ManagedCompressGetInStreamProcessedSize::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressGetInStreamProcessedSize^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressGetInStreamProcessedSize();
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
                            managedInterfaceObject->~ManagedCompressGetInStreamProcessedSize();
                    }
                    throw ex;
                }
            }

            UInt64 ManagedCompressGetInStreamProcessedSize::InStreamProcessedSize::get()
            {
                UInt64 inStreamProcessedSize;
                HRESULT result = GetNativeInterfaceObject()->GetInStreamProcessedSize(&inStreamProcessedSize);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                return inStreamProcessedSize;
            }
        }
    }
}
