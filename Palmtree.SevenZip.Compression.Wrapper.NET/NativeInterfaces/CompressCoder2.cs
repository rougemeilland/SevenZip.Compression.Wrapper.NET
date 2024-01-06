using System;
using System.IO;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressCoder2
    {
        public static CompressCoder2 Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressCoder2(nativeInterfaceObject);
        }

        public void Code(ReadOnlySpan<(Stream inStream, UInt64? inStreamSize)> inStreams, ReadOnlySpan<(Stream outStream, UInt64? outStreamSize)> outStreams, IProgress<(UInt64? inSize, UInt64? outSize)>? progress)
        {
            var newInStreams = new (NativeInStreamReader, UInt64?)[inStreams.Length];
            for (var index = 0; index < inStreams.Length; ++index)
            {
                var (inStream, inStreamSize) = inStreams[index];
                newInStreams[index] = (inStream.FromInputStreamToNativeDelegate(), inStreamSize);
            }

            var newOutStreams = new (NativeOutStreamWriter, UInt64?)[outStreams.Length];
            for (var index = 0; index < outStreams.Length; ++index)
            {
                var (outStream, outStreamSize) = outStreams[index];
                newOutStreams[index] = (outStream.FromOutputStreamToNativeDelegate(), outStreamSize);
            }

            Code(newInStreams, newOutStreams, progress);
        }

        public void Code(ReadOnlySpan<(ISequentialInputByteStream inStream, UInt64? inStreamSize)> inStreams, ReadOnlySpan<(ISequentialOutputByteStream outStream, UInt64? outStreamSize)> outStreams, IProgress<(UInt64? inSize, UInt64? outSize)>? progress)
        {
            var newInStreams = new (NativeInStreamReader, UInt64?)[inStreams.Length];
            for (var index = 0; index < inStreams.Length; ++index)
            {
                var (inStream, inStreamSize) = inStreams[index];
                newInStreams[index] = (inStream.FromInputStreamToNativeDelegate(), inStreamSize);
            }

            var newOutStreams = new (NativeOutStreamWriter, UInt64?)[outStreams.Length];
            for (var index = 0; index < outStreams.Length; ++index)
            {
                var (outStream, outStreamSize) = outStreams[index];
                newOutStreams[index] = (outStream.FromOutputStreamToNativeDelegate(), outStreamSize);
            }

            Code(newInStreams, newOutStreams, progress);
        }

        private void Code(ReadOnlySpan<(NativeInStreamReader inStreamReader, UInt64? inStreamSize)> inStreams, ReadOnlySpan<(NativeOutStreamWriter outStreamWriter, UInt64? outStreamSize)> outStreams, IProgress<(UInt64? inSize, UInt64? outSize)>? progress)
        {
            var result =
                NativeInterOp.ICompressCoder2__Code(
                    NativeInterfaceObject,
                    inStreams,
                    outStreams,
                    progress);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
