#pragma once

#include "SevenZipInterface.h"

struct Unknown
    : public IUnknown
{
private:
    UInt32 _referenceCounter;
    Unknown(const Unknown& p);
protected:
    Unknown();
public:
    virtual ~Unknown();
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
    virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
    virtual ULONG STDMETHODCALLTYPE Release(void) override;
};
