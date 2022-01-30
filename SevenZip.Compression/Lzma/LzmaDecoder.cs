using SevenZip.IO;
using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression.Lzma
{
    /// <summary>
    /// A class of LZMA decoders.
    /// </summary>
    public class LzmaDecoder
        : CompressCoder
    {
        /// <summary>
        /// The length of the property embedded in the compressed stream in LZMA format.
        /// </summary>
        public const Int32 LZMA_CONTENT_PROPERTY_SIZE = 5;
        
        private readonly ICompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;
        private ICompressSetDecoderProperties2 _compressSetDecoderProperties2;
        private CoderHeaderParser _lzmaHeaderParser;

        private bool _isDisposed;

        private LzmaDecoder(
            ICompressCoder compressCoder,
            ICompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            ICompressSetDecoderProperties2 compressSetDecoderProperties2,
            CoderHeaderParser lzmaHeaderParser)
            : base(compressCoder)
        {
            _isDisposed = false;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _compressSetDecoderProperties2 = compressSetDecoderProperties2;
            _lzmaHeaderParser = lzmaHeaderParser;
        }

        /// <summary>
        /// Create an instance of <see cref="LzmaDecoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the LZMA decoder.
        /// </param>
        /// <param name="lzmaHeaderParser">
        /// A delegate for the function that reads the LZMA header from the input stream.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="LzmaDecoder"/> created.
        /// </returns>
        /// <example>
        /// The delegate set in <paramref name="lzmaHeaderParser"/> depends on the application.
        /// For example, if you want to decode the data in the format described in lzma.txt included in the LZMA SDK, write as follows:
        /// <code>
        /// var coder =
        ///     LzmaDecoder.Create(
        ///         new LzmaDecoderProperties { FinishMode = true },
        ///         reader =>
        ///         {
        ///             reader.ReadProperty(); // Read the 5-byte property and set it in the decoder.
        ///             var length = reader.ReadUInt64LE() // Read the length of uncompressed data
        ///             reader.SetOutStreamSize(length); // Sets the length of the output stream to write the uncompressed data
        ///             return LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE + sizeof(UInt64);
        ///         });
        /// </code>
        /// If you want to uncompress the contents of a ZIP file compressed in LZMA format, write as follows:
        /// <code>
        /// var coder =
        ///     LzmaDecoder.Create(
        ///         new LzmaDecoderProperties { FinishMode = true },
        ///         reader =>
        ///         {
        ///             reader.ReadByte(); // Reads the major version of the encoded application. (This value is ignored.)
        ///             reader.ReadByte(); // Reads the minor version of the encoded application. (This value is ignored.)
        ///             var length = reader.ReadUInt16LE(); // Read the length of the property. (This value should always match LZMA_CONTENT_PROPERTY_SIZE)
        ///             if (lengtth != LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE)
        ///                 throw new Exception("Bad data format.");
        ///             reader.ReadProperty(); // Read the 5-byte property and set it in the decoder.
        ///             return sizeof(Byte) + sizeof(Byte) + sizeof(UInt16 + LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE;
        ///         });
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="properties"/> is null.
        /// </exception>
        public static LzmaDecoder Create(LzmaDecoderProperties properties, CoderHeaderParser lzmaHeaderParser)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (lzmaHeaderParser is null)
                throw new ArgumentNullException(nameof(lzmaHeaderParser));

            ICompressCoder? compressCoder = null;
            ICompressGetInStreamProcessedSize? compressGetInStreamProcessedSize = null;
            ICompressSetDecoderProperties2? compressSetDecoderProperties2 = null;
            ICompressSetFinishMode? compressSetFinishMode = null;
            ICompressSetBufSize? compressSetBufSize = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("LZMA", CoderType.Decoder);
                compressGetInStreamProcessedSize = (ICompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(ICompressGetInStreamProcessedSize));
                compressSetDecoderProperties2 = (ICompressSetDecoderProperties2)compressCoder.QueryInterface(typeof(ICompressSetDecoderProperties2));
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
                var coder =
                    new LzmaDecoder(
                        compressCoder,
                        compressGetInStreamProcessedSize,
                        compressSetDecoderProperties2,
                        lzmaHeaderParser);
                success = true;
                return coder;
            }
            finally
            {
                if (!success)
                {
                    (compressSetDecoderProperties2 as IDisposable)?.Dispose();
                    (compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                    (compressCoder as IDisposable)?.Dispose();
                }
                (compressSetFinishMode as IDisposable)?.Dispose();
                (compressSetBufSize as IDisposable)?.Dispose();
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
            Code(
                compressedInStream.AsISequentialInStream(),
                uncompressedOutStream.AsISequentialOutStream(),
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress);
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
            var lzmaDecoderHeaderReader =
                CoderHeaderReader.Create(
                    compressedInStream.GetStreamReader(),
                    () =>
                    {
                        Span<Byte> propertiesBuffer = stackalloc Byte[LZMA_CONTENT_PROPERTY_SIZE];
                        compressedInStream.ReadBytes(propertiesBuffer);
                        _compressSetDecoderProperties2.SetDecoderProperties2(propertiesBuffer);
                    });
            var headerLength = _lzmaHeaderParser(lzmaDecoderHeaderReader);
            if (compressedInStreamSize is null)
                compressedInStreamSize = lzmaDecoderHeaderReader.InStreamSize;
            else if (lzmaDecoderHeaderReader.InStreamSize is null)
                compressedInStreamSize = compressedInStreamSize.Value - headerLength;
            else
                compressedInStreamSize = (compressedInStreamSize.Value - headerLength).Minimum(lzmaDecoderHeaderReader.InStreamSize.Value);
            if (uncompressedOutStreamSize is null)
                uncompressedOutStreamSize = lzmaDecoderHeaderReader.OutStreamSize;
            else if (lzmaDecoderHeaderReader.OutStreamSize is null)
            {
                // NOP
            }
            else
                uncompressedOutStreamSize = uncompressedOutStreamSize.Value.Minimum(lzmaDecoderHeaderReader.OutStreamSize.Value);
            base.Code(
                compressedInStream,
                uncompressedOutStream,
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress);
        }

        /// <summary>
        /// The number of bytes of processed data in the coder's input stream.
        /// </summary>
        public UInt64 InStreamProcessedSize => _compressGetInStreamProcessedSize.InStreamProcessedSize;

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
                    (_compressSetDecoderProperties2 as IDisposable)?.Dispose();
                    (_compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
