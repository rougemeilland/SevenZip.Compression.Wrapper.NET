#pragma once

#include "Unknown.h"
#include "SevenZipEntryPoint.h"

class CompressCodecsInfo
    : public Unknown, public ICompressCodecsInfo
{
private:
    SevenZipEntryPoint* _entryPoint;
protected:
    CompressCodecsInfo();
public:
    virtual ~CompressCodecsInfo();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
    virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
    virtual ULONG STDMETHODCALLTYPE Release(void) override;
    static HRESULT Create(SevenZipEntryPoint::EntryPointsTable* entryPointsTable, UInt32 sizeOfEntryPointsTable, CompressCodecsInfo** obj);
    virtual HRESULT STDMETHODCALLTYPE GetNumMethods(UInt32* numMethods) throw() override;
    virtual HRESULT STDMETHODCALLTYPE GetProperty(UInt32 index, PROPID propID, PROPVARIANT* value) throw() override;
    virtual HRESULT STDMETHODCALLTYPE CreateDecoder(UInt32 index, const GUID* iid, void** coder) throw() override;
    virtual HRESULT STDMETHODCALLTYPE CreateEncoder(UInt32 index, const GUID* iid, void** coder) throw() override;
    virtual HRESULT STDMETHODCALLTYPE GetModuleProp(PROPID propID, PROPVARIANT* value) throw() override;
};

