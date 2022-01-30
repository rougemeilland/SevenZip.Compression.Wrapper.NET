#include <stdlib.h>
#include "ClrInterOperation.h"
#include "ManagedCompressCodecsInfo.h"
#include "ManagedCompressCodecInfo.h"
#include "ManagedCompressCoder.h"
#include "NativeEntryPoint.h"
#include "Platform.h"
#include "SevenZipInterface.h"
#include "WideString.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            static UInt32 GetMethodsCount(NativeEntryPoint* entryPoint)
            {
                UInt32 methodsCount;
                HRESULT result = entryPoint->GetNumberOfMethods(&methodsCount);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                return methodsCount;
            }

            static UInt64 GetCodecId(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kID, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt != VT_UI8)
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kID\" property is not \"VT_U8\".");
                return propertyValueBuffer->uhVal.QuadPart;
            }

            static System::String^ GetCodecName(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kName, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt != VT_BSTR)
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kName\" property is not \"VT_BSTR\".");
                return gcnew System::String(propertyValueBuffer->bstrVal);
            }

            static bool IsDecoderAssigned(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kDecoderIsAssigned, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt != VT_BOOL)
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kDecoderIsAssigned\" property is not \"VT_BOOL\".");
                return propertyValueBuffer->boolVal != VARIANT_FALSE;
            }

            static bool IsEncoderAssigned(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kEncoderIsAssigned, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt != VT_BOOL)
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kEncoderIsAssigned\" property is not \"VT_BOOL\".");
                return propertyValueBuffer->boolVal != VARIANT_FALSE;
            }

            static System::Guid GetDecodecClsssId(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kDecoder, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt != VT_BSTR)
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kDecoder\" property is not \"VT_BSTR\".");
                // vt is VT_BSTR, but bstrVal points to a GUID structure.
                return FromNativeGuidToManagedGuid(*(const GUID*)propertyValueBuffer->bstrVal);
            }

            static System::Guid GetEncoderClsssId(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kEncoder, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt != VT_BSTR)
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kEncoder\" property is not \"VT_BSTR\".");
                // vt is VT_BSTR, but bstrVal points to a GUID structure.
                return FromNativeGuidToManagedGuid(*(const GUID*)propertyValueBuffer->bstrVal);
            }

            static UInt32 GetPackStreamsCount(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kPackStreams, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt == VT_UI4)
                    return propertyValueBuffer->ulVal;
                else if (propertyValueBuffer->vt == VT_EMPTY)
                    return 1;
                else
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kPackStreams\" property is neither \"VT_U8\" nor \"VT_EMPTY\".");
            }

            static UInt32 GetUnpackStreamsCount(NativeEntryPoint* entryPoint, Int32 index, PROPVARIANT* propertyValueBuffer)
            {
                HRESULT result = entryPoint->GetMethodProperty(index, SevenZipInterface::MethodPropID::kUnpackStreams, propertyValueBuffer);
                if (result != S_OK)
                    __ThrowExceptionForHR(result);
                if (propertyValueBuffer->vt == VT_UI4)
                    return propertyValueBuffer->ulVal;
                else if (propertyValueBuffer->vt == VT_EMPTY)
                    return 1;
                else
                    throw gcnew System::ArgumentException(L"The value type of the coder's \"kUnpackStreams\" property is neither \"VT_U8\" nor \"VT_EMPTY\".");
            }

            static bool IsSupportedICompressCoderByDecoder(NativeEntryPoint* entryPoint, Int32 index)
            {
                SevenZipInterface::ICompressCoder* coder = nullptr;
                try
                {
                    HRESULT result = entryPoint->CreateDecoder(index, &__UUIDOF(SevenZipInterface::ICompressCoder), (void**)&coder);
                    if (result != S_OK)
                    {
                        coder = nullptr;
                        if (result != E_NOINTERFACE)
                            __ThrowExceptionForHR(result);
                        return false;
                    }
                    return true;
                }
                finally
                {
                    if (coder != nullptr)
                        coder->Release();

                }
            }

            static bool IsSupportedICompressCoderByEncoder(NativeEntryPoint* entryPoint, Int32 index)
            {
                SevenZipInterface::ICompressCoder* coder = nullptr;
                try
                {
                    HRESULT result = entryPoint->CreateEncoder(index, &__UUIDOF(SevenZipInterface::ICompressCoder), (void**)&coder);
                    if (result != S_OK)
                    {
                        coder = nullptr;
                        if (result != E_NOINTERFACE)
                            __ThrowExceptionForHR(result);
                        return false;
                    }
                    return true;
                }
                finally
                {
                    if (coder != nullptr)
                        coder->Release();

                }
            }

            ManagedCompressCodecsInfo::ManagedCompressCodecsInfo()
            {
#ifdef _DEBUG
                ValidateTypes();
                ValidateInterfaceIds();
#endif
                _entryPoint = nullptr;
            }

            ManagedCompressCodecsInfo::ManagedCompressCodecsInfo(const ManagedCompressCodecsInfo% p)
            {
                _entryPoint = p._entryPoint;
                if (_entryPoint != nullptr)
                    _entryPoint->AddRef();
            }

            ManagedCompressCodecsInfo::~ManagedCompressCodecsInfo()
            {
                this->!ManagedCompressCodecsInfo();
            }

            ManagedCompressCodecsInfo::!ManagedCompressCodecsInfo()
            {
                if (_entryPoint != nullptr)
                {
                    _entryPoint->Release();
                    _entryPoint = nullptr;
                }
            }

            void ManagedCompressCodecsInfo::Initialize()
            {
                _entryPoint = NativeEntryPoint::Create();
                _entryPoint->AddRef();
            }

            SevenZip::NativeInterface::IUnknown^ ManagedCompressCodecsInfo::QueryInterface(System::Type^ interfaceType)
            {
                if (interfaceType->GUID.Equals(NativeInterface::IUnknown::GetType()->GUID))
                    return this;
                else if (interfaceType->GUID.Equals(NativeInterface::Compression::ICompressCodecsInfo::GetType()->GUID))
                    return this;
                else
                    return nullptr;
            }

            void ManagedCompressCodecsInfo::EnumerateCodecs(SevenZip::NativeInterface::Compression::CompressCodecInfoGetter^ compressCodecInfoGetter)
            {
                Int32 methodCount = (Int32)GetMethodsCount(_entryPoint);
                if (methodCount < 0)
                    throw gcnew System::Exception(L"Too large methodCount");

                /*
                Note on how to allocate the "PROPVARIANTT" structure:

                [Summary]
                A memory leak can occur when getting a property value containing a "VT_BSTR" type with the 7zip "GetMethodProperty" method.
                In order to avoid the problem, the following should be noted.
                - The "PROPVARIANT" structure must be initialized in advance.
                - The "PROPVARIANT" structure should be shared as much as possible.
                - The last call to the "GetMethodProperty" method should return a type other than "VT_BSTR".

                [Details]
                In 7zip's "GetMethodProperty" method, when a value of type "VT_BSTR" is returned,
                the "PROPVARIANT" structure contains a pointer to the memory allocated by the "malloc" function.
                There is no way to free this memory from the caller.
                However, in 7zip, when the "GetMethodProperty" method is called, if VT_BSTR type data is already set in the "PROPVARIANT" structure,
                the memory of the pointer stored there is released by the "free" function.
                Later, the new property value is stored in the "PROPVARIANT" structure.
                Therefore, the "PROPVARIANT" structure should be shared as much as possible,
                and the final "GetMethodProperty" method call should be a call that returns a value of a type other than "VT_BSTR".
                */
                PROPVARIANT propertyValueBuffer;
                memset(&propertyValueBuffer, 0, sizeof(propertyValueBuffer));
                for (Int32 codecIndex = 0; codecIndex < methodCount; ++codecIndex)
                {
                    System::String^ currentCodecName = GetCodecName(_entryPoint, codecIndex, &propertyValueBuffer);
                    bool isCurrentDecoderIsAssigned = IsDecoderAssigned(_entryPoint, codecIndex, &propertyValueBuffer);
                    bool isCurrentEncoderIsAssigned = IsEncoderAssigned(_entryPoint, codecIndex, &propertyValueBuffer);
                    if (isCurrentDecoderIsAssigned)
                    {
                        System::Guid currentDecoderClassId = GetDecodecClsssId(_entryPoint, codecIndex, &propertyValueBuffer);
                        UInt64 currentCodecId = GetCodecId(_entryPoint, codecIndex, &propertyValueBuffer);
                        bool isCurrentDecoderSupportICompressCoder = IsSupportedICompressCoderByDecoder(_entryPoint, codecIndex);
                        compressCodecInfoGetter->Invoke(
                            gcnew ManagedCompressCodecInfo(
                                _entryPoint,
                                codecIndex,
                                currentCodecId,
                                currentCodecName,
                                currentDecoderClassId,
                                NativeInterface::CoderType::Decoder,
                                isCurrentDecoderSupportICompressCoder));
                    }
                    if (isCurrentEncoderIsAssigned)
                    {
                        System::Guid currentEncoderClassId = GetEncoderClsssId(_entryPoint, codecIndex, &propertyValueBuffer);
                        UInt64 currentCodecId = GetCodecId(_entryPoint, codecIndex, &propertyValueBuffer);
                        bool isCurrentEncoderSupportICompressCoder = IsSupportedICompressCoderByEncoder(_entryPoint, codecIndex);
                        compressCodecInfoGetter->Invoke(
                            gcnew ManagedCompressCodecInfo(
                                _entryPoint,
                                codecIndex,
                                currentCodecId,
                                currentCodecName,
                                currentEncoderClassId,
                                NativeInterface::CoderType::Encoder,
                                isCurrentEncoderSupportICompressCoder));
                    }
                }
            }
        }
    }
}
