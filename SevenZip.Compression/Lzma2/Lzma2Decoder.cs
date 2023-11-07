using SevenZip.IO;
using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression.Lzma2
{
    /// <summary>
    /// A class of LZMA2 decoders.
    /// </summary>
    public class Lzma2Decoder
        : IDisposable
    {
        /// <summary>
        /// The length of the property embedded in the compressed stream in LZMA2 format.
        /// </summary>
        public const Int32 LZMA2_CONTENT_PROPERTY_SIZE = 1;

        private readonly ICompressCoder _compressCoder;
        private readonly ICompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;

        private bool _isDisposed;

        private Lzma2Decoder(ICompressCoder compressCoder, ICompressGetInStreamProcessedSize compressGetInStreamProcessedSize)
        {
            _isDisposed = false;
            _compressCoder = compressCoder;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
        }

        /// <summary>
        /// The number of bytes of processed data in the decoder's input stream.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        public UInt64 InStreamProcessedSize
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().FullName);

                return _compressGetInStreamProcessedSize.InStreamProcessedSize;
            }
        }

        /// <summary>
        /// Create an instance of <see cref="Lzma2Decoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the LZMA decoder.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in LZMA format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Lzma2Decoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static Lzma2Decoder Create(Lzma2DecoderProperties properties, ReadOnlySpan<Byte> contentProperties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            ICompressCoder? compressCoder = null;
            ICompressSetDecoderProperties2? compressSetDecoderProperties2 = null;
            ICompressSetFinishMode? compressSetFinishMode = null;
            ICompressGetInStreamProcessedSize? compressGetInStreamProcessedSize = null;
            ICompressSetBufSize? compressSetBufSize = null;
            ICompressSetCoderMt? compressSetCoderMt = null;
            ICompressSetMemLimit? compressSetMemLimit = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("LZMA2", CoderType.Decoder);
                compressGetInStreamProcessedSize = (ICompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(ICompressGetInStreamProcessedSize));
                compressSetDecoderProperties2 = (ICompressSetDecoderProperties2)compressCoder.QueryInterface(typeof(ICompressSetDecoderProperties2));
                compressSetDecoderProperties2.SetDecoderProperties2(contentProperties);
                if (properties.FinishMode.HasValue)
                {
                    compressSetFinishMode = (ICompressSetFinishMode)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }
                if (properties.InBufSize.HasValue || properties.OutBufSize.HasValue)
                {
                    compressSetBufSize = (ICompressSetBufSize)compressCoder.QueryInterface(typeof(ICompressSetBufSize));
                    if (properties.InBufSize.HasValue)
                    {
                        // The streamIndex parameter is ignored in the LZMA decoder.
                        compressSetBufSize.SetInBufSize(0, properties.InBufSize.Value);
                    }
                    if (properties.OutBufSize.HasValue)
                    {
                        // The streamIndex parameter is ignored in the LZMA decoder.
                        compressSetBufSize.SetOutBufSize(0, properties.OutBufSize.Value);
                    }
                }
                if (properties.NumThreads.HasValue)
                {
                    compressSetCoderMt = (ICompressSetCoderMt)compressCoder.QueryInterface(typeof(ICompressSetCoderMt));
                    compressSetCoderMt.SetNumberOfThreads(properties.NumThreads.Value);
                }
                if (properties.MemUsage.HasValue)
                {
                    compressSetMemLimit = (ICompressSetMemLimit)compressCoder.QueryInterface(typeof(ICompressSetMemLimit));
                    compressSetMemLimit.SetMemLimit(properties.MemUsage.Value);
                }
                var decoder = new Lzma2Decoder(compressCoder, compressGetInStreamProcessedSize);
                success = true;
                return decoder;
            }
            finally
            {
                if (!success)
                {
                    (compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                    (compressCoder as IDisposable)?.Dispose();
                }
                (compressSetMemLimit as IDisposable)?.Dispose();
                (compressSetCoderMt as IDisposable)?.Dispose();
                (compressSetFinishMode as IDisposable)?.Dispose();
                (compressSetBufSize as IDisposable)?.Dispose();
                (compressSetDecoderProperties2 as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with LZMA, and write the uncompressed data to the output stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set a stream to read the compressed input data.
        /// </param>
        /// <param name="uncompressedOutStream">
        /// Set a stream to write the uncompressed input data.
        /// </param>
        /// <param name="compressedInStreamSize">
        /// <para>
        /// Set the length in bytes of the compressed data.
        /// </para>
        /// <para>
        /// Set null if the length of the data is unknown.
        /// </para>
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data.
        /// </para>
        /// <para>
        /// Set null if the length of the data is unknown.
        /// </para>
        /// </param>
        /// <param name="progress">
        /// <para>
        /// Set an object to receive notification of coding progress.
        /// </para>
        /// <para>
        /// Set to null if you do not need to be notified of progress.
        /// </para>
        /// </param>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 21.07" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="uncompressedOutStream"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="compressedInStream"/> does not support reading, or <paramref name="uncompressedOutStream"/> does not support writing.</exception>
        public void Code(Stream compressedInStream, Stream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (!compressedInStream.CanRead)
                throw new ArgumentException("The specified stream does not support reading.", nameof(compressedInStream));
            if (uncompressedOutStream is null)
                throw new ArgumentNullException(nameof(uncompressedOutStream));
            if (!uncompressedOutStream.CanWrite)
                throw new ArgumentException("The specified stream does not support writing.", nameof(uncompressedOutStream));

            _compressCoder.Code(
                compressedInStream.GetStreamReader(),
                uncompressedOutStream.GetStreamWriter(),
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress.GetProgressReporter());
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with LZMA, and write the uncompressed data to the output stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set a stream to read the compressed input data.
        /// </param>
        /// <param name="uncompressedOutStream">
        /// Set a stream to write the uncompressed input data.
        /// </param>
        /// <param name="compressedInStreamSize">
        /// <para>
        /// Set the length in bytes of the compressed data.
        /// </para>
        /// <para>
        /// Set null if the length of the data is unknown.
        /// </para>
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data.
        /// </para>
        /// <para>
        /// Set null if the length of the data is unknown.
        /// </para>
        /// </param>
        /// <param name="progress">
        /// <para>
        /// Set an object to receive notification of coding progress.
        /// </para>
        /// <para>
        /// Set to null if you do not need to be notified of progress.
        /// </para>
        /// </param>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>This override is provided in case you do not want to use <see cref="Stream"/> class for the I/O stream,
        /// and you must have an implementation of the <see cref="ISequentialInStream"/> and <see cref="ISequentialOutStream"/> interfaces in advance.</description></item>
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 21.07" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="uncompressedOutStream"/> is null.</exception>
        public void Code(ISequentialInStream compressedInStream, ISequentialOutStream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (uncompressedOutStream is null)
                throw new ArgumentNullException(nameof(uncompressedOutStream));

            _compressCoder.Code(
                compressedInStream.GetStreamReader(),
                uncompressedOutStream.GetStreamWriter(),
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress.GetProgressReporter());
        }

        /// <summary>
        /// Explicitly release the resource associated with this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources associated with the object.
        /// </summary>
        /// <param name="disposing">
        /// Set true when calling explicitly from <see cref="IDisposable.Dispose"/>.
        /// Set to false when calling implicitly from the garbage collector.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    (_compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                    (_compressCoder as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
