using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressReadUnusedFromInBuf
        : Unknown, ICompressReadUnusedFromInBuf
    {
        protected CompressReadUnusedFromInBuf(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressReadUnusedFromInBuf Create(IntPtr nativeInterfaceObject)
        {
            return new CompressReadUnusedFromInBuf(nativeInterfaceObject);
        }

        UInt32 ICompressReadUnusedFromInBuf.ReadUnusedFromInBuf(Span<Byte> data)
        {
            UInt32 processedSize;
            var result = UnmanagedEntryPoint.ICompressReadUnusedFromInBuf__ReadUnusedFromInBuf(NativeInterfaceObject, data, out processedSize);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return processedSize;
        }
    }
}
