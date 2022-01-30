#include "ManagedCompressSetBufSize.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetBufSize^ ManagedCompressSetBufSize::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetBufSize^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetBufSize();
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
                            managedInterfaceObject->~ManagedCompressSetBufSize();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetBufSize::SetInBufSize(UInt32 streamIndex, UInt32 size)
            {
                HRESULT result = GetNativeInterfaceObject()->SetInBufSize(streamIndex, size);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }

            void ManagedCompressSetBufSize::SetOutBufSize(UInt32 streamIndex, UInt32 size)
            {
                HRESULT result = GetNativeInterfaceObject()->SetOutBufSize(streamIndex, size);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }
        }
    }
}
