using System;

namespace SevenZip.Compression.NativeInterfaces
{
    delegate HRESULT NativeOutStreamWriter(IntPtr buffer, UInt32 size, out UInt32 processedSize);
}
