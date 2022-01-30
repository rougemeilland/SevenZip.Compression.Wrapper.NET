#include "Unknown.h"

Unknown::Unknown()
{
    _referenceCounter = 0;
    AddRef();
}

Unknown::~Unknown()
{
}

HRESULT STDMETHODCALLTYPE Unknown::QueryInterface(REFIID riid, void** ppvObject)
{
    if (!IsEqualIID(riid, IID_IUnknown))
    {
        *ppvObject = nullptr;
        return E_NOINTERFACE;
    }
    *ppvObject = this;
    return S_OK;
}

ULONG STDMETHODCALLTYPE Unknown::AddRef(void)
{
    return InterlockedIncrement(&_referenceCounter);
}

ULONG STDMETHODCALLTYPE Unknown::Release(void)
{
    UInt32 count = InterlockedDecrement(&_referenceCounter);
    if (count <= 0)
        delete this;
    return count;
}
