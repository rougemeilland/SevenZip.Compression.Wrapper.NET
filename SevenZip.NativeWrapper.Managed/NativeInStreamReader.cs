using System;

namespace SevenZip.NativeWrapper.Managed.win.x64
{
    delegate HRESULT NativeInStreamReader(IntPtr buffer, UInt32 size, out UInt32 processedSize);
}
