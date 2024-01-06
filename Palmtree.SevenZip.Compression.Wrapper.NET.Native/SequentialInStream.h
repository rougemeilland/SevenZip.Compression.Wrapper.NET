#pragma once

#include "Unknown.h"

struct SequentialInStream
    : public Unknown, public ISequentialInStream
{
private:
    SequentialInStreamReader _reader;
    bool _allocatedStatically;
public:
    SequentialInStream(SequentialInStreamReader reader, bool allocatedStatically);
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
    virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
    virtual ULONG STDMETHODCALLTYPE Release(void) override;
    virtual HRESULT STDMETHODCALLTYPE Read(void* data, UInt32 size, UInt32* processedSize) throw() override;
};
