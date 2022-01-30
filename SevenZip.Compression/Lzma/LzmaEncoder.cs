using SevenZip.IO;
using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression.Lzma
{
    /// <summary>
    /// A class of LZMA encoders.
    /// </summary>
    public class LzmaEncoder
        : CompressCoder
    {
        /// <summary>
        /// The length of the property to embed in the compressed stream in LZMA format.
        /// </summary>
        public const Int32 LZMA_CONTENT_PROPERTY_SIZE = 5;

        private bool _isDisposed;
        private ICompressWriteCoderProperties _compressWriteCoderProperties;
        private IO.CorderHeaderFormatter _lzmaHeaderFormatter;

        private LzmaEncoder(ICompressCoder compressCoder, ICompressWriteCoderProperties compressWriteCoderProperties, IO.CorderHeaderFormatter lzmaHeaderFormatter)
            : base(compressCoder)
        {
            _isDisposed = false;
            _compressWriteCoderProperties = compressWriteCoderProperties;
            _lzmaHeaderFormatter = lzmaHeaderFormatter;
        }

        /// <summary>
        /// Create an instance of <see cref="LzmaEncoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a container object with properties that specify the behavior of the LZMA encoder.
        /// </param>
        /// <param name="lzmaHeaderFormatter">
        /// A delegate for the function that writes the LZMA header to the output stream.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="LzmaEncoder"/> created.
        /// </returns>
        /// <example>
        /// The delegate set in <paramref name="lzmaHeaderFormatter"/> depends on the application.
        /// For example, if you want to encode the data in the format described in lzma.txt included in the LZMA SDK, write as follows:
        /// <code>
        /// var coder =
        ///     LzmaEncoder.Create(
        ///         new LzmaEncoderProperties { Level = CompressionLevel.Normal },
        ///         writer =>
        ///         {
        ///             writer.WriteProperty(); // Write the 5-byte property set in the encoder to the output stream
        ///             writer.WriteUInt64LE(uncompressedStreamSize) // Write the length of the uncompressed data
        ///             writer.SetInStreamSize(uncompressedStreamSize); // Sets the length of the input stream to read the uncompressed data.
        ///             return LzmaEncoder.LZMA_CONTENT_PROPERTY_SIZE + sizeof(UInt64);
        ///         });
        /// </code>
        /// If you want to uncompress the contents of a ZIP file compressed in LZMA format, write as follows:
        /// <code>
        /// var coder =
        ///     LzmaEncoder.Create(
        ///         new LzmaEncoderProperties { Level = CompressionLevel.Normal },
        ///         writer =>
        ///         {
        ///             writer.WriteByte(MAJOR_VERSION); // Write a major version of the encoder application. (For example, in the case of "7-zip 21.07", the integer 21 is written.)
        ///             writer.WriteByte(MINOR_VERSION); // Write a minor version of the encoder application. (For example, in the case of "7-zip 21.07", the integer 7 is written.)
        ///             writer.WriteUInt16LE(LzmaEncoder.LZMA_CONTENT_PROPERTY_SIZE); // Write the length of the encoder property to write next.
        ///             writer.WriteProperty(); // Write the properties of the 5-byte encoder.
        ///             return sizeof(Byte) + sizeof(Byte) + sizeof(UInt16 + LzmaEncoder.LZMA_CONTENT_PROPERTY_SIZE;
        ///         });
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="properties"/> is null.
        /// </exception>
        public static LzmaEncoder Create(LzmaEncoderProperties properties, IO.CorderHeaderFormatter lzmaHeaderFormatter)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            ICompressCoder? compressCoder = null;
            ICompressSetCoderProperties? compressSetCoderProperties = null;
            ICompressSetCoderPropertiesOpt? compressSetCoderPropertiesOpt = null;
            ICompressWriteCoderProperties? compressWriteCoderProperties = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("LZMA", CoderType.Encoder);
                compressSetCoderProperties = (ICompressSetCoderProperties)compressCoder.QueryInterface(typeof(ICompressSetCoderProperties));
                compressSetCoderPropertiesOpt = (ICompressSetCoderPropertiesOpt)compressCoder.QueryInterface(typeof(ICompressSetCoderPropertiesOpt));
                compressWriteCoderProperties = (ICompressWriteCoderProperties)compressCoder.QueryInterface(typeof(ICompressWriteCoderProperties));
                compressSetCoderProperties.SetCoderProperties(properties);
                compressSetCoderPropertiesOpt.SetCoderPropertiesOpt(properties);
                var coder = new LzmaEncoder(compressCoder, compressWriteCoderProperties, lzmaHeaderFormatter);
                success = true;
                return coder;
            }
            finally
            {
                if (!success)
                {
                    (compressCoder as IDisposable)?.Dispose();
                }
                (compressSetCoderPropertiesOpt as IDisposable)?.Dispose();
                (compressSetCoderProperties as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Reads the uncompressed data from the input stream, encodes it with LZMA, and writes the compressed data to the output stream.
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
            Code(
                uncompressedInStream.AsISequentialInStream(),
                compressedOutStream.AsISequentialOutStream(),
                uncompressedInStreamSize,
                compressedOutStreamSize,
                progress);
        }

        /// <summary>
        /// Reads the uncompressed data from the input stream, encodes it with LZMA, and writes the compressed data to the output stream.
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
            var lzmaDecoderHeaderWriter =
                CoderHeaderWriter.Create(
                    compressedOutStream,
                    () =>_compressWriteCoderProperties.WriteCoderProperties(compressedOutStream.GetStreamWriter()));
            var headerLength = _lzmaHeaderFormatter(lzmaDecoderHeaderWriter);
            if (compressedOutStreamSize is null)
                compressedOutStreamSize = lzmaDecoderHeaderWriter.OutStreamSize;
            else if (lzmaDecoderHeaderWriter.OutStreamSize is null)
                compressedOutStreamSize = compressedOutStreamSize.Value - headerLength;
            else
                compressedOutStreamSize = (compressedOutStreamSize.Value - headerLength).Minimum(lzmaDecoderHeaderWriter.OutStreamSize.Value);
            if (uncompressedInStreamSize is null)
                uncompressedInStreamSize = lzmaDecoderHeaderWriter.InStreamSize;
            else if (lzmaDecoderHeaderWriter.InStreamSize is null)
            {
                // NOP
            }
            else
                uncompressedInStreamSize = uncompressedInStreamSize.Value.Minimum(lzmaDecoderHeaderWriter.InStreamSize.Value);
            base.Code(uncompressedInStream, compressedOutStream, uncompressedInStreamSize, compressedOutStreamSize, progress);
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
                    (_compressWriteCoderProperties as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
