using SevenZip.NativeInterface.Compression;
using SevenZip.NativeInterface.IO;
using SevenZip.NativeWrapper.Managed.Platform;
using System;
namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressCoder
        : Unknown, ICompressCoder
    {
        protected CompressCoder(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressCoder Create(IntPtr nativeInterfaceObject)
        {
            return new CompressCoder(nativeInterfaceObject);
        }

        void ICompressCoder.Code(SequentialInStreamReader sequentialInStreamReader, SequentialOutStreamWriter sequentialOutStreamWriter, UInt64? inSize, UInt64? outSize, CompressProgressInfoReporter? progressReporter)
        {
            var result =
                UnmanagedEntryPoint.ICompressCoder__Code(
                    NativeInterfaceObject,
                    sequentialInStreamReader,
                    sequentialOutStreamWriter,
                    inSize,
                    outSize,
                    progressReporter);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
