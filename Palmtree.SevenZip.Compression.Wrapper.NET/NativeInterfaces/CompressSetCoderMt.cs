using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetCoderMt
    {
        public static CompressSetCoderMt Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetCoderMt(nativeInterfaceObject);
        }

        public void SetNumberOfThreads(UInt32 numThreads)
        {
            var result = NativeInterOp.ICompressSetCoderMt__SetNumberOfThreads(NativeInterfaceObject, numThreads);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
