using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressProgressInfo
    {
        public static CompressProgressInfo Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressProgressInfo(nativeInterfaceObject);
        }
    }
}
