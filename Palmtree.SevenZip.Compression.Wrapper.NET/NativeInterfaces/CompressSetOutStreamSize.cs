using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetOutStreamSize
    {
        public static CompressSetOutStreamSize Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetOutStreamSize(nativeInterfaceObject);
        }

        public void SetOutStreamSize(UInt64? outSize)
        {
            var result = NativeInterOp.ICompressSetOutStreamSize__SetOutStreamSize(NativeInterfaceObject, outSize);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
