#include "ManagedCompressSetMemLimit.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetMemLimit^ ManagedCompressSetMemLimit::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetMemLimit^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetMemLimit();
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
                            managedInterfaceObject->~ManagedCompressSetMemLimit();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetMemLimit::SetMemLimit(UInt64 memUsage)
            {
                HRESULT result = GetNativeInterfaceObject()->SetMemLimit(memUsage);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }
        }
    }
}
