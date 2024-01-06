#include "SequentialOutStream.h"

SequentialOutStream::SequentialOutStream(SequentialOutStreamWriter writer, bool allocatedStatically)
{
    _writer = writer;
    _allocatedStatically = allocatedStatically;
    if (!allocatedStatically)
        AddRef();
}

HRESULT STDMETHODCALLTYPE SequentialOutStream::QueryInterface(REFIID riid, void** ppvObject)
{
    HRESULT result = Unknown::QueryInterface(riid, ppvObject);
    if (result != E_NOINTERFACE)
        return result;
    if (IsEqualIID(riid, IID_ISequentialInStream) || IsEqualIID(riid, IID_IUnknown))
    {
        *ppvObject = this;
        return S_OK;
    }
    *ppvObject = nullptr;
    return E_NOINTERFACE;
}

ULONG STDMETHODCALLTYPE SequentialOutStream::AddRef(void)
{
    if (_allocatedStatically)
        return ULONG_MAX;
    return Unknown::AddRef();
}

ULONG STDMETHODCALLTYPE SequentialOutStream::Release(void)
{
    if (_allocatedStatically)
        return ULONG_MAX;
    return Unknown::Release();
}

HRESULT STDMETHODCALLTYPE SequentialOutStream::Write(const void* data, UInt32 size, UInt32* processedSize) throw()
{
    return _writer(data, size, processedSize);
}
