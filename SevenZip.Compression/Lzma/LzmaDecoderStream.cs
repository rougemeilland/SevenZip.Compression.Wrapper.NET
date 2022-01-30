using SevenZip.NativeInterface;
using System.Runtime.InteropServices;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeInterface.IO;
using System;
using System.IO;

namespace SevenZip.Compression.Lzma
{
    /// <summary>
    /// A class of LZMA decoders in virtual stream format that can be read sequentially.
    /// </summary>
    public class LzmaDecoderStream
        : CompressCoderInStream
    {
        /// <summary>
        /// The length of the property embedded in the compressed stream in LZMA format.
        /// </summary>
        public const Int32 LZMA_CONTENT_PROPERTY_SIZE = 5;

        private readonly ICompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;

        private bool _isDisposed;
        private bool _isFirst;
        private ICompressSetOutStreamSize _compressSetOutStreamSize;
        private IO.CoderHeaderParser _lzmaHeaderParser;
        private IO.CoderHeaderReader _lzmaHeaderReader;
        private UInt64? _uncompressedOutStreamSize;

        private LzmaDecoderStream(
            ISequentialInStream sequentialInStream,
            ICompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            ICompressSetOutStreamSize compressSetOutStreamSize,
            IO.CoderHeaderParser lzmaHeaderParser,
            IO.CoderHeaderReader lzmaHeaderReader,
            UInt64? uncompressedOutStreamSize)
            : base(sequentialInStream)
        {
            _isDisposed = false;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _compressSetOutStreamSize = compressSetOutStreamSize;
            _lzmaHeaderParser = lzmaHeaderParser;
            _lzmaHeaderReader = lzmaHeaderReader;
            _uncompressedOutStreamSize = uncompressedOutStreamSize;
            _isFirst = true;
        }

        /// <summary>
        /// Create an instance of <see cref="LzmaDecoderStream"/>.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the LZMA decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data (including the header parsed by <paramref name="lzmaHeaderParser"/>) read from <paramref name="compressedInStream"/>.
        /// </para>
        /// <para>
        /// Set null if the length of the uncompressed data is unknown.
        /// </para>
        /// </param>
        /// <param name="lzmaHeaderParser">
        /// A delegate for a function that reads the header of a compressed LZMA input stream.
        /// </param>
        /// <returns>
        /// The created <see cref="LzmaDecoderStream"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="properties"/> is null.
        /// </exception>
        /// <example>
        /// The delegate set in <paramref name="lzmaHeaderParser"/> depends on the application.
        /// For example, if you want to decode the data in the format described in lzma.txt included in the LZMA SDK, write as follows:
        /// <code>
        /// var coder =
        ///     LzmaDecoderStream.Create(
        ///         new LzmaDecoderProperties { FinishMode = true },
        ///         reader =>
        ///         {
        ///             reader.ReadProperty(); // Read the 5-byte property and set it in the decoder.
        ///             var length = reader.ReadUInt64LE() // Read the length of uncompressed data
        ///             reader.SetOutStreamSize(length); // Sets the length of the output stream to write the uncompressed data
        ///             return LzmaDecoderStream.LZMA_CONTENT_PROPERTY_SIZE + sizeof(UInt64);
        ///         });
        /// </code>
        /// If you want to uncompress the contents of a ZIP file compressed in LZMA format, write as follows:
        /// <code>
        /// var coder =
        ///     LzmaDecoderStream.Create(
        ///         new LzmaDecoderProperties { FinishMode = true },
        ///         reader =>
        ///         {
        ///             reader.ReadByte(); // Read the major version of 7-zip. (This value is ignored)
        ///             reader.ReadByte(); // Read the minor version of 7-zip. (This value is ignored)
        ///             var length = reader.ReadUInt16LE(); // Read the length of the property. (This value should always match LZMA_CONTENT_PROPERTY_SIZE)
        ///             if (length != LzmaDecoderStream.LZMA_CONTENT_PROPERTY_SIZE)
        ///                 throw new Exception("Bad data format.");
        ///             reader.ReadProperty(); // Read the 5-byte property and set it in the decoder.
        ///             return sizeof(Byte) + sizeof(Byte) + sizeof(UInt16) + LzmaDecoderStream.LZMA_CONTENT_PROPERTY_SIZE;
        ///         });
        /// </code>
        /// </example>
        public static Stream Create(Stream compressedInStream, LzmaDecoderProperties properties, UInt64? uncompressedOutStreamSize, IO.CoderHeaderParser lzmaHeaderParser)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            return Create(compressedInStream.AsISequentialInStream(), properties, uncompressedOutStreamSize, lzmaHeaderParser).AsStream();
        }

