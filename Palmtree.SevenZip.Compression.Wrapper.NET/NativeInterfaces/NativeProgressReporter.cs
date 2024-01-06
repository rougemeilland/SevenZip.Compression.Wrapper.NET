using System;

namespace SevenZip.Compression.NativeInterfaces
{
    delegate void NativeProgressReporter(IntPtr inSize, IntPtr outSize);
}
