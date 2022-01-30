#pragma once
#include "Unknown.h"

class CompressProgressInfo
    : public Unknown, public ICompressProgressInfo
{
private:
    CompressProgressInfoReporter _reporter;
public:
    CompressProgressInfo(CompressProgressInfoReporter reporter);
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
    virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
    virtual ULONG STDMETHODCALLTYPE Release(void) override;
    virtual HRESULT STDMETHODCALLTYPE SetRatioInfo(const UInt64* inSize, const UInt64* outSize) throw() override;
};

