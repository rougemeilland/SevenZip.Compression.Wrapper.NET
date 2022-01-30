using SevenZip.IO;
using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression.Deflate
{
    /// <summary>
    /// A class of Deflate decoders.
    /// </summary>
    public class DeflateDecoder
        : CompressCoder
    {
        private readonly ICompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;
        private readonly ICompressReadUnusedFromInBuf _compressReadUnusedFromInBuf;

        private bool _isDisposed;

        private DeflateDecoder(
            ICompressCoder compressCoder,
            ICompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            ICompressReadUnusedFromInBuf compressReadUnusedFromInBuf)
            : base(compressCoder)
        {
            _isDisposed = false;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _compressReadUnusedFromInBuf = compressReadUnusedFromInBuf;
        }

        /// <summary>
        /// Create an instance of <see cref="DeflateDecoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the Deflate decoder.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="DeflateDecoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="properties"/> is null.
        /// </exception>
        public static DeflateDecoder Create(DeflateDecoderProperties properties)
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
                compressCoder = CompressCodecsInfo.CreateCompressCoder("Deflate", CoderType.Decoder);
                compressGetInStreamProcessedSize = (ICompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(ICompressGetInStreamProcessedSize));
                compressReadUnusedFromInBuf = (ICompressReadUnusedFromInBuf)compressCoder.QueryInterface(typeof(ICompressReadUnusedFromInBuf));
                if (properties.FinishMode.HasValue)
                {
                    compressSetFinishMode = (ICompressSetFinishMode)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }
                var coder =
                    new DeflateDecoder(
                        compressCoder,
                        compressGetInStreamProcessedSize,
                        compressReadUnusedFromInBuf);
                success = true;
                return coder;
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
        /// Read the compressed data from the input stream, decode it with Deflate, and write the uncompressed data to the output stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set a stream to read the compressed input data.
        /// </param>
        /// <param name="uncompressedOutStream">
        /// Set a stream to write the uncompressed input data.
        /// </param>
        /// <param name="compressedInStreamSize">
        /// This parameter is ignored.
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
        /// This object must implement <see cref="IProgress{T}">IProgress&lt;(<see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> inStreamProcessedCount, <see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> outStreamProcessedCount)&gt;</see>.
        /// </para>
        /// <para>
        /// Set to null if you do not need to be notified of progress.
        /// </para>
        /// </param>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public override void Code(Stream compressedInStream, Stream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            base.Code(compressedInStream, uncompressedOutStream, compressedInStreamSize, uncompressedOutStreamSize, progress);
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with Deflate, and write the uncompressed data to the output stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set a stream to read the compressed input data.
        /// </param>
        /// <param name="uncompressedOutStream">
        /// Set a stream to write the uncompressed input data.
        /// </param>
        /// <param name="compressedInStreamSize">
        /// This parameter is ignored.
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
        /// This object must implement <see cref="IProgress{T}">IProgress&lt;(<see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> inStreamProcessedCount, <see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> outStreamProcessedCount)&gt;</see>.
        /// </para>
        /// <para>
        /// Set to null if you do not need to be notified of progress.
        /// </para>
        /// </param>
        /// <remarks>
        /// <para>
        /// This override is provided in case you do not want to use <see cref="Stream"/> class for the I/O stream,
        /// and you must have an implementation of the <see cref="ISequentialInStream"/> and <see cref="ISequentialOutStream"/> interfaces in advance.
        /// </para>
        /// <para>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </para>
        /// </remarks>
        public override void Code(ISequentialInStream compressedInStream, ISequentialOutStream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            base.Code(compressedInStream, uncompressedOutStream, compressedInStreamSize, uncompressedOutStreamSize, progress);
        }

        /// <summary>
        /// The number of bytes of processed data in the coder's input stream.
        /// </summary>
        public UInt64 InStreamProcessedSize => _compressGetInStreamProcessedSize.InStreamProcessedSize;

        /// <summary>
        /// Reads the remaining data after processing DeflateDecoder.Code() from the input stream.
        /// </summary>
        /// <param name="data">
        /// A buffer for storing the data to be read.
        /// </param>
        /// <returns>
        /// The length in bytes of the data actually read.
        /// </returns>
        public Int32 ReadUnusedFromInBuf(Span<Byte> data)
        {
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
    }
}
