#include "ManagedCompressReadUnusedFromInBuf.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressReadUnusedFromInBuf^ ManagedCompressReadUnusedFromInBuf::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressReadUnusedFromInBuf^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressReadUnusedFromInBuf();
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
                            managedInterfaceObject->~ManagedCompressReadUnusedFromInBuf();
                    }
                    throw ex;
                }
            }

            UInt32 ManagedCompressReadUnusedFromInBuf::ReadUnusedFromInBuf(System::IntPtr data, UInt32 size)
            {
                UInt32 length;
                HRESULT result = GetNativeInterfaceObject()->ReadUnusedFromInBuf(data.ToPointer(), size, &length);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                return length;
            }

        }
    }
}
