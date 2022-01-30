#pragma once

#include <vcclr.h>
#include "Platform.h"
#include "CoderType.h"

#define __ThrowExceptionForHR(result)\
    {\
        System::Runtime::InteropServices::Marshal::ThrowExceptionForHR(result);\
        throw gcnew System::Exception(L"Unexpected code was executed.");\
    }\

namespace SevenZip
{
    namespace NativeWrapper
    {
        __INLINE static SevenZip::NativeInterface::CoderType FromNativeCoderTypeToManagedCoderType(CoderType coderType)
        {
            switch (coderType)
            {
            case SevenZip::NativeWrapper::Decoder:
                return SevenZip::NativeInterface::CoderType::Decoder;
            case SevenZip::NativeWrapper::Encoder:
                return SevenZip::NativeInterface::CoderType::Encoder;
            default:
                return SevenZip::NativeInterface::CoderType::Unknown;
            }
        }

        __INLINE static CoderType FromManagedCoderTypeToNativeCoderType(SevenZip::NativeInterface::CoderType coderType)
        {
            switch (coderType)
            {
            case SevenZip::NativeInterface::CoderType::Decoder:
                return CoderType::Decoder;
            case SevenZip::NativeInterface::CoderType::Encoder:
                return CoderType::Encoder;
            default:
                return CoderType::Unknown;
            }
        }

        __INLINE static System::Guid FromNativeGuidToManagedGuid(const GUID& guid)
        {
            return
                System::Guid(
                    guid.Data1,
                    guid.Data2,
                    guid.Data3,
                    guid.Data4[0],
                    guid.Data4[1],
                    guid.Data4[2],
                    guid.Data4[3],
                    guid.Data4[4],
                    guid.Data4[5],
                    guid.Data4[6],
                    guid.Data4[7]);
        }

        __INLINE static GUID FromManagedGuidToNativeGuid(System::Guid& guid)
        {
            array<Byte>^ guidArrayBuffer = guid.ToByteArray();
            pin_ptr<Byte> data = &(guidArrayBuffer[0]);
            return *(_GUID*)data;
        }

        __INLINE static const UInt64* FromNullableUInt64ToUInt64Pointer(System::Nullable<UInt64> value, UInt64* buffer)
        {
            if (!value.HasValue)
                return nullptr;
            *buffer = value.Value;
            return buffer;
        }
    }
}
