#include "ClrInterOperation.h"
#include "ManagedCompressCodecInfo.h"
#include "ManagedCompressCoder.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        namespace Compression
        {
            ManagedCompressCodecInfo::ManagedCompressCodecInfo(NativeEntryPoint* nativeResource, Int32 index, UInt64 id, System::String^ coderName, System::Guid coderClassId, NativeInterface::CoderType coderType, bool isSupportedICompressCoder)
            {
                _nativeResource = nativeResource;
                _nativeResource->AddRef();
                Index = index;
                ID = id;
                CodecName = coderName;
                CoderClassId = coderClassId;
                CoderType = coderType;
                IsSupportedICompressCoder = isSupportedICompressCoder;
            }

            ManagedCompressCodecInfo::ManagedCompressCodecInfo(ManagedCompressCodecInfo% p)
            {
                _nativeResource = p._nativeResource;
                _nativeResource->AddRef();
                Index = p.Index;
                ID = p.ID;
                CodecName = p.CodecName;
                CoderClassId = p.CoderClassId;
                CoderType = p.CoderType;
                IsSupportedICompressCoder = p.IsSupportedICompressCoder;
            }

            ManagedCompressCodecInfo::~ManagedCompressCodecInfo()
            {
                this->!ManagedCompressCodecInfo();
            }

            ManagedCompressCodecInfo::!ManagedCompressCodecInfo()
            {
                if (_nativeResource != nullptr)
                {
                    _nativeResource->Release();
                    _nativeResource = nullptr;
                }
            }

            SevenZip::NativeInterface::Compression::ICompressCoder^ ManagedCompressCodecInfo::CreateCompressCoder()
            {
                if (!IsSupportedICompressCoder)
                    throw gcnew System::NotSupportedException();
                SevenZipInterface::ICompressCoder* _coder = nullptr;
                HRESULT result;
                switch (CoderType)
                {
                case SevenZip::NativeInterface::CoderType::Decoder:
                    result = _nativeResource->CreateDecoder(Index, &__UUIDOF(SevenZipInterface::ICompressCoder), (void**)&_coder);
                    break;
                case SevenZip::NativeInterface::CoderType::Encoder:
                    result = _nativeResource->CreateEncoder(Index, &__UUIDOF(SevenZipInterface::ICompressCoder), (void**)&_coder);
                    break;
                default:
                    throw gcnew System::Exception(L"Illegal coder type.");
                    break;
                }
                if (result != S_OK)
                {
                    if (result == E_NOINTERFACE)
                        throw gcnew System::NotSupportedException();
                    __ThrowExceptionForHR(result);
                }
                return ManagedCompressCoder::Create(_coder);
            }
        }
    }
}
