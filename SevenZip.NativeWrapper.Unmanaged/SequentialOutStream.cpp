#include "SequentialOutStream.h"

SequentialOutStream::SequentialOutStream(SequentialOutStreamWriter writer)
{
    _writer = writer;
}

HRESULT STDMETHODCALLTYPE SequentialOutStream::QueryInterface(REFIID riid, void** ppvObject)
{
    HRESULT result = Unknown::QueryInterface(riid, ppvObject);
    if (result != E_NOINTERFACE)
        return result;
    if (!IsEqualIID(riid, IID_ISequentialInStream))
    {
        *ppvObject = nullptr;
        return E_NOINTERFACE;
    }
    *ppvObject = this;
    return S_OK;
}

ULONG STDMETHODCALLTYPE SequentialOutStream::AddRef(void)
{
    return Unknown::AddRef();
}

ULONG STDMETHODCALLTYPE SequentialOutStream::Release(void)
{
    return Unknown::Release();
}

HRESULT STDMETHODCALLTYPE SequentialOutStream::Write(const void* data, UInt32 size, UInt32* processedSize) throw()
{
    return _writer(data, size, processedSize);
}
