// Experiment.Cpp.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include <stdio.h>
#include <iostream>
#include <guiddef.h>
#include <windows.h>

int main()
{
    printf("");
    PROPVARIANT propVariant;
    const unsigned char* addreessOfPropVariant = (unsigned char*)&propVariant;
    const unsigned char* addreessOfVT = (unsigned char*)&propVariant.vt;
    const unsigned char* addreessOfBoolean = (unsigned char*)&propVariant.boolVal;
    const unsigned char* addreessOfUInt32 = (unsigned char*)&propVariant.ulVal;
    const unsigned char* addreessOfUInt64 = (unsigned char*)&propVariant.uhVal.QuadPart;
    const unsigned char* addreessOfBSTR = (unsigned char*)&propVariant.bstrVal;

    size_t sizeOfPROPVARIANT = sizeof(PROPVARIANT);
    size_t sizeOfSizeT = sizeof(size_t);

    size_t offsetOfVT = addreessOfVT - addreessOfPropVariant;
    size_t sizeOfVT = sizeof(propVariant.vt);

    size_t offsetOfBoolean = addreessOfBoolean - addreessOfPropVariant;
    size_t sizeOfBoolean = sizeof(propVariant.boolVal);

    size_t offsetOfUInt32 = addreessOfUInt32 - addreessOfPropVariant;
    size_t sizeOfUInt32 = sizeof(propVariant.ulVal);

    size_t offsetOfUInt64 = addreessOfUInt64 - addreessOfPropVariant;
    size_t sizeOfUInt64 = sizeof(propVariant.uhVal.QuadPart);

    size_t offsetOfBSTR = addreessOfBSTR - addreessOfPropVariant;
    size_t sizeOfBSTR = sizeof(propVariant.bstrVal);

    std::cout << "size_t: size=" << sizeOfSizeT << "\n";
    std::cout << "PROPVARIANT: size=" << sizeOfPROPVARIANT << "\n";
    std::cout << "PROPVARIANT.vt: offset=" << offsetOfVT << ", size=" << sizeOfVT << "\n";
    std::cout << "PROPVARIANT.boolVal: offset=" << offsetOfBoolean << ", size=" << sizeOfBoolean << "\n";
    std::cout << "PROPVARIANT.ulVal: offset=" << offsetOfUInt32 << ", size=" << sizeOfUInt32 << "\n";
    std::cout << "PROPVARIANT.uhVal.QuadPart: offset=" << offsetOfUInt64 << ", size=" << sizeOfUInt64 << "\n";
    std::cout << "PROPVARIANT.bstrVal: offset=" << offsetOfBSTR << ", size=" << sizeOfBSTR << "\n";
}
