#pragma once

#ifdef __GNUC__

#include "_GUID.h"
#include "_types.h"

#define S_OK        ((HRESULT)0x00000000L)
#define S_FALSE     ((HRESULT)0x00000001L)
#define E_NOTIMPL   ((HRESULT)0x80004001L)
#define E_NOINTERFACE   ((HRESULT)0x80004002L)
#define E_ABORT     ((HRESULT)0x80004004L)
#define E_FAIL      ((HRESULT)0x80004005L)
#define E_UNEXPECTED    ((HRESULT)0x8000ffffL)
#define STG_E_INVALIDFUNCTION   ((HRESULT)0x80030001L)
#define CLASS_E_CLASSNOTAVAILABLE   ((HRESULT)0x80040111L)
#define E_DLL_NOT_FOUND ((HRESULT)0x8007007e)

typedef Int32   HRESULT;
typedef UInt32  PROPID;

extern "C" const GUID IID_IUnknown;
struct IUnknown
{
    virtual HRESULT QueryInterface(REFIID riid, void** outObject) = 0;
    virtual ULONG AddRef(void) = 0;
    virtual ULONG Release(void) = 0;
};

typedef struct _FILETIME {
    UInt32  dwLowDateTime;
    UInt32  dwHighDateTime;
} FILETIME, * PFILETIME, * LPFILETIME;

typedef struct { int64_t QuadPart; } LARGE_INTEGER;
typedef struct { uint64_t QuadPart; } ULARGE_INTEGER;

typedef int16_t VARTYPE;

typedef struct
{
    VARTYPE vt;
    int16_t wReserved1;
    int16_t wReserved2;
    int16_t wReserved3;
    union
    {
        int8_t cVal;
        uint8_t bVal;
        int16_t iVal;
        uint16_t uiVal;
        int32_t lVal;
        uint32_t ulVal;
        int32_t intVal;
        uint32_t uintVal;
        LARGE_INTEGER hVal;
        ULARGE_INTEGER uhVal;
        uint16_t boolVal;
        int32_t scode;
        FILETIME filetime;
        wchar_t* bstrVal;
    };
} PROPVARIANT;

#endif
