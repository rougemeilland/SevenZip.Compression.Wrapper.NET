#include "ManagedSequentialInStream.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace IO
        {
            ManagedSequentialInStream^ ManagedSequentialInStream::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedSequentialInStream^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedSequentialInStream();
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
                            managedInterfaceObject->~ManagedSequentialInStream();
                    }
                    throw ex;
                }
            }

            UInt32 ManagedSequentialInStream::Read(System::IntPtr data, UInt32 size)
            {
                UInt32 processedCount;
                HRESULT result = GetNativeInterfaceObject()->Read(data.ToPointer(), size, &processedCount);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                return processedCount;
            }
        }
    }
}
