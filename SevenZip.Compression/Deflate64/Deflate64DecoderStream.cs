using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeInterface.IO;
using System;
using System.IO;

namespace SevenZip.Compression.Deflate64
{
    /// <summary>
    /// A class of Deflate64 decoders in virtual stream format that can be read sequentially.
    /// </summary>
    public class Deflate64DecoderStream
        : IO.DecoderStream
    {
        private readonly ICompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;
        private readonly ICompressReadUnusedFromInBuf _compressReadUnusedFromInBuf;

        bool _isDisposed;

        private Deflate64DecoderStream(
            ISequentialInStream sequentialInStream,
            ICompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            ICompressReadUnusedFromInBuf compressReadUnusedFromInBuf)
            : base(sequentialInStream)
        {
            _isDisposed = false;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _compressReadUnusedFromInBuf = compressReadUnusedFromInBuf;
        }

        /// <summary>
        /// Create an instance of <see cref="Deflate64DecoderStream"/>.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a container object with properties that specify the behavior of the Deflate64 decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <returns>
        /// The created <see cref="Deflate64DecoderStream"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        public static Deflate64DecoderStream Create(IO.ISequentialInStream compressedInStream, Deflate64DecoderProperties properties, UInt64? uncompressedOutStreamSize)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            return Create(properties, compressedInStream.GetStreamReader(), uncompressedOutStreamSize);
        }

        /// <summary>
        /// Create an instance of <see cref="Deflate64DecoderStream"/>.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a container object with properties that specify the behavior of the Deflate64 decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <returns>
        /// The created <see cref="Deflate64Decoder"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="compressedInStream"/> does not support reading.</exception>
        public static Deflate64DecoderStream Create(Stream compressedInStream, Deflate64DecoderProperties properties, UInt64? uncompressedOutStreamSize)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (!compressedInStream.CanRead)
                throw new ArgumentException("The specified stream does not support reading.", nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            return Create(properties, compressedInStream.GetStreamReader(), uncompressedOutStreamSize);
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
        /// Releases the resources associated with the object.
        /// </summary>
        /// <param name="disposing">
        /// Set true when calling explicitly from <see cref="IDisposable.Dispose"/>.
        /// Set to false when calling implicitly from the garbage collector.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    (_compressReadUnusedFromInBuf as IDisposable)?.Dispose();
                    (_compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
            base.Dispose(disposing);
        }

        private static Deflate64DecoderStream Create(Deflate64DecoderProperties properties, SequentialInStreamReader compressedInStreamReader, UInt64? uncompressedOutStreamSize)
        {
            ICompressCoder? compressCoder = null;
            ISequentialInStream? sequentialInStream = null;
            ICompressSetInStream? compressSetInStream = null;
            ICompressSetOutStreamSize? compressSetOutStreamSize = null;
            ICompressSetFinishMode? compressSetFinishMode = null;
            ICompressGetInStreamProcessedSize? compressGetInStreamProcessedSize = null;
            ICompressReadUnusedFromInBuf? compressReadUnusedFromInBuf = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("Deflate64", CoderType.Decoder);
                sequentialInStream = (ISequentialInStream)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressSetInStream = (ICompressSetInStream)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressSetOutStreamSize = (ICompressSetOutStreamSize)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressGetInStreamProcessedSize = (ICompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(ICompressGetInStreamProcessedSize));
                compressReadUnusedFromInBuf = (ICompressReadUnusedFromInBuf)compressCoder.QueryInterface(typeof(ICompressReadUnusedFromInBuf));
                if (properties.FinishMode.HasValue)
                {
                    compressSetFinishMode = (ICompressSetFinishMode)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }
                compressSetInStream.SetInStream(compressedInStreamReader);
                compressSetOutStreamSize.SetOutStreamSize(uncompressedOutStreamSize);
                var decoder =
                    new Deflate64DecoderStream(
                        sequentialInStream,
                        compressGetInStreamProcessedSize,
                        compressReadUnusedFromInBuf);
                success = true;
                return decoder;
            }
            finally
            {
                if (!success)
                {
                    (sequentialInStream as IDisposable)?.Dispose();
                    (compressReadUnusedFromInBuf as IDisposable)?.Dispose();
                    (compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                }
                (compressSetOutStreamSize as IDisposable)?.Dispose();
                (compressSetInStream as IDisposable)?.Dispose();
                (compressSetFinishMode as IDisposable)?.Dispose();
                (compressCoder as IDisposable)?.Dispose();
            }
        }
    }
}
