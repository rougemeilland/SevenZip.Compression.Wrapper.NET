#include "CompressCodecsInfo.h"
#include "CompressProgressInfo.h"
#include "SequentialInStream.h"
#include "SequentialOutStream.h"
#include "SevenZipInterface.h"

extern "C"
{
    __DEFINE_PUBLIC_FUNC(HRESULT, ICompressCodecsInfo, Create)(const wchar_t* locationPath, ICompressCodecsInfo** obj)
    {
        CompressCodecsInfo* createdObject = nullptr;
        HRESULT result = CompressCodecsInfo::Create(locationPath, &createdObject);
        if (result != S_OK)
        {
            *obj = nullptr;
            return result;
        }
        *obj = createdObject;
        return S_OK;
    }
}

HRESULT Customized_IUnknown__QueryInterface(IUnknown* ifp, GUID* piid, void** ppvObject)
{
    HRESULT result = ifp->QueryInterface(*piid, ppvObject);
    if (result != S_OK)
        *ppvObject = nullptr;
    return result;
}

HRESULT Customized_ICompressCoder__Code(ICompressCoder* ifp, SequentialInStreamReader inStreamReader, SequentialOutStreamWriter outStreamWriter, const UInt64* inSize, const UInt64* outSize, CompressProgressInfoReporter progressReporter)
{
    SequentialInStream inStream(inStreamReader);
    SequentialOutStream outStream(outStreamWriter);
    if (progressReporter == nullptr)
        return ifp->Code(&inStream, &outStream, inSize, outSize, nullptr);
    else
    {
        CompressProgressInfo progress(progressReporter);
        return ifp->Code(&inStream, &outStream, inSize, outSize, &progress);
    }
}

HRESULT Customized_ICompressCoder2__Code(ICompressCoder2* ifp, SequentialInStreamReader const* inStreamReaders, const UInt64* const* inSizes, UInt32 numInStreams, SequentialOutStreamWriter const* outStreamWriters, const UInt64* const* outSizes, UInt32 numOutStreams, CompressProgressInfoReporter progressReporter)
{
    ISequentialInStream** inStreams = new ISequentialInStream * [numInStreams];
    for (UInt32 index = 0; index < numInStreams; ++index)
        inStreams[index] = new SequentialInStream(inStreamReaders[index]);
    ISequentialOutStream** outStreams = new ISequentialOutStream * [numOutStreams];
    for (UInt32 index = 0; index < numOutStreams; ++index)
        outStreams[index] = new SequentialOutStream(outStreamWriters[index]);
    HRESULT result;
    if (progressReporter == nullptr)
        result = ifp->Code(inStreams, inSizes, numInStreams, outStreams, outSizes, numOutStreams, nullptr);
    else
    {
        CompressProgressInfo progress(progressReporter);
        result = ifp->Code(inStreams, inSizes, numInStreams, outStreams, outSizes, numOutStreams, &progress);
    }
    for (UInt32 index = 0; index < numOutStreams; ++index)
        delete outStreams[index];
    delete[] outStreams;
    for (UInt32 index = 0; index < numInStreams; ++index)
        delete inStreams[index];
    delete[] inStreams;
    return result;
}

HRESULT Customized_ICompressWriteCoderProperties__WriteCoderProperties(ICompressWriteCoderProperties* ifp, SequentialOutStreamWriter outStreamWriter)
{
    SequentialOutStream outStream(outStreamWriter);
    return ifp->WriteCoderProperties(&outStream);
}

HRESULT Customized_ICompressSetInStream__SetInStream(ICompressSetInStream* ifp, SequentialInStreamReader inStreamReader)
{
    SequentialInStream inStream(inStreamReader);
    return ifp->SetInStream(&inStream);
}
