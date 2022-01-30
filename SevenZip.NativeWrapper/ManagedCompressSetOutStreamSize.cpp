#include "ManagedCompressSetOutStreamSize.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetOutStreamSize^ ManagedCompressSetOutStreamSize::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetOutStreamSize^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetOutStreamSize();
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
                            managedInterfaceObject->~ManagedCompressSetOutStreamSize();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetOutStreamSize::SetOutStreamSize(System::Nullable<UInt64> outSize)
            {
                UInt64 outSizeBuffer;
                HRESULT result = GetNativeInterfaceObject()->SetOutStreamSize(FromNullableUInt64ToUInt64Pointer(outSize, &outSizeBuffer));
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }
        }
    }
}
