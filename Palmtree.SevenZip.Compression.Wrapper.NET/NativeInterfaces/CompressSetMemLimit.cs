using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetMemLimit
    {
        public static CompressSetMemLimit Create(IntPtr nativeInterfaceObject)
            => new(nativeInterfaceObject);

        public void SetMemLimit(UInt64 memUsage)
        {
            var result = NativeInterOp.ICompressSetMemLimit__SetMemLimit(NativeInterfaceObject, memUsage);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
