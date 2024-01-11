using System;
using System.IO;
using Palmtree.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression.Bzip2
{
    /// <summary>
    /// A class of BZIP2 encoders.
    /// </summary>
    public class Bzip2Encoder
        : IDisposable, ICompressCoder
    {
        private readonly CompressCoder _compressCoder;

        private Boolean _isDisposed;

        private Bzip2Encoder(CompressCoder compressCoder)
        {
            _isDisposed = false;
            _compressCoder = compressCoder;
        }

        /// <summary>
        /// Create an instance of <see cref="Bzip2Encoder"/> with default properties.
        /// </summary>
        /// <returns>
        /// It is an instance of <see cref="Bzip2Encoder"/> created.
        /// </returns>
        public static Bzip2Encoder CreateEncoder() => CreateEncoder(new Bzip2EncoderProperties());

        /// <summary>
        /// Create an instance of <see cref="Bzip2Encoder"/>.
        /// </summary>
        /// <param name="properties">
        ///Set a container object with properties that specify the behavior of the BZip2 encoder.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Bzip2Encoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static Bzip2Encoder CreateEncoder(Bzip2EncoderProperties properties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            var compressCoder = (CompressCoder?)null;
            var compressSetCoderProperties = (CompressSetCoderProperties?)null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsCollection.Instance.CreateCompressCoder(Bzip2Constants.CODER_NAME, CoderType.Encoder);
                compressSetCoderProperties = (CompressSetCoderProperties)compressCoder.QueryInterface(typeof(CompressSetCoderProperties));
                compressSetCoderProperties.SetCoderProperties(properties);
                var encoder = new Bzip2Encoder(compressCoder);
                success = true;
                return encoder;
            }
            finally
            {
                if (!success)
                    compressCoder?.Dispose();
                compressSetCoderProperties?.Dispose();
            }
        }

        /// <summary>
        /// Reads the uncompressed data from the input stream, encodes it with BZIP2, and writes the compressed data to the output stream.
        /// </summary>
        /// <param name="uncompressedInStream">
        /// Set the input stream to read the uncompressed data.
        /// </param>
        /// <param name="compressedOutStream">
        /// Set the output stream to write the compressed data.
        /// </param>
        /// <param name="uncompressedInStreamSize">
        /// <b>The value of this parameter is ignored.</b>
        /// </param>
        /// <param name="compressedOutStreamSize">
        /// <b>The value of this parameter is ignored.</b>
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
        /// <exception cref="ObjectDisposedException">The encoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="uncompressedInStream"/> or <paramref name="compressedOutStream"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="uncompressedInStream"/> does not support reading, or <paramref name="compressedOutStream"/> does not support writing.</exception>
        public void Code(Stream uncompressedInStream, Stream compressedOutStream, UInt64? uncompressedInStreamSize, UInt64? compressedOutStreamSize, IProgress<(UInt64 inStreamProcessedCount, UInt64 outStreamProcessedCount)>? progress)
        {
            if (uncompressedInStream is null)
                throw new ArgumentNullException(nameof(uncompressedInStream));
            if (!uncompressedInStream.CanRead)
                throw new ArgumentException("The specified stream does not support reading.", nameof(uncompressedInStream));
            if (compressedOutStream is null)
                throw new ArgumentNullException(nameof(compressedOutStream));
            if (!compressedOutStream.CanWrite)
                throw new ArgumentException("The specified stream does not support writing.", nameof(compressedOutStream));

            _compressCoder.Code(
                uncompressedInStream,
                compressedOutStream,
                uncompressedInStreamSize,
                compressedOutStreamSize,
                progress);
        }

        /// <summary>
        /// Reads the uncompressed data from the input stream, encodes it with BZIP2, and writes the compressed data to the output stream.
        /// </summary>
        /// <param name="uncompressedInStream">
        /// Set the input stream to read the uncompressed data.
        /// </param>
        /// <param name="compressedOutStream">
        /// Set the output stream to write the compressed data.
        /// </param>
        /// <param name="uncompressedInStreamSize">
        /// <b>The value of this parameter is ignored.</b>
        /// </param>
        /// <param name="compressedOutStreamSize">
        /// <b>The value of this parameter is ignored.</b>
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
        /// <exception cref="ObjectDisposedException">The encoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="uncompressedInStream"/> or <paramref name="compressedOutStream"/> is null.</exception>
        public void Code(ISequentialInputByteStream uncompressedInStream, ISequentialOutputByteStream compressedOutStream, UInt64? uncompressedInStreamSize, UInt64? compressedOutStreamSize, IProgress<(UInt64 inStreamProcessedCount, UInt64 outStreamProcessedCount)>? progress)
        {
            if (uncompressedInStream is null)
                throw new ArgumentNullException(nameof(uncompressedInStream));
            if (compressedOutStream is null)
                throw new ArgumentNullException(nameof(compressedOutStream));

            _compressCoder.Code(
                uncompressedInStream,
                compressedOutStream,
                uncompressedInStreamSize,
                compressedOutStreamSize,
                progress);
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
        protected virtual void Dispose(Boolean disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                    _compressCoder.Dispose();
                _isDisposed = true;
            }
        }
    }
}
