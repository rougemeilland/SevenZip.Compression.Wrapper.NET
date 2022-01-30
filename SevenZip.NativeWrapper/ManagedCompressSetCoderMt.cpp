#include "ManagedCompressSetCoderMt.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetCoderMt^ ManagedCompressSetCoderMt::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetCoderMt^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetCoderMt();
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
                            managedInterfaceObject->~ManagedCompressSetCoderMt();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetCoderMt::SetNumberOfThreads(UInt32 numThreads)
            {
                HRESULT result = GetNativeInterfaceObject()->SetNumberOfThreads(numThreads);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }
        }
    }
}
