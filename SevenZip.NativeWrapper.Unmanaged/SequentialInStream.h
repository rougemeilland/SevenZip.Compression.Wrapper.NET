#pragma once

#include "Unknown.h"

struct SequentialInStream
    : public Unknown, public ISequentialInStream
{
private:
    SequentialInStreamReader _reader;
public:
    SequentialInStream(SequentialInStreamReader reader);
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
    virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
    virtual ULONG STDMETHODCALLTYPE Release(void) override;
    virtual HRESULT STDMETHODCALLTYPE Read(void* data, UInt32 size, UInt32* processedSize) throw() override;
};

