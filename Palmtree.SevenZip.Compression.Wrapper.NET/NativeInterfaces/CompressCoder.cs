using System;
using System.IO;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressCoder
    {
        public static CompressCoder Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressCoder(nativeInterfaceObject);
        }

        public void Code(Stream inStream, Stream outStream, UInt64? inSize, UInt64? outSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));
            if (!inStream.CanRead)
                throw new ArgumentException($"The specified stream ({nameof(inStream)}) does not support reading.", nameof(inStream));
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));
            if (!outStream.CanWrite)
                throw new ArgumentException($"The specified stream ({nameof(outStream)}) does not support writing.", nameof(outStream));

            var result =
                NativeInterOp.ICompressCoder__Code(
                    NativeInterfaceObject,
                    inStream.FromInputStreamToNativeDelegate(),
                    outStream.FromOutputStreamToNativeDelegate(),
                    inSize,
                    outSize,
                    progress.FromProgressToNativeDelegate());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }

        public void Code(ISequentialInputByteStream inStream, ISequentialOutputByteStream outStream, UInt64? inSize, UInt64? outSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));

            var result =
                NativeInterOp.ICompressCoder__Code(
                    NativeInterfaceObject,
                    inStream.FromInputStreamToNativeDelegate(),
                    outStream.FromOutputStreamToNativeDelegate(),
                    inSize,
                    outSize,
                    progress.FromProgressToNativeDelegate());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
