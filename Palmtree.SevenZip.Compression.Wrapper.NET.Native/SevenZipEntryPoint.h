#pragma once

#include "SevenZipInterface.h"

class SevenZipEntryPoint
{
private:
    struct IHashers;
    typedef HRESULT(STDMETHODCALLTYPE* FuncCreateDecoder)(UInt32 index, const GUID* iid, void** outObject);
    typedef HRESULT(STDMETHODCALLTYPE* FuncCreateEncoder)(UInt32 index, const GUID* iid, void** outObject);
    typedef HRESULT(STDMETHODCALLTYPE* FuncCreateObject)(const GUID* clsid, const GUID* iid, void** outObject);
    typedef HRESULT(STDMETHODCALLTYPE* FuncGetHandlerProperty)(PROPID propID, PROPVARIANT* value);
    typedef HRESULT(STDMETHODCALLTYPE* FuncGetHandlerProperty2)(UInt32 formatIndex, PROPID propID, PROPVARIANT* value);
    typedef HRESULT(STDMETHODCALLTYPE* FuncGetHashers)(IHashers** hashers);
    typedef HRESULT(STDMETHODCALLTYPE* FuncGetMethodProperty)(UInt32 codecIndex, PROPID propID, PROPVARIANT* value);
    typedef HRESULT(STDMETHODCALLTYPE* FuncGetNumberOfFormats)(UInt32* numFormats);
    typedef HRESULT(STDMETHODCALLTYPE* FuncGetNumberOfMethods)(UInt32* numCodecs);

    void* _dllHandle;
    UInt32 _referenceCount;
    FuncCreateDecoder _fpCreateDecoder;
    FuncCreateEncoder _fpCreateEncoder;
    FuncCreateObject _fpCreateObject;
    FuncGetHandlerProperty _fpGetHandlerProperty;
    FuncGetHandlerProperty2 _fpGetHandlerProperty2;
    FuncGetHashers _fpGetHashers;
    FuncGetMethodProperty _fpGetMethodProperty;
    FuncGetNumberOfFormats _fpGetNumberOfFormats;
    FuncGetNumberOfMethods _fpGetNumberOfMethods;
    SevenZipEntryPoint();
    SevenZipEntryPoint(const SevenZipEntryPoint& p); // unused

public:
    struct EntryPointsTable
    {
        /* 00 */FuncCreateDecoder FpCreateDecoder;
        /* 01 */FuncCreateEncoder FpCreateEncoder;
        /* 02 */FuncCreateObject FpCreateObject;
        /* 03 */FuncGetHandlerProperty FpGetHandlerProperty;
        /* 04 */FuncGetHandlerProperty2 FpGetHandlerProperty2;
        /* 05 */FuncGetHashers FpGetHashers;
        /* 06 */FuncGetMethodProperty FpGetMethodProperty;
        /* 07 */FuncGetNumberOfFormats FpGetNumberOfFormats;
        /* 08 */FuncGetNumberOfMethods FpGetNumberOfMethods;
    };

    virtual ~SevenZipEntryPoint();
    static HRESULT Create(EntryPointsTable* entryPointsTable, SevenZipEntryPoint** entryPoint);
    HRESULT STDMETHODCALLTYPE CreateDecoder(UInt32 index, const GUID* iid, void** outObject) const;
    HRESULT STDMETHODCALLTYPE CreateEncoder(UInt32 index, const GUID* iid, void** outObject) const;
    HRESULT STDMETHODCALLTYPE CreateObject(const GUID& clsid, const GUID& iid, void** outObject) const;
    HRESULT STDMETHODCALLTYPE GetHandlerProperty(PROPID propID, PROPVARIANT* value) const;
    HRESULT STDMETHODCALLTYPE GetHandlerProperty2(UInt32 formatIndex, PROPID propID, PROPVARIANT* value) const;
    HRESULT STDMETHODCALLTYPE GetHashers(IHashers** hashers) const;
    HRESULT STDMETHODCALLTYPE GetMethodProperty(UInt32 codecIndex, PROPID propID, PROPVARIANT* value) const;
    HRESULT STDMETHODCALLTYPE GetNumberOfFormats(UInt32* numFormats) const;
    HRESULT STDMETHODCALLTYPE GetNumberOfMethods(UInt32* numCodecs) const;
    void AddRef();
    void Release();
private:
    HRESULT LoadSevenZipLibrary(Byte* locationPath);
    void UnloadSevenZipLibrary();
};
