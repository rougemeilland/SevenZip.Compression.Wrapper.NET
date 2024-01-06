#pragma once

#include "Unknown.h"

class SequentialOutStream
    : public Unknown, public ISequentialOutStream
{
private:
    SequentialOutStreamWriter _writer;
    bool _allocatedStatically;
public:
    SequentialOutStream(SequentialOutStreamWriter writer, bool allocatedStatically);
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID riid, void** ppvObject) override;
    virtual ULONG STDMETHODCALLTYPE AddRef(void) override;
    virtual ULONG STDMETHODCALLTYPE Release(void) override;
    virtual HRESULT STDMETHODCALLTYPE Write(const void* data, UInt32 size, UInt32* processedSize) throw() override;
};

