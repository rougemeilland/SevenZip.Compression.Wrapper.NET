#include "SequentialInStream.h"

SequentialInStream::SequentialInStream(SequentialInStreamReader reader, bool allocatedStatically)
{
    _reader = reader;
    if (!_allocatedStatically)
        AddRef();
}

HRESULT STDMETHODCALLTYPE SequentialInStream::QueryInterface(REFIID riid, void** ppvObject)
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

ULONG STDMETHODCALLTYPE SequentialInStream::AddRef(void)
{
    if (_allocatedStatically)
        return ULONG_MAX;
    return Unknown::AddRef();
}

ULONG STDMETHODCALLTYPE SequentialInStream::Release(void)
{
    if (_allocatedStatically)
        return ULONG_MAX;
    return Unknown::Release();
}

HRESULT STDMETHODCALLTYPE SequentialInStream::Read(void* data, UInt32 size, UInt32* processedSize) throw()
{
    return _reader(data, size, processedSize);
}
