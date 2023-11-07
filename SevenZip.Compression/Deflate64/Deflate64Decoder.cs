using SevenZip.IO;
using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression.Deflate64
{
    /// <summary>
    /// A class of Deflate64 decoders.
    /// </summary>
    public class Deflate64Decoder
        : IDisposable
    {
        private readonly ICompressCoder _compressCoder;
        private readonly ICompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;
        private readonly ICompressReadUnusedFromInBuf _compressReadUnusedFromInBuf;

        private bool _isDisposed;

        private Deflate64Decoder(
            ICompressCoder compressCoder,
            ICompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            ICompressReadUnusedFromInBuf compressReadUnusedFromInBuf)
        {
            _isDisposed = false;
            _compressCoder = compressCoder;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _compressReadUnusedFromInBuf = compressReadUnusedFromInBuf;
        }

        /// <summary>
        /// Create an instance of <see cref="Deflate64Decoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the Deflate64 decoder.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Deflate64Decoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static Deflate64Decoder Create(Deflate64DecoderProperties properties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            ICompressCoder? compressCoder = null;
            ICompressSetFinishMode? compressSetFinishMode = null;
            ICompressGetInStreamProcessedSize? compressGetInStreamProcessedSize = null;
            ICompressReadUnusedFromInBuf? compressReadUnusedFromInBuf = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("Deflate64", CoderType.Decoder);
                compressGetInStreamProcessedSize = (ICompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(ICompressGetInStreamProcessedSize));
                compressReadUnusedFromInBuf = (ICompressReadUnusedFromInBuf)compressCoder.QueryInterface(typeof(ICompressReadUnusedFromInBuf));
                if (properties.FinishMode.HasValue)
                {
                    compressSetFinishMode = (ICompressSetFinishMode)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }
                var decoder =
                    new Deflate64Decoder(
                        compressCoder,
                        compressGetInStreamProcessedSize,
                        compressReadUnusedFromInBuf);
                success = true;
                return decoder;
            }
            finally
            {
                if (!success)
                {
                    (compressReadUnusedFromInBuf as IDisposable)?.Dispose();
                    (compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                    (compressCoder as IDisposable)?.Dispose();
                }
                (compressSetFinishMode as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with Deflate64, and write the uncompressed data to the output stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set a stream to read the compressed input data.
        /// </param>
        /// <param name="uncompressedOutStream">
        /// Set a stream to write the uncompressed input data.
        /// </param>
        /// <param name="compressedInStreamSize">
        /// <b>The value of this parameter is ignored.</b>
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
        /// Read the compressed data from the input stream, decode it with Deflate64, and write the uncompressed data to the output stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set a stream to read the compressed input data.
        /// </param>
        /// <param name="uncompressedOutStream">
        /// Set a stream to write the uncompressed input data.
        /// </param>
        /// <param name="compressedInStreamSize">
        /// <b>The value of this parameter is ignored.</b>
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
        /// Reads the remaining data after processing Deflate64Decoder.Code() from the input stream.
        /// </summary>
        /// <param name="data">
        /// A buffer for storing the data to be read.
        /// </param>
        /// <returns>
        /// The length in bytes of the data actually read.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        public Int32 ReadUnusedFromInBuf(Span<Byte> data)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            return checked((Int32)_compressReadUnusedFromInBuf.ReadUnusedFromInBuf(data));
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
                    (_compressReadUnusedFromInBuf as IDisposable)?.Dispose();
                    (_compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                    (_compressCoder as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
