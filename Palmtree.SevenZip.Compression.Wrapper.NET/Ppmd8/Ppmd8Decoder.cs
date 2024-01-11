#if IS_SUPPORTED_SEVENZIP_PPMD8
using System;
using System.IO;
using Palmtree.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression.Ppmd8
{
    /// <summary>
    /// A class of PPMd8 (PPMd version I) decoders.
    /// </summary>
    public class Ppmd8Decoder
        : IDisposable
    {
        private readonly CompressCoder _compressCoder;
        private readonly CompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;

        private Boolean _isDisposed;

        private Ppmd8Decoder(
            CompressCoder compressCoder,
            CompressGetInStreamProcessedSize compressGetInStreamProcessedSize)
        {
            _isDisposed = false;
            _compressCoder = compressCoder;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
        }

        /// <summary>
        /// Create an instance of <see cref="Ppmd8Decoder"/> with default properties.
        /// </summary>
        /// <returns>
        /// It is an instance of <see cref="Ppmd8Decoder"/> created.
        /// </returns>
        public static Ppmd8Decoder CreateDecoder() => CreateDecoder(new Ppmd8DecoderProperties());

        /// <summary>
        /// Create an instance of <see cref="Ppmd8Decoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the PPMd8 decoder.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Ppmd8Decoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static Ppmd8Decoder CreateDecoder(Ppmd8DecoderProperties properties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            var compressCoder = (CompressCoder?)null;
            var compressSetFinishMode = (CompressSetFinishMode?)null;
            var compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize?)null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsCollection.Instance.CreateCompressCoder(Ppmd8Constants.CODER_NAME, CoderType.Decoder);
                if (properties.FinishMode is not null)
                {
                    compressSetFinishMode = (CompressSetFinishMode)compressCoder.QueryInterface(typeof(CompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }

                compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(CompressGetInStreamProcessedSize));
                var decoder =
                    new Ppmd8Decoder(
                        compressCoder,
                        compressGetInStreamProcessedSize);
                success = true;
                return decoder;
            }
            finally
            {
                if (!success)
                {
                    compressGetInStreamProcessedSize?.Dispose();
                    compressCoder?.Dispose();
                }

                compressSetFinishMode?.Dispose();
            }
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with PPMd8, and write the uncompressed data to the output stream.
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
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 23.01" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="uncompressedOutStream"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="compressedInStream"/> does not support reading, or <paramref name="uncompressedOutStream"/> does not support writing.</exception>
        public void Code(Stream compressedInStream, Stream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64 inStreamProcessedCount, UInt64 outStreamProcessedCount)>? progress)
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
                compressedInStream,
                uncompressedOutStream,
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress);
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with PPMd8, and write the uncompressed data to the output stream.
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
        /// and you must have an implementation of the <see cref="ISequentialInputByteStream"/> and <see cref="ISequentialOutputByteStream"/> interfaces in advance.</description></item>
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 23.01" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="uncompressedOutStream"/> is null.</exception>
        public void Code(ISequentialInputByteStream compressedInStream, ISequentialOutputByteStream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64 inStreamProcessedCount, UInt64 outStreamProcessedCount)>? progress)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (uncompressedOutStream is null)
                throw new ArgumentNullException(nameof(uncompressedOutStream));

            _compressCoder.Code(
                compressedInStream,
                uncompressedOutStream,
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress);
        }

        /// <summary>
        /// The number of bytes of processed data in the decoder's input stream.
        /// </summary>
        public UInt64 InStreamProcessedSize => _compressGetInStreamProcessedSize.InStreamProcessedSize;

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
        protected virtual void Dispose(Boolean disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _compressGetInStreamProcessedSize.Dispose();
                    _compressCoder.Dispose();
                }

                _isDisposed = true;
            }
        }
    }
}
#endif