        /// <summary>
        /// Create an instance of <see cref="LzmaDecoderStream"/>.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the LZMA decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data (including the header parsed by <paramref name="lzmaHeaderParser"/>) read from <paramref name="compressedInStream"/>.
        /// </para>
        /// <para>
        /// Set null if the length of the uncompressed data is unknown.
        /// </para>
        /// </param>
        /// <param name="lzmaHeaderParser">
        /// A delegate for the function that reads the LZMA header from the input stream.
        /// </param>
        /// <returns>
        /// The created <see cref="LzmaDecoderStream"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="properties"/> is null.
        /// </exception>
        /// <example>
        /// The delegate set in <paramref name="lzmaHeaderParser"/> depends on the application.
        /// For example, if you want to decode the data in the format described in lzma.txt included in the LZMA SDK, write as follows:
        /// <code>
        /// var coder =
        ///     LzmaDecoderStream.Create(
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
        ///     LzmaDecoderStream.Create(
        ///         new LzmaDecoderProperties { FinishMode = true },
        ///         reader =>
        ///         {
        ///             reader.ReadByte(); // Reads the major version of the encoded application. (This value is ignored.)
        ///             reader.ReadByte(); // Reads the minor version of the encoded application. (This value is ignored.)
        ///             var length = reader.ReadUInt16LE(); // Read the length of the property. (This value should always match LZMA_CONTENT_PROPERTY_SIZE)
        ///             if (length != LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE)
        ///                 throw new Exception("Bad data format.");
        ///             reader.ReadProperty(); // Read the 5-byte property and set it in the decoder.
        ///             return sizeof(Byte) + sizeof(Byte) + sizeof(UInt16) + LzmaDecoder.LZMA_CONTENT_PROPERTY_SIZE;
        ///         });
        /// </code>
        /// </example>
        public static LzmaDecoderStream Create(IO.ISequentialInStream compressedInStream, LzmaDecoderProperties properties, UInt64? uncompressedOutStreamSize, IO.CoderHeaderParser lzmaHeaderParser)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            return Create(compressedInStream.GetStreamReader(), properties, uncompressedOutStreamSize, lzmaHeaderParser);
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
                    (_compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
            base.Dispose(disposing);
        }

        private static LzmaDecoderStream Create(SequentialInStreamReader compressedInStreamReader, LzmaDecoderProperties properties, UInt64? uncompressedOutStreamSize, IO.CoderHeaderParser lzmaHeaderParser)
        {
            ICompressCoder? compressCoder = null;
            ISequentialInStream? sequentialInStream = null;
            ICompressGetInStreamProcessedSize? compressGetInStreamProcessedSize = null;
            ICompressSetOutStreamSize? compressSetOutStreamSize = null;
            ICompressSetDecoderProperties2? compressSetDecoderProperties2 = null;
            ICompressSetInStream? compressSetInStream = null;
            ICompressSetFinishMode? compressSetFinishMode = null;
            ICompressSetBufSize? compressSetBufSize = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("LZMA", CoderType.Decoder);
                sequentialInStream = (ISequentialInStream)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressGetInStreamProcessedSize = (ICompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(ICompressGetInStreamProcessedSize));
                compressSetOutStreamSize = (ICompressSetOutStreamSize)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressSetDecoderProperties2 = (ICompressSetDecoderProperties2)compressCoder.QueryInterface(typeof(ICompressSetDecoderProperties2));
                compressSetInStream = (ICompressSetInStream)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
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
                compressSetInStream.SetInStream(compressedInStreamReader);
                var lzmaDecoderHeaderReader =
                    IO.CoderHeaderReader.Create(
                        compressedInStreamReader,
                        () =>
                        {
                            Span<Byte> propertiesBuffer = stackalloc Byte[LZMA_CONTENT_PROPERTY_SIZE];
                            compressedInStreamReader.ReadBytes(propertiesBuffer);
                        compressSetDecoderProperties2.SetDecoderProperties2(propertiesBuffer);
                        });
                var coder =
                    new LzmaDecoderStream(
                        sequentialInStream,
                        compressGetInStreamProcessedSize,
                        compressSetOutStreamSize,
                        lzmaHeaderParser,
                        lzmaDecoderHeaderReader,
                        uncompressedOutStreamSize);
                success = true;
                return coder;
            }
            finally
            {
                if (!success)
                {
                    (compressSetOutStreamSize as IDisposable)?.Dispose();
                    (compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                    (sequentialInStream as IDisposable)?.Dispose();
                }
                (compressSetBufSize as IDisposable)?.Dispose();
                (compressSetFinishMode as IDisposable)?.Dispose();
                (compressSetInStream as IDisposable)?.Dispose();
                (compressSetDecoderProperties2 as IDisposable)?.Dispose();
                (compressCoder as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Read the data from the coder.
        /// </summary>
        /// <param name="data">
        /// Set a buffer to store the data to be read.
        /// </param>
        /// <returns>
        /// Returns the byte length of the read data.
        /// If the end of the stream is reached and no more data can be read, 0 is returned.
        /// </returns>
        public override Int32 Read(Span<Byte> data)
        {
            if (_isFirst)
            {
                _lzmaHeaderParser(_lzmaHeaderReader);
                UInt64? uncompressedOutStreamSize;
                if (_uncompressedOutStreamSize is null)
                    uncompressedOutStreamSize = _lzmaHeaderReader.OutStreamSize;
                else if (_lzmaHeaderReader.OutStreamSize is null)
                    uncompressedOutStreamSize = _uncompressedOutStreamSize;
                else
                    uncompressedOutStreamSize = _uncompressedOutStreamSize.Value.Minimum(_lzmaHeaderReader.OutStreamSize.Value);
                if (uncompressedOutStreamSize.HasValue)
                    _compressSetOutStreamSize.SetOutStreamSize(uncompressedOutStreamSize);
                _isFirst = false;
            }
            return base.Read(data);
        }
    }
}
