using SevenZip.IO;
using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression.Ppmd8
{
    /// <summary>
    /// A class of PPMd version I encoders.
    /// </summary>
    public class Ppmd8Encoder
        : IDisposable
    {
        private readonly ICompressCoder _compressCoder;

        private bool _isDisposed;

        private Ppmd8Encoder(ICompressCoder compressCoder)
        {
            _isDisposed = false;
            _compressCoder = compressCoder;
        }

        /// <summary>
        /// Create an instance of <see cref="Ppmd8Encoder"/>.
        /// </summary>
        /// <param name="properties">
        ///Set a container object with properties that specify the behavior of the PPMd version I encoder.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Ppmd8Encoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static Ppmd8Encoder Create(Ppmd8EncoderProperties properties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            ICompressCoder? compressCoder = null;
            ICompressSetCoderProperties? compressSetCoderProperties = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("PPMDZip", CoderType.Encoder);
                compressSetCoderProperties = (ICompressSetCoderProperties)compressCoder.QueryInterface(typeof(ICompressSetCoderProperties));
                compressSetCoderProperties.SetCoderProperties(properties);
                var encoder = new Ppmd8Encoder(compressCoder);
                success = true;
                return encoder;
            }
            finally
            {
                if (!success)
                {
                    (compressCoder as IDisposable)?.Dispose();
                }
                (compressSetCoderProperties as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Reads the uncompressed data from the input stream, encodes it with PPMd version I, and writes the compressed data to the output stream.
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
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 21.07" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The encoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="uncompressedInStream"/> or <paramref name="compressedOutStream"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="uncompressedInStream"/> does not support reading, or <paramref name="compressedOutStream"/> does not support writing.</exception>
        public void Code(Stream uncompressedInStream, Stream compressedOutStream, UInt64? uncompressedInStreamSize, UInt64? compressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
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
                uncompressedInStream.GetStreamReader(),
                compressedOutStream.GetStreamWriter(),
                uncompressedInStreamSize,
                compressedOutStreamSize,
                progress.GetProgressReporter());
        }

        /// <summary>
        /// Reads the uncompressed data from the input stream, encodes it with PPMd version I, and writes the compressed data to the output stream.
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
        /// and you must have an implementation of the <see cref="ISequentialInStream"/> and <see cref="ISequentialOutStream"/> interfaces in advance.</description></item>
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 21.07" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The encoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="uncompressedInStream"/> or <paramref name="compressedOutStream"/> is null.</exception>
        public void Code(ISequentialInStream uncompressedInStream, ISequentialOutStream compressedOutStream, UInt64? uncompressedInStreamSize, UInt64? compressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (uncompressedInStream is null)
                throw new ArgumentNullException(nameof(uncompressedInStream));
            if (compressedOutStream is null)
                throw new ArgumentNullException(nameof(compressedOutStream));

            _compressCoder.Code(
                uncompressedInStream.GetStreamReader(),
                compressedOutStream.GetStreamWriter(),
                uncompressedInStreamSize,
                compressedOutStreamSize,
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
                    (_compressCoder as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
