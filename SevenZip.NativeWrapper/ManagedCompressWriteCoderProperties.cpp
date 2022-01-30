#include "ManagedCompressWriteCoderProperties.h"
#include "NativeSequentialOutStream.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressWriteCoderProperties^ ManagedCompressWriteCoderProperties::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressWriteCoderProperties^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressWriteCoderProperties();
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
                            managedInterfaceObject->~ManagedCompressWriteCoderProperties();
                    }
                    throw ex;
                }
            }

            void ManagedCompressWriteCoderProperties::WriteCoderProperties(NativeInterface::IO::SequentialOutStreamWriter^ outStreamWriter)
            {
                IO::NativeSequentialOutStream outStream(outStreamWriter);
                outStream.AddRef(); // Call AddRef() to avoid deleting the auto variable
                HRESULT result = GetNativeInterfaceObject()->WriteCoderProperties(&outStream);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }
        }
    }
}
