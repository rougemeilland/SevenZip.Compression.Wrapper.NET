#include "SequentialInStream.h"

SequentialInStream::SequentialInStream(SequentialInStreamReader reader)
{
    _reader = reader;
}

HRESULT STDMETHODCALLTYPE SequentialInStream::QueryInterface(REFIID riid, void** ppvObject)
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

ULONG STDMETHODCALLTYPE SequentialInStream::AddRef(void)
{
    return Unknown::AddRef();
}

ULONG STDMETHODCALLTYPE SequentialInStream::Release(void)
{
    return Unknown::Release();
}

HRESULT STDMETHODCALLTYPE SequentialInStream::Read(void* data, UInt32 size, UInt32* processedSize) throw()
{
    return _reader(data, size, processedSize);
}
