#include "NativeUnknown.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        NativeUnknown::NativeUnknown(const GUID* supprtedInterfaceIds, Int16 supprtedInterfaceCount)
        {
            _supprtedInterfaceIds = supprtedInterfaceIds;
            _supprtedInterfaceCount = supprtedInterfaceCount;
            _referenceCount = 0;
        }

        NativeUnknown::~NativeUnknown()
        {
        }

        HRESULT STDMETHODCALLTYPE NativeUnknown::QueryInterface(REFIID riid, void** ppvObject)
        {
            if (!ppvObject)
                return E_INVALIDARG;
            *ppvObject = nullptr;
            for (Int32 index = 0; index < _supprtedInterfaceCount; ++index)
            {
                if (riid == _supprtedInterfaceIds[index])
                {
                    *ppvObject = this;
                    AddRef();
                    return NOERROR;
                }
            }
            if (riid == IID_IUnknown)
            {
                *ppvObject = this;
                AddRef();
                return NOERROR;
            }
            return E_NOINTERFACE;
        }
        ULONG STDMETHODCALLTYPE NativeUnknown::AddRef(void)
        {
            InterlockedIncrement(&_referenceCount);
            return _referenceCount;
        }

        ULONG STDMETHODCALLTYPE NativeUnknown::Release(void)
        {
            ULONG count = InterlockedDecrement(&_referenceCount);
            if (count == 0)
                delete this;
            return count;
        }
    }
}
