using System;
using System.Runtime.InteropServices;

namespace SevenZip.Compression.NativeInterfaces
{
    [StructLayout(LayoutKind.Explicit, Pack = 8, Size = 8)]
    struct NativeFILETIME
    {
        [FieldOffset(0)]
        public UInt32 LowDateTime;

        [FieldOffset(4)]
        public UInt32 HighDateTime;

        [FieldOffset(0)]
        public UInt64 DateTime;
    }
}
