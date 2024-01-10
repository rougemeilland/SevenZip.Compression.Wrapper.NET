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
    _referenceCount = 0;
    AddRef();
}

SevenZipEntryPoint::~SevenZipEntryPoint()
{
}

HRESULT SevenZipEntryPoint::Create(EntryPointsTable* entryPoints, UInt32 sizeOfEntryPointsTable, SevenZipEntryPoint** entryPoint)
{
    *entryPoint = nullptr;
    if (sizeof(*entryPoints) != sizeOfEntryPointsTable)
        return E_INVALIDARG;
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
    instance->_fpGetModuleProp = entryPoints->FpGetModuleProp;
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

HRESULT STDMETHODCALLTYPE SevenZipEntryPoint::GetModuleProp(PROPID propID, PROPVARIANT* value) const
{
    return (*_fpGetModuleProp)(propID, value);
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
