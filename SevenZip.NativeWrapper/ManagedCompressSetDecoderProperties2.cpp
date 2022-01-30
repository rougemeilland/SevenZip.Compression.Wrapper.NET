#include "ManagedCompressSetDecoderProperties2.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetDecoderProperties2^ ManagedCompressSetDecoderProperties2::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetDecoderProperties2^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetDecoderProperties2();
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
                            managedInterfaceObject->~ManagedCompressSetDecoderProperties2();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetDecoderProperties2::SetDecoderProperties2(System::IntPtr data, Int32 length)
            {
                HRESULT result = GetNativeInterfaceObject()->SetDecoderProperties2((Byte*)data.ToPointer(), length);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }
        }
    }
}
