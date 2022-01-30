#pragma once

#include "Platform.h"

namespace SevenZip
{
    namespace NativeWrapper
    {
        __INLINE static bool IsEqualWideString(const wchar_t* s1, const wchar_t* s2)
        {
            while (true)
            {
                if (*s1 != *s2)
                    return false;
                if (*s1 == L'\0')
                    return true;
                ++s1;
                ++s2;
            }
        }

        __INLINE static bool IsEqualWideStringWithIgnoreCase(const wchar_t* s1, const wchar_t* s2)
        {
            while (true)
            {
                wchar_t c1 = *s1;
                if (c1 >= L'a' && c1 <= L'z')
                    c1 += L'A' - L'a';
                wchar_t c2 = *s2;
                if (c2 >= L'a' && c2 <= L'z')
                    c2 += L'A' - L'a';
                if (c1 != c2)
                    return false;
                if (c1 == L'\0')
                    return true;
                ++s1;
                ++s2;
            }
        }

        __INLINE static void CopyWideString(wchar_t* destinationString, const wchar_t* sourceString)
        {
            while (*sourceString != L'\0')
                *destinationString++ = *sourceString++;
            *destinationString = L'\0';
        }

        __INLINE static Int32 GetWideStringLength(const wchar_t* s)
        {
            Int32 length = 0;
            while (*s != L'\0')
            {
                ++s;
                ++length;
            }
            return length;
        }
    }
}
