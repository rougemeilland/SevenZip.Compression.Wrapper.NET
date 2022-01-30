#pragma once

#ifdef _MSC_VER

#include <windows.h>
#include <guiddef.h>

#define __INLINE __inline

typedef unsigned char Byte;
typedef short Int16;
typedef unsigned short UInt16;
typedef signed int Int32;
typedef unsigned int UInt32;
typedef signed long long Int64;
typedef unsigned long long UInt64;
#else
#error "No platform-dependent types or macros are defined."
#endif

#if     defined(_PLATFORM_WINDOWS_X86)
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_32_BIT
#elif   defined(_PLATFORM_WINDOWS_X64)
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_64_BIT
#else
#error "No architecture-maceos are defined."
#endif


// Writing code that calls ThrowExceptionForHR also seems to execute the following line in C ++ syntax.
// Therefore, you may get a warning at compile time, but to avoid that problem, use this macro instead of ThrowExceptionForHR.
namespace SevenZip
{
    namespace NativeWrapper
    {
#ifdef _DEBUG
        __INLINE static void ValidateTypes()
        {
#pragma warning( disable : 6285) // Suppress compiler warnings for constant-only conditional decisions
            if (sizeof(Byte) != 1 || (Byte)(long long)-1 < 0)
                throw gcnew System::Exception(L"Detected that 'Byte' is not an unsigned 8-bit integer.");
            if (sizeof(Int16) != 2 || (Int16)(long long)-1 >= 0)
                throw gcnew System::Exception(L"Detected that 'Int16' is not a signed 16-bit integer.");
            if (sizeof(UInt16) != 2 || (UInt16)(long long)-1 < 0)
                throw gcnew System::Exception(L"Detected that 'UInt16' is not an unsigned 16-bit integer.");
            if (sizeof(Int32) != 4 || (Int32)(long long)-1 >= 0)
                throw gcnew System::Exception(L"Detected that 'Int32' is not a signed 32-bit integer.");
            if (sizeof(HRESULT) != 4 || (HRESULT)(long long)-1 >= 0)
                throw gcnew System::Exception(L"Detected that 'HRESULT' is not a signed 32-bit integer.");
            if (sizeof(UInt32) != 4 || (UInt32)(long long)-1 < 0)
                throw gcnew System::Exception(L"Detected that 'UInt32' is not an unsigned 32-bit integer.");
            if (sizeof(ULONG) != 4 || (ULONG)(long long)-1 < 0)
                throw gcnew System::Exception(L"Detected that 'ULONG' is not an unsigned 32-bit integer.");
            if (sizeof(Int64) != 8 || (Int64)(long long)-1 >= 0)
                throw gcnew System::Exception(L"Detected that 'Int64' is not a signed 64-bit integer.");
            if (sizeof(UInt64) != 8 || (UInt64)(long long)-1 < 0)
                throw gcnew System::Exception(L"Detected that 'UInt64' is not an unsigned 64-bit integer.");
            {
                BSTR x = nullptr;
                if (sizeof(*x) != sizeof(wchar_t))
                    throw gcnew System::Exception(L"Detected that \"BSTR\" is not a pointer to \"wchar_t\".");
            }
#pragma warning( default : 6285) // Suppress compiler warnings for constant-only conditional decisions
        }
#endif
    }
}
