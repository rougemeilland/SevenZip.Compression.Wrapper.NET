using SevenZip.IO;
using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression.Deflate64
{
    /// <summary>
    /// A class of Deflate64 encoders.
    /// </summary>
    public class Deflate64Encoder
        : CompressCoder
    {
        private Deflate64Encoder(ICompressCoder compressCoder)
            : base(compressCoder)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="Deflate64Encoder"/>.
        /// </summary>
        /// <param name="properties">
        ///Set a container object with properties that specify the behavior of the Deflate64 encoder.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Deflate64Encoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="properties"/> is null.
        /// </exception>
        public static Deflate64Encoder Create(Deflate64EncoderProperties properties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            ICompressCoder? compressCoder = null;
            ICompressSetCoderProperties? compressSetCoderProperties = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("Deflate64", CoderType.Encoder);
                compressSetCoderProperties = (ICompressSetCoderProperties)compressCoder.QueryInterface(typeof(ICompressSetCoderProperties));
                compressSetCoderProperties.SetCoderProperties(properties);
                var coder = new Deflate64Encoder(compressCoder);
                success = true;
                return coder;
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
        /// Reads the uncompressed data from the input stream, encodes it with Deflate64, and writes the compressed data to the output stream.
        /// </summary>
        /// <param name="uncompressedInStream">
        /// Set the input stream to read the uncompressed data.
        /// </param>
        /// <param name="compressedOutStream">
        /// Set the output stream to write the compressed data.
        /// </param>
        /// <param name="uncompressedInStreamSize">
        /// This parameter is ignored.
        /// </param>
        /// <param name="compressedOutStreamSize">
        /// This parameter is ignored.
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
        public override void Code(Stream uncompressedInStream, Stream compressedOutStream, UInt64? uncompressedInStreamSize, UInt64? compressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            base.Code(uncompressedInStream, compressedOutStream, uncompressedInStreamSize, compressedOutStreamSize, progress);
        }

        /// <summary>
        /// Reads the uncompressed data from the input stream, encodes it with Deflate64, and writes the compressed data to the output stream.
        /// </summary>
        /// <param name="uncompressedInStream">
        /// Set the input stream to read the uncompressed data.
        /// </param>
        /// <param name="compressedOutStream">
        /// Set the output stream to write the compressed data.
        /// </param>
        /// <param name="uncompressedInStreamSize">
        /// This parameter is ignored.
        /// </param>
        /// <param name="compressedOutStreamSize">
        /// This parameter is ignored.
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
        public override void Code(ISequentialInStream uncompressedInStream, ISequentialOutStream compressedOutStream, UInt64? uncompressedInStreamSize, UInt64? compressedOutStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            base.Code(uncompressedInStream, compressedOutStream, uncompressedInStreamSize, compressedOutStreamSize, progress);
        }
    }
}
