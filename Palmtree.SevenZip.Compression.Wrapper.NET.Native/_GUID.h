#pragma once

#ifdef __GNUC__

#include <stdint.h>
#include <string.h>
#include "_types.h"

#ifdef __cplusplus
#define REFGUID const GUID &
#else
#define REFGUID const GUID *
#endif

#define IsEqualIID(riid1, riid2) IsEqualGUID(riid1, riid2)

typedef struct {
    uint32_t    Data1;
    uint16_t    Data2;
    uint16_t    Data3;
    uint8_t     Data4[8];
} GUID;

typedef REFGUID REFIID;

inline int IsEqualGUID(REFGUID rguid1, REFGUID rguid2)
{
    return memcmp(&rguid1, &rguid2, sizeof(GUID)) == 0;
}

#endif // __GNUC__
