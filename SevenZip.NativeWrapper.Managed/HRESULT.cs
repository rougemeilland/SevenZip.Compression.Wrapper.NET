using System;

namespace SevenZip.NativeWrapper.Managed
{
    enum HRESULT
        : Int32
    {
        S_OK = 0,
        E_NOINTERFACE = unchecked((Int32)0x80004002),
    }
}
