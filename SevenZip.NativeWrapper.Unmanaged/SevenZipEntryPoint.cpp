#include "SevenZipEntryPoint.h"

static const Int32 E_INVALIDOPERATION = 0x80131509;

extern "C"
{
    static void __stdcall PrintLog(const wchar_t* s)
    {
        wprintf(s);
    }
}

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

HRESULT SevenZipEntryPoint::Create(const wchar_t* locationPath, SevenZipEntryPoint** entryPoint)
{
    *entryPoint = nullptr;
    SevenZipEntryPoint* instance = new SevenZipEntryPoint();
    HRESULT result = instance->LoadSevenZipLibrary(locationPath);
    if (result != S_OK)
    {
        delete instance;
        return result;
    }
    *entryPoint = instance;
    return S_OK;
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::CreateDecoder(UInt32 index, const GUID* iid, void** outObject)
{
    return (*_fpCreateDecoder)(index, iid, outObject);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::CreateEncoder(UInt32 index, const GUID* iid, void** outObject)
{
    return (*_fpCreateEncoder)(index, iid, outObject);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::CreateObject(const GUID& clsid, const GUID& iid, void** outObject)
{
    return (*_fpCreateObject)(&clsid, &iid, outObject);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetHandlerProperty(PROPID propID, PROPVARIANT* value)
{
    return (*_fpGetHandlerProperty)(propID, value);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetHandlerProperty2(UInt32 formatIndex, PROPID propID, PROPVARIANT* value)
{
    return (*_fpGetHandlerProperty2)(formatIndex, propID, value);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetHashers(IHashers** hashers)
{
    return (*_fpGetHashers)(hashers);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetIsArc(UInt32 formatIndex, Func_IsArc* isArc)
{
    return (*_fpGetIsArc)(formatIndex, isArc);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetMethodProperty(UInt32 codecIndex, PROPID propID, PROPVARIANT* value)
{
    return (*_fpGetMethodProperty)(codecIndex, propID, value);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetNumberOfFormats(UINT32* numFormats)
{
    return (*_fpGetNumberOfFormats)(numFormats);
}

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetNumberOfMethods(UINT32* numCodecs)
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

HRESULT SevenZipEntryPoint::LoadSevenZipLibrary(const wchar_t* locationPath)
{
    // If an error occurs because the 7-zip library does not exist in the path specified by "locationPath", E_DLL_NOT_FOUND must be returned.
    const UInt32 E_DLL_NOT_FOUND = 0x8007007e;

    if (_dllHandle != nullptr)
        return E_INVALIDOPERATION;
#if defined(_PLATFORM_WINDOWS)
    HINSTANCE dllHandle = LoadLibraryW(locationPath);
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
    FuncGetIsArc fpGetIsArc = (FuncGetIsArc)GetProcAddress(dllHandle, "GetIsArc");
    if (fpGetIsArc == nullptr)
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
    FuncSetLogger fpSetLogger = (FuncSetLogger)GetProcAddress(dllHandle, "Debug__SetLogger");
    if (fpSetLogger != nullptr)
        (*fpSetLogger)(PrintLog);
    fpSetLogger = (FuncSetLogger)GetProcAddress(dllHandle, "Debug__SetLogger@4");
    if (fpSetLogger != nullptr)
        (*fpSetLogger)(PrintLog);
#elif defined(_PLATFORM_LINUX_X86) || defined(_PLATFORM_LINUX_X64)
#error "Write the code for symbol resolution of the dynamic link library (shared library) following the code for Windows above."
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
    _fpGetIsArc = fpGetIsArc;
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
#error "Write the code to unload the dynamic link library (shared library) by referring to the above code for Windows."
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
    _fpGetIsArc = nullptr;
    _fpGetMethodProperty = nullptr;
    _fpGetNumberOfFormats = nullptr;
    _fpGetNumberOfMethods = nullptr;
    _dllHandle = nullptr;
    _referenceCount = 0;
}
