#pragma once

#include "ManagedUnknown.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            template <typename NATIVE_INTERFACE_T, const GUID* nativeInterfaceId>
            ref class ManagedCompressSetCoderPropertiesBase abstract
                : public NativeWrapper::ManagedUnknown<NATIVE_INTERFACE_T, nativeInterfaceId>
            {
            protected:
                void SetCommonCoderProperties(SevenZip::NativeInterface::ICoderPropertyGetter^ propertiesGetter, SevenZip::NativeInterface::CoderPropertyId properIdMask)
                {
                    SevenZip::NativeInterface::CoderPropertyId propertyIds = propertiesGetter->PropertyIds & properIdMask;
                    Int32 maximumPropertiesCount = GetPropertiesCount(propertyIds);
                    PROPID* nativePropertyIds = new PROPID[maximumPropertiesCount];
                    PROPVARIANT* nativePropertyValues = new PROPVARIANT[maximumPropertiesCount];
                    try
                    {
                        Int32 propertiesCount;
                        ConvertProperties(propertiesGetter, propertyIds, nativePropertyIds, nativePropertyValues, &propertiesCount);
#ifdef _DEBUG
                        {
                            System::Diagnostics::Debug::WriteLine(L"**********");
                            System::Diagnostics::Debug::WriteLine(L"SetCommonCoderProperties:");
                            for (Int32 __index = 0; __index < propertiesCount; ++__index)
                            {
                                PROPID propertyId = (CoderPropID)nativePropertyIds[__index];
                                PROPVARIANT* propertyValue = &nativePropertyValues[__index];
                                const wchar_t* propertyName;
                                switch (propertyId)
                                {
                                case CoderPropID::kDefaultProp: propertyName = L"kDefaultProp"; break;
                                case CoderPropID::kDictionarySize: propertyName = L"kDictionarySize"; break;
                                case CoderPropID::kUsedMemorySize: propertyName = L"kUsedMemorySize"; break;
                                case CoderPropID::kOrder: propertyName = L"kOrder"; break;
                                case CoderPropID::kBlockSize: propertyName = L"kBlockSize"; break;
                                case CoderPropID::kPosStateBits: propertyName = L"kPosStateBits"; break;
                                case CoderPropID::kLitContextBits: propertyName = L"kLitContextBits"; break;
                                case CoderPropID::kLitPosBits: propertyName = L"kLitPosBits"; break;
                                case CoderPropID::kNumFastBytes: propertyName = L"kNumFastBytes"; break;
                                case CoderPropID::kMatchFinder: propertyName = L"kMatchFinder"; break;
                                case CoderPropID::kMatchFinderCycles: propertyName = L"kMatchFinderCycles"; break;
                                case CoderPropID::kNumPasses: propertyName = L"kNumPasses"; break;
                                case CoderPropID::kAlgorithm: propertyName = L"kAlgorithm"; break;
                                case CoderPropID::kNumThreads: propertyName = L"kNumThreads"; break;
                                case CoderPropID::kEndMarker: propertyName = L"kEndMarker"; break;
                                case CoderPropID::kLevel: propertyName = L"kLevel"; break;
                                case CoderPropID::kReduceSize: propertyName = L"kReduceSize"; break;
                                case CoderPropID::kExpectedDataSize: propertyName = L"kExpectedDataSize"; break;
                                case CoderPropID::kBlockSize2: propertyName = L"kBlockSize2"; break;
                                case CoderPropID::kCheckSize: propertyName = L"kCheckSize"; break;
                                case CoderPropID::kFilter: propertyName = L"kFilter"; break;
                                case CoderPropID::kMemUse: propertyName = L"kMemUse"; break;
                                case CoderPropID::kAffinity: propertyName = L"kAffinity"; break;
                                default: propertyName = L"<unknown>"; break;
                                }
                                System::String^ propertyNameString = gcnew System::String(propertyName);
                                System::String^ propertyValueTypeString = nullptr;
                                System::String^ propertyValueString = nullptr;
                                switch (propertyValue->vt)
                                {
                                case VT_EMPTY:
                                    propertyValueTypeString = L"VT_EMPTY";
                                    propertyValueString = L"<none>";
                                    break;
                                case VT_BOOL:
                                    propertyValueTypeString = L"VT_BOOL";
                                    propertyValueString = gcnew System::String(propertyValue->boolVal != VARIANT_FALSE ? L"true" : L"false");
                                    break;
                                case VT_UI4:
                                    propertyValueTypeString = L"VT_UI4";
                                    propertyValueString = System::String::Format(L"{0} (0x{0:x8})", gcnew System::UInt32(propertyValue->ulVal));
                                    break;
                                case VT_UI8:
                                    propertyValueTypeString = L"VT_UI8";
                                    propertyValueString = System::String::Format(L"{0} (0x{0:x16})", gcnew System::UInt64(propertyValue->uhVal.QuadPart));
                                    break;
                                case VT_BSTR:
                                    propertyValueTypeString = L"VT_BSTR";
                                    propertyValueString = System::String::Format(L"\"{0}\"", gcnew System::String(propertyValue->bstrVal));
                                    break;
                                default:
                                    propertyValueTypeString = L"<unknown>";
                                    propertyValueString = L"";
                                    break;
                                }
                                System::Diagnostics::Debug::WriteLine(System::String::Format(L"[{0}] name=\"{1}\", type=\"{2}\", value={3}", System::Int32(__index), propertyNameString, propertyValueTypeString, propertyValueString));
                            }
                            System::Diagnostics::Debug::WriteLine(L"**********");
                        }
#endif
                        HRESULT result = SetNativeCoderProperties(nativePropertyIds, nativePropertyValues, propertiesCount);
                        if (result != S_OK)
                            __ThrowExceptionForHR(result);
                    }
                    finally
                    {
                        delete[] nativePropertyIds;
                        delete[] nativePropertyValues;
                    }
                }

                virtual HRESULT SetNativeCoderProperties(const PROPID* propIDs, const PROPVARIANT* props, UInt32 numProps) abstract;

            private:
                static Int32 GetPropertiesCount(SevenZip::NativeInterface::CoderPropertyId propertyIds)
                {
                    Int32 maximumPropertiesCount = 0;
                    UInt64 propertyIdsBitPattern = (UInt64)propertyIds;
                    while (propertyIdsBitPattern != 0)
                    {
                        if (propertyIdsBitPattern & 1)
                            ++maximumPropertiesCount;
                        propertyIdsBitPattern >>= 1;
                    }
                    return maximumPropertiesCount;
                }

                static void ConvertProperties(SevenZip::NativeInterface::ICoderPropertyGetter^ propertiesGetter, SevenZip::NativeInterface::CoderPropertyId propertyIds, PROPID* nativePropertyIds, PROPVARIANT* nativePropertyValues, Int32* propertiesCount)
                {
                    *propertiesCount = 0;
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::Affinity, CoderPropID::kAffinity, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::Algorithm, CoderPropID::kAlgorithm, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::BlockSize, CoderPropID::kBlockSize, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::BlockSize2, CoderPropID::kBlockSize2, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::CheckSize, CoderPropID::kCheckSize, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::DefaultProp, CoderPropID::kDefaultProp, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::DictionarySize, CoderPropID::kDictionarySize, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::EndMarker, CoderPropID::kEndMarker, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::ExpectedDataSize, CoderPropID::kExpectedDataSize, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::Filter, CoderPropID::kFilter, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::Level, CoderPropID::kLevel, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::LitContextBits, CoderPropID::kLitContextBits, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::LitPosBits, CoderPropID::kLitPosBits, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::MatchFinder, CoderPropID::kMatchFinder, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::MatchFinderCycles, CoderPropID::kMatchFinderCycles, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::MemUse, CoderPropID::kMemUse, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::NumFastBytes, CoderPropID::kNumFastBytes, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::NumPasses, CoderPropID::kNumPasses, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::NumThreads, CoderPropID::kNumThreads, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::Order, CoderPropID::kOrder, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::PosStateBits, CoderPropID::kPosStateBits, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::ReduceSize, CoderPropID::kReduceSize, nativePropertyIds, nativePropertyValues, propertiesCount);
                    ConvertProperty(propertiesGetter, propertyIds, SevenZip::NativeInterface::CoderPropertyId::UsedMemorySize, CoderPropID::kUsedMemorySize, nativePropertyIds, nativePropertyValues, propertiesCount);
                }

                static void ConvertProperty(SevenZip::NativeInterface::ICoderPropertyGetter^ propertiesGetter, SevenZip::NativeInterface::CoderPropertyId properIds, SevenZip::NativeInterface::CoderPropertyId propertyId, Int32 nativePropertyId, PROPID* nativePropertyIds, PROPVARIANT* nativePropertyValues, Int32* propertiesCount)
                {
                    if (properIds.HasFlag(propertyId))
                    {
                        SevenZip::NativeInterface::CoderPropertyValueType valueType = propertiesGetter->QueryPropertyValueTypes(propertyId);
                        if (valueType.HasFlag(SevenZip::NativeInterface::CoderPropertyValueType::VT_BOOL))
                        {
                            System::Nullable<bool> propertyValue = propertiesGetter->GetPropertyAsBoolean(propertyId);
                            if (propertyValue.HasValue)
                            {
                                nativePropertyIds[*propertiesCount] = nativePropertyId;
                                nativePropertyValues[*propertiesCount].vt = VT_BOOL;
                                nativePropertyValues[*propertiesCount].boolVal = propertyValue.Value ? VARIANT_TRUE : VARIANT_FALSE;
                                ++(*propertiesCount);
                            }
                        }
                        else if (valueType.HasFlag(SevenZip::NativeInterface::CoderPropertyValueType::VT_UI4))
                        {
                            System::Nullable<UInt32> propertyValue = propertiesGetter->GetPropertyAsUInt32(propertyId);
                            if (propertyValue.HasValue)
                            {
                                nativePropertyIds[*propertiesCount] = nativePropertyId;
                                nativePropertyValues[*propertiesCount].vt = VT_UI4;
                                nativePropertyValues[*propertiesCount].ulVal = propertyValue.Value;
                                ++(*propertiesCount);
                            }
                        }
                        else if (valueType.HasFlag(SevenZip::NativeInterface::CoderPropertyValueType::VT_UI8))
                        {
                            System::Nullable<UInt64> propertyValue = propertiesGetter->GetPropertyAsUInt64(propertyId);
                            if (propertyValue.HasValue)
                            {
                                nativePropertyIds[*propertiesCount] = nativePropertyId;
                                nativePropertyValues[*propertiesCount].vt = VT_UI8;
                                nativePropertyValues[*propertiesCount].uhVal.QuadPart = propertyValue.Value;
                                ++(*propertiesCount);
                            }
                        }
                        else if (valueType.HasFlag(SevenZip::NativeInterface::CoderPropertyValueType::VT_MF))
                        {
                            NativeInterface::Compression::MatchFinderType propertyValue = propertiesGetter->GetPropertyAsMatchFinderType(propertyId);
                            if (propertyValue != NativeInterface::Compression::MatchFinderType::None)
                            {
                                nativePropertyIds[*propertiesCount] = nativePropertyId;
                                nativePropertyValues[*propertiesCount].vt = VT_BSTR;
                                switch (propertyValue)
                                {
                                case NativeInterface::Compression::MatchFinderType::BT2:
                                    nativePropertyValues[*propertiesCount].bstrVal = L"BT2";
                                    break;
                                case NativeInterface::Compression::MatchFinderType::BT3:
                                    nativePropertyValues[*propertiesCount].bstrVal = L"BT3";
                                    break;
                                case NativeInterface::Compression::MatchFinderType::BT4:
                                    nativePropertyValues[*propertiesCount].bstrVal = L"BT4";
                                    break;
                                case NativeInterface::Compression::MatchFinderType::BT5:
                                    nativePropertyValues[*propertiesCount].bstrVal = L"BT5";
                                    break;
                                case NativeInterface::Compression::MatchFinderType::HC4:
                                    nativePropertyValues[*propertiesCount].bstrVal = L"HC4";
                                    break;
                                case NativeInterface::Compression::MatchFinderType::HC5:
                                    nativePropertyValues[*propertiesCount].bstrVal = L"HC5";
                                    break;
                                default:
                                    throw gcnew System::ArgumentException(System::String::Format(L"Unknown match finder type: {0}", propertyValue));
                                }
                                ++(*propertiesCount);
                            }
                        }
                        else
                            throw gcnew System::ArgumentException(System::String::Format(L"Unknown value type: {0}", valueType));
                    }
                }
            };
        }
    }
}
