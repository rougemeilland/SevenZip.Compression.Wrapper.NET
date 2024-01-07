using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal enum HRESULT
        : Int32
    {
        S_OK = 0,
        S_FALSE = 1,
        E_NOINTERFACE = unchecked((Int32)0x80004002),
        E_NOT_SUPPORTED = unchecked((Int32)0x80004021),
        E_DLL_NOT_FOUND = unchecked((Int32)0x8007007e),
    }
}
