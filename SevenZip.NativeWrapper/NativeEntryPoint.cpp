#include "NativeEntryPoint.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        NativeEntryPoint::NativeEntryPoint(HINSTANCE dllHandle)
        {
            _dllHandle = dllHandle;
            _referenceCount = 0;
            AddRef();
        }

        NativeEntryPoint::~NativeEntryPoint()
        {
            _fpCreateDecoder = nullptr;
            _fpCreateEncoder = nullptr;
            _fpCreateObject = nullptr;
            _fpGetHandlerProperty = nullptr;
            _fpGetHandlerProperty2 = nullptr;
            _fpGetHashers = nullptr;
            _fpGetIsArc = nullptr;
            _fpGetMethodProperty = nullptr;
            _fpGetNumberOfFormats = nullptr;
            _fpGetNumberOfMethods = nullptr;
            FreeLibrary(_dllHandle);
            _referenceCount = 0;
        }

        NativeEntryPoint* NativeEntryPoint::Create()
        {
            bool success = false;
            NativeEntryPoint* instance = nullptr;
            try
            {
                HINSTANCE dllHandle = LoadLibraryA(__SEVEN_ZIP_NATIVE_MODULE_FILE_NAME);
                if (dllHandle == nullptr)
                    throw gcnew System::Exception("Can not load 7z");
                FuncCreateDecoder fpCreateDecoder = (FuncCreateDecoder)GetProcAddress(dllHandle, "CreateDecoder");
                if (fpCreateDecoder == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: CreateDecoder");
                FuncCreateEncoder fpCreateEncoder = (FuncCreateEncoder)GetProcAddress(dllHandle, "CreateEncoder");
                if (fpCreateEncoder == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: CreateEncoder");
                FuncCreateObject fpCreateObject = (FuncCreateObject)GetProcAddress(dllHandle, "CreateObject");
                if (fpCreateObject == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: CreateObject");
                FuncGetHandlerProperty fpGetHandlerProperty = (FuncGetHandlerProperty)GetProcAddress(dllHandle, "GetHandlerProperty");
                if (fpGetHandlerProperty == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: GetHandlerProperty");
                FuncGetHandlerProperty2 fpGetHandlerProperty2 = (FuncGetHandlerProperty2)GetProcAddress(dllHandle, "GetHandlerProperty2");
                if (fpGetHandlerProperty2 == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: GetHandlerProperty2");
                FuncGetHashers fpGetHashers = (FuncGetHashers)GetProcAddress(dllHandle, "GetHashers");
                if (fpGetHashers == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: GetHashers");
                FuncGetIsArc fpGetIsArc = (FuncGetIsArc)GetProcAddress(dllHandle, "GetIsArc");
                if (fpGetIsArc == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: GetIsArc");
                FuncGetMethodProperty fpGetMethodProperty = (FuncGetMethodProperty)GetProcAddress(dllHandle, "GetMethodProperty");
                if (fpGetMethodProperty == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: GetMethodProperty");
                FuncGetNumberOfFormats fpGetNumberOfFormats = (FuncGetNumberOfFormats)GetProcAddress(dllHandle, "GetNumberOfFormats");
                if (fpGetNumberOfFormats == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: GetNumberOfFormats");
                FuncGetNumberOfMethods fpGetNumberOfMethods = (FuncGetNumberOfMethods)GetProcAddress(dllHandle, "GetNumberOfMethods");
                if (fpGetNumberOfMethods == nullptr)
                    throw gcnew System::EntryPointNotFoundException(L"Not found: GetNumberOfMethods");
                instance = new NativeEntryPoint(dllHandle);
                instance->_fpCreateDecoder = fpCreateDecoder;
                instance->_fpCreateEncoder = fpCreateEncoder;
                instance->_fpCreateObject = fpCreateObject;
                instance->_fpGetHandlerProperty = fpGetHandlerProperty;
                instance->_fpGetHandlerProperty2 = fpGetHandlerProperty2;
                instance->_fpGetHashers = fpGetHashers;
                instance->_fpGetIsArc = fpGetIsArc;
                instance->_fpGetMethodProperty = fpGetMethodProperty;
                instance->_fpGetNumberOfFormats = fpGetNumberOfFormats;
                instance->_fpGetNumberOfMethods = fpGetNumberOfMethods;
                success = true;
                return instance;

            }
            finally
            {
                if (!success)
                {
                    if (instance != nullptr)
                        delete instance;
                }
            }
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::CreateDecoder(UInt32 index, const GUID* iid, void** outObject)
        {
            return (*_fpCreateDecoder)(index, iid, outObject);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::CreateEncoder(UInt32 index, const GUID* iid, void** outObject)
        {
            return (*_fpCreateEncoder)(index, iid, outObject);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::CreateObject(const GUID& clsid, const GUID& iid, void** outObject)
        {
            return (*_fpCreateObject)(&clsid, &iid, outObject);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::GetHandlerProperty(PROPID propID, PROPVARIANT* value)
        {
            return (*_fpGetHandlerProperty)(propID, value);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::GetHandlerProperty2(UInt32 formatIndex, PROPID propID, PROPVARIANT* value)
        {
            return (*_fpGetHandlerProperty2)(formatIndex, propID, value);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::GetHashers(SevenZipInterface::IHashers** hashers)
        {
            return (*_fpGetHashers)(hashers);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::GetIsArc(UInt32 formatIndex, SevenZipInterface::Func_IsArc* isArc)
        {
            return (*_fpGetIsArc)(formatIndex, isArc);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::GetMethodProperty(UInt32 codecIndex, PROPID propID, PROPVARIANT* value)
        {
            return (*_fpGetMethodProperty)(codecIndex, propID, value);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::GetNumberOfFormats(UINT32* numFormats)
        {
            return (*_fpGetNumberOfFormats)(numFormats);
        }

        HRESULT STDMETHODCALLTYPE NativeEntryPoint::GetNumberOfMethods(UINT32* numCodecs)
        {
            return (*_fpGetNumberOfMethods)(numCodecs);
        }

        void NativeEntryPoint::AddRef()
        {
            InterlockedIncrement(&_referenceCount);
        }

        void NativeEntryPoint::Release()
        {
            if (InterlockedDecrement(&_referenceCount) <= 0)
                delete this;
        }
    }
}
