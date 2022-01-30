#pragma once

#include "Platform.h"
#include "ClrInterOperation.h"

#define __DEFINE_DEFAULT_MANAGED_INTERFACE(managedInterfaceName, nativeInterface) public SevenZip::NativeWrapper::ManagedUnknown<SevenZip::NativeWrapper::SevenZipInterface:: ## nativeInterface, &__UUIDOF(SevenZip::NativeWrapper::SevenZipInterface:: ## nativeInterface)>, public SevenZip::NativeInterface:: ## managedInterfaceName

namespace SevenZip
{
    namespace NativeWrapper
    {
        template <typename NATIVE_INTERFACE_T, const GUID* nativeInterfaceId>
        ref class ManagedUnknown
            : public SevenZip::NativeInterface::IUnknown
        {
        private:
            NATIVE_INTERFACE_T* _interfaceObject;
        protected:
            ManagedUnknown()
            {
                _interfaceObject = nullptr;
            }

        public:
            ManagedUnknown(const ManagedUnknown% p)
            {
                _interfaceObject = p._interfaceObject;
                if (_interfaceObject != nullptr)
                    _interfaceObject->AddRef();
            }

            ~ManagedUnknown()
            {
                this->!ManagedUnknown();
            }

            !ManagedUnknown()
            {
                if (_interfaceObject != nullptr)
                {
                    _interfaceObject->Release();
                    _interfaceObject = nullptr;
                }
            }

            virtual SevenZip::NativeInterface::IUnknown^ QueryInterface(System::Type^ interfaceType)
            {
                GUID interfaceId = FromManagedGuidToNativeGuid(interfaceType->GUID);
                SevenZip::NativeInterface::IUnknown^ interfaceObject = CreateManagedInterfaceObject(interfaceId, _interfaceObject);
                if (interfaceObject == nullptr)
                    throw gcnew System::NotSupportedException();
                return interfaceObject;
            }

        protected:
            NATIVE_INTERFACE_T* GetNativeInterfaceObject()
            {
                if (_interfaceObject == nullptr)
                    throw gcnew System::InvalidOperationException(L"\"_interfaceObject\" is not attached.");
                return _interfaceObject;
            }

            IUnknown* GetNativeUnknownObject() { return _interfaceObject; }

            bool AttatchNativeInterfaceObject(IUnknown* nativeUnknownObject)
            {
                try
                {
                    NATIVE_INTERFACE_T* interfaceObject = nullptr;
                    HRESULT result = nativeUnknownObject->QueryInterface(*nativeInterfaceId, (void**)&interfaceObject);
                    if (result != S_OK)
                    {
                        if (result == E_NOINTERFACE)
                            return false;
                        __ThrowExceptionForHR(result);
                    }
                    _interfaceObject = interfaceObject;
                    return true;
                }
                catch (System::Exception^ ex)
                {
                    this->~ManagedUnknown();
                    throw ex;
                }
            }
        };
    }
}
