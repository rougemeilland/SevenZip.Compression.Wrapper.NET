#include "ManagedCompressSetFinishMode.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressSetFinishMode^ ManagedCompressSetFinishMode::Create(IUnknown* nativeUnknownObject)
            {
                bool success = false;
                ManagedCompressSetFinishMode^ managedInterfaceObject = nullptr;
                try
                {
                    managedInterfaceObject = gcnew ManagedCompressSetFinishMode();
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
                            managedInterfaceObject->~ManagedCompressSetFinishMode();
                    }
                    throw ex;
                }
            }

            void ManagedCompressSetFinishMode::SetFinishMode(bool finishMode)
            {
                HRESULT result = GetNativeInterfaceObject()->SetFinishMode(finishMode ? 1U : 0U);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
            }
        }
    }
}
