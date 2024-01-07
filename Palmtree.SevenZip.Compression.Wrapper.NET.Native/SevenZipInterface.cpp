#include "CompressCodecsInfo.h"
#include "CompressProgressInfo.h"
#include "SequentialInStream.h"
#include "SequentialOutStream.h"
#include "SevenZipInterface.h"

extern "C"
{
    __DEFINE_PUBLIC_FUNC(Int32, Global, GetSizeOfPROPVARIANT)()
    {
        return sizeof(PROPVARIANT);
    }

    __DEFINE_PUBLIC_FUNC(Int32, Global, GetSizeOfOleChar)()
    {
        PROPVARIANT value = {};
        return sizeof(*value.bstrVal);
    }

    __DEFINE_PUBLIC_FUNC(HRESULT, ICompressCodecsInfo, Create)(SevenZipEntryPoint::EntryPointsTable* entryPontsTable, ICompressCodecsInfo** obj)
    {
#ifdef _DEBUG
        // 型のビット長の自己診断に失敗したらエラーを返す。
        if (ValidateTypes() != nullptr)
            return E_UNEXPECTED;
#endif // _DEBUG

        CompressCodecsInfo* createdObject = nullptr;
        HRESULT result = CompressCodecsInfo::Create(entryPontsTable, &createdObject);
        if (result != S_OK)
        {
            *obj = nullptr;
            return result;
        }
        *obj = createdObject;
        return S_OK;
    }
}

HRESULT STDMETHODCALLTYPE Customized_IUnknown__QueryInterface(IUnknown* ifp, GUID* piid, void** ppvObject)
{
    HRESULT result = ifp->QueryInterface(*piid, ppvObject);
    if (result != S_OK)
        *ppvObject = nullptr;
    return result;
}

HRESULT STDMETHODCALLTYPE Customized_ICompressCoder__Code(ICompressCoder* ifp, SequentialInStreamReader inStreamReader, SequentialOutStreamWriter outStreamWriter, const UInt64* inSize, const UInt64* outSize, CompressProgressInfoReporter progressReporter)
{
    SequentialInStream inStream(inStreamReader, true);
    SequentialOutStream outStream(outStreamWriter, true);
    if (progressReporter == nullptr)
    {
        return ifp->Code(&inStream, &outStream, inSize, outSize, nullptr);
    }
    else
    {
        CompressProgressInfo progress(progressReporter, true);
        return ifp->Code(&inStream, &outStream, inSize, outSize, &progress);
    }
}

HRESULT STDMETHODCALLTYPE Customized_ICompressCoder2__Code(ICompressCoder2* ifp, SequentialInStreamReader const* inStreamReaders, const UInt64* const* inSizes, UInt32 numInStreams, SequentialOutStreamWriter const* outStreamWriters, const UInt64* const* outSizes, UInt32 numOutStreams, CompressProgressInfoReporter progressReporter)
{
    ISequentialInStream** inStreams = new ISequentialInStream * [numInStreams];
    for (UInt32 index = 0; index < numInStreams; ++index)
        inStreams[index] = new SequentialInStream(inStreamReaders[index], false);
    ISequentialOutStream** outStreams = new ISequentialOutStream * [numOutStreams];
    for (UInt32 index = 0; index < numOutStreams; ++index)
        outStreams[index] = new SequentialOutStream(outStreamWriters[index], false);
    HRESULT result;
    if (progressReporter == nullptr)
    {
        result = ifp->Code(inStreams, inSizes, numInStreams, outStreams, outSizes, numOutStreams, nullptr);
    }
    else
    {
        CompressProgressInfo progress(progressReporter, true);
        result = ifp->Code(inStreams, inSizes, numInStreams, outStreams, outSizes, numOutStreams, &progress);
    }
    for (UInt32 index = 0; index < numOutStreams; ++index)
        outStreams[index]->Release();
    delete[] outStreams;
    for (UInt32 index = 0; index < numInStreams; ++index)
        inStreams[index]->Release();
    delete[] inStreams;
    return result;
}

HRESULT STDMETHODCALLTYPE Customized_ICompressWriteCoderProperties__WriteCoderProperties(ICompressWriteCoderProperties* ifp, SequentialOutStreamWriter outStreamWriter)
{
    SequentialOutStream outStream(outStreamWriter, true);
    return ifp->WriteCoderProperties(&outStream);
}

HRESULT STDMETHODCALLTYPE Customized_ICompressSetInStream__SetInStream(ICompressSetInStream* ifp, SequentialInStreamReader inStreamReader)
{
    SequentialInStream* inStream = new SequentialInStream(inStreamReader, false);
    HRESULT result = ifp->SetInStream(inStream);
    inStream->Release();
    return result;
}
