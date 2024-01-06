using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressGetInStreamProcessedSize
    {
        public UInt64 InStreamProcessedSize
        {
            get
            {
                var result = NativeInterOp.ICompressGetInStreamProcessedSize__GetInStreamProcessedSize(NativeInterfaceObject, out UInt64 processedSize);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return processedSize;
            }
        }

        public static CompressGetInStreamProcessedSize Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressGetInStreamProcessedSize(nativeInterfaceObject);
        }
    }
}
