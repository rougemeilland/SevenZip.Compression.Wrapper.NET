using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetCoderMt
        : Unknown, ICompressSetCoderMt
    {
        protected CompressSetCoderMt(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetCoderMt Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetCoderMt(nativeInterfaceObject);
        }

        void ICompressSetCoderMt.SetNumberOfThreads(UInt32 numThreads)
        {
            var result = UnmanagedEntryPoint.ICompressSetCoderMt__SetNumberOfThreads(NativeInterfaceObject, numThreads);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
