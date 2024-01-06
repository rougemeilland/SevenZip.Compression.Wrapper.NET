#include "CompressCodecsInfo.h"

CompressCodecsInfo::CompressCodecsInfo()
{
    _entryPoint = nullptr;
}

CompressCodecsInfo::~CompressCodecsInfo()
{
    if (_entryPoint != nullptr)
    {
        delete _entryPoint;
        _entryPoint = nullptr;
    }
}

HRESULT STDMETHODCALLTYPE CompressCodecsInfo::QueryInterface(REFIID riid, void** ppvObject)
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

ULONG STDMETHODCALLTYPE CompressCodecsInfo::AddRef(void)
{
    return Unknown::AddRef();
}

ULONG STDMETHODCALLTYPE CompressCodecsInfo::Release(void)
{
    return Unknown::Release();
}

HRESULT CompressCodecsInfo::Create(Byte* locationPath, CompressCodecsInfo** obj)
{
    *obj = nullptr;
    SevenZipEntryPoint* entryPoint;
    HRESULT result = SevenZipEntryPoint::Create(locationPath, &entryPoint);
    if (result != S_OK)
        return result;
    *obj = new CompressCodecsInfo();
    (*obj)->_entryPoint = entryPoint;
    return S_OK;
}

HRESULT STDMETHODCALLTYPE CompressCodecsInfo::GetNumMethods(UInt32* numMethods) throw()
{
    return _entryPoint->GetNumberOfMethods(numMethods);
}

HRESULT STDMETHODCALLTYPE CompressCodecsInfo::GetProperty(UInt32 index, PROPID propID, PROPVARIANT* value) throw()
{
    return _entryPoint->GetMethodProperty(index, propID, value);
}

HRESULT STDMETHODCALLTYPE CompressCodecsInfo::CreateDecoder(UInt32 index, const GUID* iid, void** coder) throw()
{
    return _entryPoint->CreateDecoder(index, iid, coder);
}

HRESULT STDMETHODCALLTYPE CompressCodecsInfo::CreateEncoder(UInt32 index, const GUID* iid, void** coder) throw()
{
    return _entryPoint->CreateEncoder(index, iid, coder);
}
