using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetDecoderProperties2
    {
        public static CompressSetDecoderProperties2 Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetDecoderProperties2(nativeInterfaceObject);
        }

        public void SetDecoderProperties2(ReadOnlySpan<Byte> data)
        {
            var result = NativeInterOp.ICompressSetDecoderProperties2__SetDecoderProperties2(NativeInterfaceObject, data);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
