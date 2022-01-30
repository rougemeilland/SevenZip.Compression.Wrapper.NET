#pragma once

#include "Platform.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        __INLINE static Byte STDMETHODCALLTYPE ParseHexadecimalWideChar(wchar_t c)
        {
            if (c >= L'0' && c <= L'9')
                return c - L'0';
            else if (c >= L'A' && c <= L'F')
                return c - L'A' + 10;
            else if (c >= L'a' && c <= L'f')
                return c - L'a' + 10;
            else
                throw gcnew System::FormatException("Bad GUID format");
        }

        __INLINE static Byte STDMETHODCALLTYPE ParseAsByteFromWideCharString(const wchar_t* p)
        {
            Byte result = 0;
            result |= ParseHexadecimalWideChar(p[0]) << 0;
            result |= ParseHexadecimalWideChar(p[1]) << 4;
            return result;
        }

        __INLINE static UInt16 STDMETHODCALLTYPE ParseAsUInt16FromWideCharString(const wchar_t* p)
        {
            UInt16 result = 0;
            result |= ParseAsByteFromWideCharString(p + 0) << 0;
            result |= ParseAsByteFromWideCharString(p + 2) << 8;
            return result;
        }

        __INLINE static UInt32 STDMETHODCALLTYPE ParseAsUInt32FromWideCharString(const wchar_t* p)
        {
            UInt32 result = 0;
            result |= ParseAsUInt16FromWideCharString(p + 0) << 0;
            result |= ParseAsUInt16FromWideCharString(p + 4) << 16;
            return result;
        }

        __INLINE static GUID ParseAsGUIDFromWideCharString(const wchar_t* p)
        {
            GUID result = { 0, 0, 0, {0, 0, 0, 0, 0, 0, 0, 0} };
            result.Data1 = ParseAsUInt32FromWideCharString(p);
            p += 8;
            if (*p != L'-')
                throw gcnew System::FormatException("Bad GUID format");
            ++p;
            result.Data2 = ParseAsUInt16FromWideCharString(p);
            p += 4;
            if (*p != L'-')
                throw gcnew System::FormatException("Bad GUID format");
            ++p;
            result.Data3 = ParseAsUInt16FromWideCharString(p);
            p += 4;
            if (*p != L'-')
                throw gcnew System::FormatException("Bad GUID format");
            ++p;
            result.Data4[0] = ParseAsByteFromWideCharString(p);
            p += 2;
            result.Data4[1] = ParseAsByteFromWideCharString(p);
            p += 2;
            if (*p != L'-')
                throw gcnew System::FormatException("Bad GUID format");
            ++p;
            result.Data4[2] = ParseAsByteFromWideCharString(p);
            p += 2;
            result.Data4[3] = ParseAsByteFromWideCharString(p);
            p += 2;
            result.Data4[4] = ParseAsByteFromWideCharString(p);
            p += 2;
            result.Data4[5] = ParseAsByteFromWideCharString(p);
            p += 2;
            result.Data4[6] = ParseAsByteFromWideCharString(p);
            p += 2;
            result.Data4[7] = ParseAsByteFromWideCharString(p);
            p += 2;
            if (*p != 0)
                throw gcnew System::FormatException("Bad GUID format");
            return result;
        }

        __INLINE static bool IsEqualInterfaceId(const GUID& x, const GUID& y)
        {
#if defined(_ARCHITECTURE_NATIVE_INTEGER_32_BIT)
            const UInt32* x_ptr = (const UInt32*)&x;
            const UInt32* y_ptr = (const UInt32*)&y;
            return
                x_ptr[3] == y_ptr[3]
                && x_ptr[2] == y_ptr[2]
                && x_ptr[1] == y_ptr[1]
                && x_ptr[0] == y_ptr[0];
#elif defined (_ARCHITECTURE_NATIVE_INTEGER_64_BIT)
            const UInt64* x_ptr = (const UInt64*)&x;
            const UInt64* y_ptr = (const UInt64*)&y;
            return
                x_ptr[1] == y_ptr[1]
                && x_ptr[0] == y_ptr[0];
#else
#error "unexpected architecture"
#endif

        }
    }
}
