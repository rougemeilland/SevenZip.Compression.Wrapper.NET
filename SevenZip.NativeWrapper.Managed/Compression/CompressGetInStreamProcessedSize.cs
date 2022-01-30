using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressGetInStreamProcessedSize
        : Unknown, ICompressGetInStreamProcessedSize
    {
        protected CompressGetInStreamProcessedSize(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        UInt64 ICompressGetInStreamProcessedSize.InStreamProcessedSize
        {
            get
            {
                UInt64 processedSize;
                var result = UnmanagedEntryPoint.ICompressGetInStreamProcessedSize__GetInStreamProcessedSize(NativeInterfaceObject, out processedSize);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return processedSize;
            }
        }

        public static ICompressGetInStreamProcessedSize Create(IntPtr nativeInterfaceObject)
        {
            return new CompressGetInStreamProcessedSize(nativeInterfaceObject);
        }
    }
}
