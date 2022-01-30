#include "CompressProgressInfo.h"

CompressProgressInfo::CompressProgressInfo(CompressProgressInfoReporter reporter)
{
    _reporter = reporter;
}

HRESULT STDMETHODCALLTYPE CompressProgressInfo::QueryInterface(REFIID riid, void** ppvObject)
{
    HRESULT result = Unknown::QueryInterface(riid, ppvObject);
    if (result != E_NOINTERFACE)
        return result;
    if (!IsEqualIID(riid, IID_ICompressProgressInfo))
    {
        *ppvObject = nullptr;
        return E_NOINTERFACE;
    }
    *ppvObject = this;
    return S_OK;
}

ULONG STDMETHODCALLTYPE CompressProgressInfo::AddRef(void)
{
    return Unknown::AddRef();
}

ULONG STDMETHODCALLTYPE CompressProgressInfo::Release(void)
{
    return Unknown::Release();
}

HRESULT STDMETHODCALLTYPE CompressProgressInfo::SetRatioInfo(const UInt64* inSize, const UInt64* outSize) throw()
{
    _reporter(inSize, outSize);
    return S_OK;
}
