using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetBufSize
    {
        public static CompressSetBufSize Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetBufSize(nativeInterfaceObject);
        }

        public void SetInBufSize(UInt32 streamIndex, UInt32 size)
        {
            var result = NativeInterOp.ICompressSetBufSize__SetInBufSize(NativeInterfaceObject, streamIndex, size);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }

        public void SetOutBufSize(UInt32 streamIndex, UInt32 size)
        {
            var result = NativeInterOp.ICompressSetBufSize__SetOutBufSize(NativeInterfaceObject, streamIndex, size);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
