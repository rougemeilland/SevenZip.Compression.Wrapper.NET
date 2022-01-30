using System;

namespace SevenZip.NativeWrapper.Managed
{
    delegate HRESULT NativeInStreamReader(IntPtr buffer, UInt32 size, out UInt32 processedSize);
}
