#pragma once

#ifdef _MSC_VER

#include <windows.h>
#include <atlcomcli.h> // Required to convert the Windows API error code (the value that can be obtained with GetLastError) to HRESULT.

#define __INLINE __inline

typedef unsigned char Byte;
typedef short Int16;
typedef unsigned short UInt16;
typedef signed int Int32;
typedef unsigned int UInt32;
typedef signed long long Int64;
typedef unsigned long long UInt64;
#elif false
#error "If you use other compilers, identify the compiler type and define the following macros and types appropriately. : __INLINE macro / GUID structure / IUnknown structure / PROPVARIANT structure / Byte type / Int16 type / UInt16 type / Int32 type / UInt32 type / Int64 type / UInt64 type / PROPID type. In particular, for GUID structures, IUnknown structures, and PROPVARIANT structures, refer to the source code of the 7z library to match the memory layout."
#else
#error "No platform-dependent types or macros are defined."
#endif

#if     defined(_PLATFORM_WINDOWS_X86)
#define _PLATFORM_WINDOWS
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_32_BIT
#define __DEFINE_PUBLIC_FUNC(type, interfaceName, methodName) extern "C" __declspec(dllexport) type __stdcall EXPORTED_ ## interfaceName ## __ ## methodName
#elif   defined(_PLATFORM_WINDOWS_X64)
#define _PLATFORM_WINDOWS
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_64_BIT
#define __DEFINE_PUBLIC_FUNC(type, interfaceName, methodName) extern "C" __declspec(dllexport) type __stdcall EXPORTED_ ## interfaceName ## __ ## methodName
#elif   defined(_PLATFORM_LINUX_X86)
#error "linux is not supported. If you want to support linux, write as follows, for example."
#define _PLATFORM_LINUX
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_32_BIT
#define __DEFINE_PUBLIC_FUNC(type, interfaceName, methodName) extern "C"  type EXPORTED_ ## interfaceName ## __ ## methodName
typedef struct _FILETIME {
    DWORD dwLowDateTime;
    DWORD dwHighDateTime;
} FILETIME, * PFILETIME, * LPFILETIME;
#elif   defined(_PLATFORM_LINUX_X64)
#error "linux is not supported. If you want to support linux, write as follows, for example."
#define _PLATFORM_LINUX
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_64_BIT
#define __DEFINE_PUBLIC_FUNC(type, interfaceName, methodName) extern "C"  type EXPORTED_ ## interfaceName ## __ ## methodName
typedef struct _FILETIME {
    DWORD dwLowDateTime;
    DWORD dwHighDateTime;
} FILETIME, * PFILETIME, * LPFILETIME;
#elif   defined(_PLATFORM_MACOS_X86)
#error "MacOS is not supported. If you want to support MacOS, write as follows, for example."
#define _PLATFORM_MACOS
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_32_BIT
#define __DEFINE_PUBLIC_FUNC(type, interfaceName, methodName) extern "C"  type EXPORTED_ ## interfaceName ## __ ## methodName
typedef struct _FILETIME {
    DWORD dwLowDateTime;
    DWORD dwHighDateTime;
} FILETIME, * PFILETIME, * LPFILETIME;
#elif   defined(_PLATFORM_MACOS_X64)
#error "MacOS is not supported. If you want to support MacOS, write as follows, for example."
#define _PLATFORM_MACOS
#define _ARCHITECTURE_LITTLE_ENDIAN
#define _ARCHITECTURE_NATIVE_INTEGER_64_BIT
#define __DEFINE_PUBLIC_FUNC(type, interfaceName, methodName) extern "C"  type EXPORTED_ ## interfaceName ## __ ## methodName
typedef struct _FILETIME {
    DWORD dwLowDateTime;
    DWORD dwHighDateTime;
} FILETIME, * PFILETIME, * LPFILETIME;
#else
#error "No architecture-maceos are defined."
#endif

#ifdef _DEBUG
//Code that self-diagnoses that the type definition is correct.
__INLINE static const wchar_t* ValidateTypes()
{
#pragma warning( disable : 6285) // In MSC ++, suppress warnings for comparisons between constants.
    if (sizeof(Byte) != 1 || (Byte)(long long)-1 < 0)
        return L"Detected that 'Byte' is not an unsigned 8-bit integer.";
    if (sizeof(Int16) != 2 || (Int16)(long long)-1 >= 0)
        return L"Detected that 'Int16' is not a signed 16-bit integer.";
    if (sizeof(UInt16) != 2 || (UInt16)(long long)-1 < 0)
        return L"Detected that 'UInt16' is not an unsigned 16-bit integer.";
    if (sizeof(Int32) != 4 || (Int32)(long long)-1 >= 0)
        return L"Detected that 'Int32' is not a signed 32-bit integer.";
    if (sizeof(HRESULT) != 4 || (HRESULT)(long long)-1 >= 0)
        return L"Detected that 'HRESULT' is not a signed 32-bit integer.";
    if (sizeof(UInt32) != 4 || (UInt32)(long long)-1 < 0)
        return L"Detected that 'UInt32' is not an unsigned 32-bit integer.";
    if (sizeof(ULONG) != 4 || (ULONG)(long long)-1 < 0)
        return L"Detected that 'ULONG' is not an unsigned 32-bit integer.";
    if (sizeof(Int64) != 8 || (Int64)(long long)-1 >= 0)
        return L"Detected that 'Int64' is not a signed 64-bit integer.";
    if (sizeof(UInt64) != 8 || (UInt64)(long long)-1 < 0)
        return L"Detected that 'UInt64' is not an unsigned 64-bit integer.";
    {
        BSTR x = nullptr;
        if (sizeof(*x) != sizeof(wchar_t))
            return L"Detected that \"BSTR\" is not a pointer to \"wchar_t\".";
    }
    return nullptr;
#pragma warning( default : 6285) // In MSC ++, unsuppresses warnings for comparisons between constants.
}
#endif
