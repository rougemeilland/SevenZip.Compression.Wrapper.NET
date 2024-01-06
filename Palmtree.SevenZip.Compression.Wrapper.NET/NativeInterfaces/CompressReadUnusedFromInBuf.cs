using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressReadUnusedFromInBuf
    {
        public static CompressReadUnusedFromInBuf Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressReadUnusedFromInBuf(nativeInterfaceObject);
        }

        public UInt32 ReadUnusedFromInBuf(Span<Byte> data)
        {
            var result = NativeInterOp.ICompressReadUnusedFromInBuf__ReadUnusedFromInBuf(NativeInterfaceObject, data, out UInt32 processedSize);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return processedSize;
        }
    }
}
