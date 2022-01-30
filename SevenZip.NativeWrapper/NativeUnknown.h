#pragma once

#include "Platform.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        struct NativeUnknown
            : public IUnknown
        {
        private:
            const GUID* _supprtedInterfaceIds;
            INT16 _supprtedInterfaceCount;
            ULONG _referenceCount;
            NativeUnknown(const NativeUnknown& p);
        protected:
            NativeUnknown(const GUID* supprtedInterfaceIds, Int16 supprtedInterfaceCount);
        public:
            virtual ~NativeUnknown();
            virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
            virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
            virtual ULONG STDMETHODCALLTYPE Release(void) override;
        };
    }
}
