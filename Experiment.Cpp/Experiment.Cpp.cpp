// Experiment.Cpp.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include <stdio.h>
#include <iostream>
#include <guiddef.h>
#include <windows.h>
#include <atlcomcli.h>

size_t GetSizeOfShort()
{
    return sizeof(short);
}

size_t GetSizeOfInt()
{
    return sizeof(int);
}

size_t GetSizeOfLong()
{
    return sizeof(long);
}

size_t GetSizeOfLongLong()
{
    return sizeof(long long);
}

size_t GetSizeOfPointer()
{
    return sizeof(void*);
}

size_t GetSizeOfPROPVARIANT()
{
    return sizeof(PROPVARIANT);
}

void* GetOffsetOfVt(PROPVARIANT* p)
{
    return &p->vt;
}

size_t GetSizeofVt(PROPVARIANT* p)
{
    return sizeof(p->vt);
}

void* GetOffsetOfBooleanValue(PROPVARIANT* p)
{
    return &p->boolVal;
}

size_t GetSizeofBooleanValue(PROPVARIANT* p)
{
    return sizeof(p->boolVal);
}

void* GetOffsetOfUInt32Value(PROPVARIANT* p)
{
    return &p->ulVal;
}

size_t GetSizeofUInt32Value(PROPVARIANT* p)
{
    return sizeof(p->ulVal);
}

void* GetOffsetOfUInt64Value(PROPVARIANT* p)
{
    return &p->uhVal.QuadPart;
}

size_t GetSizeofUInt64Value(PROPVARIANT* p)
{
    return sizeof(p->uhVal.QuadPart);
}

void* GetOffsetOfBSTRValue(PROPVARIANT* p)
{
    return p->bstrVal;
}

size_t GetSizeofBSTRValue(PROPVARIANT* p)
{
    return sizeof(p->bstrVal);
}

int main()
{
    HINSTANCE hModule = LoadLibraryA("xxx.dll");
    DWORD err = GetLastError();
    HRESULT result = AtlHresultFromLastError();
}
