#include <stdio.h>
#include <wchar.h>
#ifdef _MSC_VER
#include <windows.h>
#elif defined(__GNUC__)
#include <dlfcn.h>
#else
#endif
#include "SevenZipEntryPoint.h"

static const Int32 E_INVALIDOPERATION = 0x80131509;

SevenZipEntryPoint::SevenZipEntryPoint()
{
    _dllHandle = nullptr;
    _referenceCount = 0;
    AddRef();
}

SevenZipEntryPoint::~SevenZipEntryPoint()
{
    UnloadSevenZipLibrary();
}

HRESULT SevenZipEntryPoint::Create(EntryPointsTable* entryPoints, SevenZipEntryPoint** entryPoint)
{
    *entryPoint = nullptr;
    SevenZipEntryPoint* instance = new SevenZipEntryPoint();
    instance->_fpCreateDecoder = entryPoints->FpCreateDecoder;
    instance->_fpCreateEncoder = entryPoints->FpCreateEncoder;
    instance->_fpCreateObject = entryPoints->FpCreateObject;
    instance->_fpGetHandlerProperty = entryPoints->FpGetHandlerProperty;
    instance->_fpGetHandlerProperty2 = entryPoints->FpGetHandlerProperty2;
    instance->_fpGetHashers = entryPoints->FpGetHashers;
    instance->_fpGetMethodProperty = entryPoints->FpGetMethodProperty;
    instance->_fpGetNumberOfFormats = entryPoints->FpGetNumberOfFormats;
    instance->_fpGetNumberOfMethods = entryPoints->FpGetNumberOfMethods;
    *entryPoint = instance;
    return S_OK;
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::CreateDecoder(UInt32 index, const GUID* iid, void** outObject) const
{
    return (*_fpCreateDecoder)(index, iid, outObject);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::CreateEncoder(UInt32 index, const GUID* iid, void** outObject) const
{
    return (*_fpCreateEncoder)(index, iid, outObject);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::CreateObject(const GUID& clsid, const GUID& iid, void** outObject) const
{
    return (*_fpCreateObject)(&clsid, &iid, outObject);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetHandlerProperty(PROPID propID, PROPVARIANT* value) const
{
    return (*_fpGetHandlerProperty)(propID, value);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetHandlerProperty2(UInt32 formatIndex, PROPID propID, PROPVARIANT* value) const
{
    return (*_fpGetHandlerProperty2)(formatIndex, propID, value);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetHashers(IHashers** hashers) const
{
    return (*_fpGetHashers)(hashers);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetMethodProperty(UInt32 codecIndex, PROPID propID, PROPVARIANT* value) const
{
    return (*_fpGetMethodProperty)(codecIndex, propID, value);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetNumberOfFormats(UInt32* numFormats) const
{
    return (*_fpGetNumberOfFormats)(numFormats);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetNumberOfMethods(UInt32* numCodecs) const
{
    return (*_fpGetNumberOfMethods)(numCodecs);
}

void SevenZipEntryPoint::AddRef()
{
    InterlockedIncrement(&_referenceCount);
}

void SevenZipEntryPoint::Release()
{
    if (InterlockedDecrement(&_referenceCount) <= 0)
        delete this;
}

HRESULT SevenZipEntryPoint::LoadSevenZipLibrary(Byte* locationPath)
{
    if (_dllHandle != nullptr)
        return E_INVALIDOPERATION;
#if defined(_PLATFORM_WINDOWS)
    HINSTANCE dllHandle = LoadLibraryW((LPCWSTR)locationPath);
    if (dllHandle == nullptr)
    {
        return
            GetLastError() == ERROR_MOD_NOT_FOUND
            ? E_DLL_NOT_FOUND
            : AtlHresultFromLastError();
    }
    FuncCreateDecoder fpCreateDecoder = (FuncCreateDecoder)GetProcAddress(dllHandle, "CreateDecoder");
    if (fpCreateDecoder == nullptr)
        return AtlHresultFromLastError();
    FuncCreateEncoder fpCreateEncoder = (FuncCreateEncoder)GetProcAddress(dllHandle, "CreateEncoder");
    if (fpCreateEncoder == nullptr)
        return AtlHresultFromLastError();
    FuncCreateObject fpCreateObject = (FuncCreateObject)GetProcAddress(dllHandle, "CreateObject");
    if (fpCreateObject == nullptr)
        return AtlHresultFromLastError();
    FuncGetHandlerProperty fpGetHandlerProperty = (FuncGetHandlerProperty)GetProcAddress(dllHandle, "GetHandlerProperty");
    if (fpGetHandlerProperty == nullptr)
        return AtlHresultFromLastError();
    FuncGetHandlerProperty2 fpGetHandlerProperty2 = (FuncGetHandlerProperty2)GetProcAddress(dllHandle, "GetHandlerProperty2");
    if (fpGetHandlerProperty2 == nullptr)
        return AtlHresultFromLastError();
    FuncGetHashers fpGetHashers = (FuncGetHashers)GetProcAddress(dllHandle, "GetHashers");
    if (fpGetHashers == nullptr)
        return AtlHresultFromLastError();
    FuncGetMethodProperty fpGetMethodProperty = (FuncGetMethodProperty)GetProcAddress(dllHandle, "GetMethodProperty");
    if (fpGetMethodProperty == nullptr)
        return AtlHresultFromLastError();
    FuncGetNumberOfFormats fpGetNumberOfFormats = (FuncGetNumberOfFormats)GetProcAddress(dllHandle, "GetNumberOfFormats");
    if (fpGetNumberOfFormats == nullptr)
        return AtlHresultFromLastError();
    FuncGetNumberOfMethods fpGetNumberOfMethods = (FuncGetNumberOfMethods)GetProcAddress(dllHandle, "GetNumberOfMethods");
    if (fpGetNumberOfMethods == nullptr)
        return AtlHresultFromLastError();
#elif defined(_PLATFORM_LINUX_X86) || defined(_PLATFORM_LINUX_X64)
    void* dllHandle = dlopen((const char*)locationPath, RTLD_LAZY | RTLD_LOCAL);
    if (dllHandle == nullptr)
        return E_DLL_NOT_FOUND;
    FuncCreateDecoder fpCreateDecoder = (FuncCreateDecoder)dlsym(dllHandle, "CreateDecoder");
    if (fpCreateDecoder == nullptr)
        return E_NOTIMPL;
    FuncCreateEncoder fpCreateEncoder = (FuncCreateEncoder)dlsym(dllHandle, "CreateEncoder");
    if (fpCreateEncoder == nullptr)
        return E_NOTIMPL;
    FuncCreateObject fpCreateObject = (FuncCreateObject)dlsym(dllHandle, "CreateObject");
    if (fpCreateObject == nullptr)
        return E_NOTIMPL;
    FuncGetHandlerProperty fpGetHandlerProperty = (FuncGetHandlerProperty)dlsym(dllHandle, "GetHandlerProperty");
    if (fpGetHandlerProperty == nullptr)
        return E_NOTIMPL;
    FuncGetHandlerProperty2 fpGetHandlerProperty2 = (FuncGetHandlerProperty2)dlsym(dllHandle, "GetHandlerProperty2");
    if (fpGetHandlerProperty2 == nullptr)
        return E_NOTIMPL;
    FuncGetHashers fpGetHashers = (FuncGetHashers)dlsym(dllHandle, "GetHashers");
    if (fpGetHashers == nullptr)
        return E_NOTIMPL;
    FuncGetMethodProperty fpGetMethodProperty = (FuncGetMethodProperty)dlsym(dllHandle, "GetMethodProperty");
    if (fpGetMethodProperty == nullptr)
        return E_NOTIMPL;
    FuncGetNumberOfFormats fpGetNumberOfFormats = (FuncGetNumberOfFormats)dlsym(dllHandle, "GetNumberOfFormats");
    if (fpGetNumberOfFormats == nullptr)
        return E_NOTIMPL;
    FuncGetNumberOfMethods fpGetNumberOfMethods = (FuncGetNumberOfMethods)dlsym(dllHandle, "GetNumberOfMethods");
    if (fpGetNumberOfMethods == nullptr)
        return E_NOTIMPL;
#elif defined(_PLATFORM_MACOS_X86) || defined(_PLATFORM_MACOS_X64)
#error "Write the code for symbol resolution of the dynamic link library (shared library) following the code for Windows above."
#else
#error "Unsupported platform."
#endif
    _fpCreateDecoder = fpCreateDecoder;
    _fpCreateEncoder = fpCreateEncoder;
    _fpCreateObject = fpCreateObject;
    _fpGetHandlerProperty = fpGetHandlerProperty;
    _fpGetHandlerProperty2 = fpGetHandlerProperty2;
    _fpGetHashers = fpGetHashers;
    _fpGetMethodProperty = fpGetMethodProperty;
    _fpGetNumberOfFormats = fpGetNumberOfFormats;
    _fpGetNumberOfMethods = fpGetNumberOfMethods;
    _dllHandle = dllHandle;
    return S_OK;
}

void SevenZipEntryPoint::UnloadSevenZipLibrary()
{
    if (_dllHandle == nullptr)
        return;
#if defined(_PLATFORM_WINDOWS)
    FreeLibrary((HINSTANCE)_dllHandle);
#elif defined(_PLATFORM_LINUX_X86) || defined(_PLATFORM_LINUX_X64)
    dlclose(_dllHandle);
#elif defined(_PLATFORM_MACOS_X86) || defined(_PLATFORM_MACOS_X64)
#error "Write the code to unload the dynamic link library (shared library) by referring to the above code for Windows."
#else
#error "Unsupported platform."
#endif
    _fpCreateDecoder = nullptr;
    _fpCreateEncoder = nullptr;
    _fpCreateObject = nullptr;
    _fpGetHandlerProperty = nullptr;
    _fpGetHandlerProperty2 = nullptr;
    _fpGetHashers = nullptr;
    _fpGetMethodProperty = nullptr;
    _fpGetNumberOfFormats = nullptr;
    _fpGetNumberOfMethods = nullptr;
    _dllHandle = nullptr;
    _referenceCount = 0;
}
