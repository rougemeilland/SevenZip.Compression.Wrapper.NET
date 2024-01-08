#pragma once

#ifdef __GNUC__
#include <stdint.h>

#ifndef ULONG_MAX
#define	ULONG_MAX	((ULONG)-1)
#endif // !ULONG_MAX

typedef int8_t SByte;
typedef uint8_t Byte;
typedef int16_t Int16;
typedef uint16_t UInt16;
typedef int32_t Int32;
typedef uint32_t UInt32;
typedef int64_t Int64;
typedef uint64_t UInt64;

typedef UInt16  WORD;
typedef UInt32  DWORD;
typedef UInt32  ULONG;
#endif // __GNUC__
