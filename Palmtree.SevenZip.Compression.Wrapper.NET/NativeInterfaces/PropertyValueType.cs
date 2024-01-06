using System;

namespace SevenZip.Compression.NativeInterfaces
{
    enum PropertyValueType
        : UInt16
    {
        VT_EMPTY = 0,
        VT_BSTR = 8,
        VT_BOOL = 11,
        VT_UI4 = 19,
        VT_UI8 = 21,
        VT_FILETIME = 64,
    }
}
