#pragma once

#define ENABLE_MSVC_SPECIFIC_SPECIFICATIONS

#include "Platform.h"
#include "InterfaceDefinitions.h"

#ifdef ENABLE_MSVC_SPECIFIC_SPECIFICATIONS

#define __DEFINE_INTERFACE(interfaceName, interfaceIdString) \
        struct  \
        __declspec(uuid(interfaceIdString)) \
        interfaceName abstract
#define __UUIDOF(interfaceName) __uuidof(interfaceName)

#else // ENABLE_MSVC_SPECIFIC_SPECIFICATIONS

#define __DEFINE_INTERFACE(interfaceName, interfaceIdString) \
        extern "C" const GUID IID_ ## interfaceName; \
        struct interfaceName abstract
#define __UUIDOF(interfaceName) SevenZip::Compression::Implementation::IID_ ## interfaceName

#endif // ENABLE_MSVC_SPECIFIC_SPECIFICATIONS

#if defined(_PLATFORM_WINDOWS_X86)
#define __SEVEN_ZIP_NATIVE_MODULE_FILE_NAME "7z.dll"
#elif  defined(_PLATFORM_WINDOWS_X64)
#define __SEVEN_ZIP_NATIVE_MODULE_FILE_NAME "7z.dll"
#else
#error "not supported platform"
#endif


namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace SevenZipInterface
        {
            struct IHasher;

            typedef UInt32(STDMETHODCALLTYPE* Func_IsArc)(const Byte* p, size_t size);
            typedef UInt64 CMethodId;
            typedef ULONG PROPID;

            enum CoderPropID
            {
                kDefaultProp = 0,
                kDictionarySize,
                kUsedMemorySize,
                kOrder,
                kBlockSize,
                kPosStateBits,
                kLitContextBits,
                kLitPosBits,
                kNumFastBytes,
                kMatchFinder,
                kMatchFinderCycles,
                kNumPasses,
                kAlgorithm,
                kNumThreads,
                kEndMarker,
                kLevel,
                kReduceSize,
                kExpectedDataSize,
                kBlockSize2,
                kCheckSize,
                kFilter,
                kMemUse,
                kAffinity
            };

            enum MethodPropID
            {
                kID,
                kName,
                kDecoder,
                kEncoder,
                kPackStreams,
                kUnpackStreams,
                kDescription,
                kDecoderIsAssigned,
                kEncoderIsAssigned,
                kDigestSize
            };

            struct CHasherInfo
            {
                IHasher* (*CreateHasher)();
                CMethodId Id;
                const char* Name;
                UInt32 DigestSize;
            };

            struct CStreamFileProps
            {
                UInt64 Size;
                UInt64 VolID;
                UInt64 FileID_Low;
                UInt64 FileID_High;
                UInt32 NumLinks;
                UInt32 Attrib;
                FILETIME CTime;
                FILETIME ATime;
                FILETIME MTime;
            };

            __DEFINE_INTERFACE(ISequentialInStream, IIDS_ISequentialInStream)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE Read(void* data, UInt32 size, UInt32 * processedSize) abstract;
            };

            __DEFINE_INTERFACE(ISequentialOutStream, IIDS_ISequentialOutStream)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE Write(const void* data, UInt32 size, UInt32 * processedSize) abstract;
            };

            __DEFINE_INTERFACE(IInStream, IIDS_IInStream)
                : public ISequentialInStream
            {
                virtual HRESULT STDMETHODCALLTYPE Seek(Int64 offset, UInt32 seekOrigin, UInt64 * newPosition) abstract;
            };

            __DEFINE_INTERFACE(IOutStream, IIDS_IOutStream)
                : public ISequentialOutStream
            {
                virtual HRESULT STDMETHODCALLTYPE Seek(Int64 offset, UInt32 seekOrigin, UInt64 * newPosition) abstract;
                virtual HRESULT STDMETHODCALLTYPE SetSize(UInt64 newSize) abstract;
            };

            __DEFINE_INTERFACE(IStreamGetSize, IIDS_IStreamGetSize)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE GetSize(UInt64 * size) abstract;
            };

            __DEFINE_INTERFACE(IOutStreamFinish, IIDS_IOutStreamFinish)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE OutStreamFinish() abstract;
            };

            __DEFINE_INTERFACE(IStreamGetProps, IIDS_IStreamGetProps)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE GetProps(UInt64 * size, FILETIME * cTime, FILETIME * aTime, FILETIME * mTime, UInt32 * attrib) abstract;
            };

            __DEFINE_INTERFACE(IStreamGetProps2, IIDS_IStreamGetProps2)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE GetProps2(CStreamFileProps * props) abstract;
            };

            __DEFINE_INTERFACE(ICompressProgressInfo, IIDS_ICompressProgressInfo)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetRatioInfo(const UInt64 * inSize, const UInt64 * outSize) abstract;
            };

            __DEFINE_INTERFACE(ICompressCoder, IIDS_ICompressCoder)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE Code(ISequentialInStream * inStream, ISequentialOutStream * outStream, const UInt64 * inSize, const UInt64 * outSize, ICompressProgressInfo * progress) abstract;
            };

            __DEFINE_INTERFACE(ICompressCoder2, IIDS_ICompressCoder2)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE Code(ISequentialInStream* const* inStreams, const UInt64* const* inSizes, UInt32 numInStreams, ISequentialOutStream* const* outStreams, const UInt64* const* outSizes, UInt32 numOutStreams, ICompressProgressInfo * progress) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetCoderPropertiesOpt, IIDS_ICompressSetCoderPropertiesOpt)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetCoderPropertiesOpt(const PROPID * propIDs, const PROPVARIANT * props, UInt32 numProps) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetCoderProperties, IIDS_ICompressSetCoderProperties)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetCoderProperties(const PROPID * propIDs, const PROPVARIANT * props, UInt32 numProps) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetDecoderProperties2, IIDS_ICompressSetDecoderProperties2)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetDecoderProperties2(const Byte * data, UInt32 size) abstract;
            };

            __DEFINE_INTERFACE(ICompressWriteCoderProperties, IIDS_ICompressWriteCoderProperties)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE WriteCoderProperties(ISequentialOutStream * outStream) abstract;
            };

            __DEFINE_INTERFACE(ICompressGetInStreamProcessedSize, IIDS_ICompressGetInStreamProcessedSize)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE GetInStreamProcessedSize(UInt64 * value) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetCoderMt, IIDS_ICompressSetCoderMt)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetNumberOfThreads(UInt32 numThreads) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetFinishMode, IIDS_ICompressSetFinishMode)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetFinishMode(UInt32 finishMode) abstract;
            };

            __DEFINE_INTERFACE(ICompressGetInStreamProcessedSize2, IIDS_ICompressGetInStreamProcessedSize2)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE GetInStreamProcessedSize2(UInt32 streamIndex, UInt64 * value) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetMemLimit, IIDS_ICompressSetMemLimit)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetMemLimit(UInt64 memUsage) abstract;
            };

            __DEFINE_INTERFACE(ICompressReadUnusedFromInBuf, IIDS_ICompressReadUnusedFromInBuf)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE ReadUnusedFromInBuf(void* data, UInt32 size, UInt32 * processedSize) abstract;
            };

            __DEFINE_INTERFACE(ICompressGetSubStreamSize, IIDS_ICompressGetSubStreamSize)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE GetSubStreamSize(UInt64 subStream, UInt64 * value) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetInStream, IIDS_ICompressSetInStream)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetInStream(ISequentialInStream * inStream) abstract;
                virtual HRESULT STDMETHODCALLTYPE ReleaseInStream() abstract;
            };

            __DEFINE_INTERFACE(ICompressSetOutStream, IIDS_ICompressSetOutStream)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetOutStream(ISequentialOutStream * outStream) abstract;
                virtual HRESULT STDMETHODCALLTYPE ReleaseOutStream() abstract;
            };

            __DEFINE_INTERFACE(ICompressSetOutStreamSize, IIDS_ICompressSetOutStreamSize)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetOutStreamSize(const UInt64 * outSize) abstract;
            };

            __DEFINE_INTERFACE(ICompressSetBufSize, IIDS_ICompressSetBufSize)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetInBufSize(UInt32 streamIndex, UInt32 size) abstract;
                virtual HRESULT STDMETHODCALLTYPE SetOutBufSize(UInt32 streamIndex, UInt32 size) abstract;
            };

            __DEFINE_INTERFACE(ICompressInitEncoder, IIDS_ICompressInitEncoder)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE InitEncoder() abstract;
            };

            __DEFINE_INTERFACE(ICompressSetInStream2, IIDS_ICompressSetInStream2)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetInStream2(UInt32 streamIndex, ISequentialInStream * inStream) abstract;
                virtual HRESULT STDMETHODCALLTYPE ReleaseInStream2(UInt32 streamIndex) abstract;
            };

            __DEFINE_INTERFACE(ICompressFilter, IIDS_ICompressFilter)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE Init() abstract;
                virtual UInt32 STDMETHODCALLTYPE Filter(Byte* data, UInt32 size) abstract;
            };

            __DEFINE_INTERFACE(ICompressCodecsInfo, IIDS_ICompressCodecsInfo)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE GetNumMethods(UInt32 * numMethods) abstract;
                virtual HRESULT STDMETHODCALLTYPE GetProperty(UInt32 index, PROPID propID, PROPVARIANT* value) abstract;
                virtual HRESULT STDMETHODCALLTYPE CreateDecoder(UInt32 index, const GUID* iid, void** coder) abstract;
                virtual HRESULT STDMETHODCALLTYPE CreateEncoder(UInt32 index, const GUID* iid, void** coder) abstract;
            };

            __DEFINE_INTERFACE(ISetCompressCodecsInfo, IIDS_ISetCompressCodecsInfo)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetCompressCodecsInfo(ICompressCodecsInfo * compressCodecsInfo) abstract;
            };

            __DEFINE_INTERFACE(ICryptoProperties, IIDS_ICryptoProperties)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE SetKey(const Byte * data, UInt32 size) abstract;
                virtual HRESULT STDMETHODCALLTYPE SetInitVector(const Byte* data, UInt32 size) abstract;
            };

            __DEFINE_INTERFACE(ICryptoResetInitVector, IIDS_ICryptoResetInitVector)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE ResetInitVector() abstract;
            };

            __DEFINE_INTERFACE(ICryptoSetPassword, IIDS_ICryptoSetPassword)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE CryptoSetPassword(const Byte * data, UInt32 size) abstract;
            };

            __DEFINE_INTERFACE(ICryptoSetCRC, IIDS_ICryptoSetCRC)
                : public IUnknown
            {
                virtual HRESULT STDMETHODCALLTYPE CryptoSetCRC(UInt32 crc) abstract;
            };

            __DEFINE_INTERFACE(IHasher, IIDS_IHasher)
                : public IUnknown
            {
                virtual void STDMETHODCALLTYPE Init() throw() abstract;
                virtual void STDMETHODCALLTYPE Update(const void* data, UInt32 size) throw() abstract;
                virtual void STDMETHODCALLTYPE Final(Byte* digest) throw() abstract;
                virtual UInt32 STDMETHODCALLTYPE GetDigestSize() throw() abstract;
            };

            __DEFINE_INTERFACE(IHashers, IIDS_IHashers)
                : public IUnknown
            {
                virtual UInt32 STDMETHODCALLTYPE GetNumHashers() abstract;
                virtual HRESULT STDMETHODCALLTYPE GetHasherProp(UInt32 index, PROPID propID, PROPVARIANT* value) abstract;
                virtual HRESULT STDMETHODCALLTYPE CreateHasher(UInt32 index, IHasher** hasher) abstract;
            };
        }
    }
}
