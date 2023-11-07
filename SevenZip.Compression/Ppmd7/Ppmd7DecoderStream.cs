using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeInterface.IO;
using System;
using System.IO;

namespace SevenZip.Compression.Ppmd7
{
    /// <summary>
    /// A class of PPMd7 (PPMd version H) decoders in virtual stream format that can be read sequentially.
    /// </summary>
    public class Ppmd7DecoderStream
        : IO.DecoderStream
    {
        /// <summary>
        /// The length of the property embedded in the compressed stream in PPMd7 format.
        /// </summary>
        public const Int32 LZMA_CONTENT_PROPERTY_SIZE = 5;

        private readonly ICompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;

        private bool _isDisposed;

        private Ppmd7DecoderStream(
            ISequentialInStream sequentialInStream,
            ICompressGetInStreamProcessedSize compressGetInStreamProcessedSize)
            : base(sequentialInStream)
        {
            _isDisposed = false;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
        }

        /// <summary>
        /// Create an instance of <see cref="Ppmd7DecoderStream"/>.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the PPMd7 decoder.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in PPMd7 format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data read from <paramref name="compressedInStream"/>.
        /// </para>
        /// <para>
        /// Set null if the length of the uncompressed data is unknown.
        /// </para>
        /// </param>
        /// <returns>
        /// The created <see cref="Ppmd7DecoderStream"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        public static Ppmd7DecoderStream Create(Stream compressedInStream, Ppmd7DecoderProperties properties, ReadOnlySpan<Byte> contentProperties, UInt64? uncompressedOutStreamSize)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            return Create(compressedInStream.GetStreamReader(), properties, contentProperties, uncompressedOutStreamSize);
        }

        /// <summary>
        /// Create an instance of <see cref="Ppmd7DecoderStream"/>.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the PPMd7 decoder.
        /// </param>
        /// <param name="contentProperties">
        /// Set the data that represents the parameters of the compressed data in PPMd7 format.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data read from <paramref name="compressedInStream"/>.
        /// </para>
        /// <para>
        /// Set null if the length of the uncompressed data is unknown.
        /// </para>
        /// </param>
        /// <returns>
        /// The created <see cref="Ppmd7DecoderStream"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        public static Ppmd7DecoderStream Create(IO.ISequentialInStream compressedInStream, Ppmd7DecoderProperties properties, ReadOnlySpan<Byte> contentProperties, UInt64? uncompressedOutStreamSize)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));

            return Create(compressedInStream.GetStreamReader(), properties, contentProperties, uncompressedOutStreamSize);
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

        private static Ppmd7DecoderStream Create(SequentialInStreamReader compressedInStreamReader, Ppmd7DecoderProperties properties, ReadOnlySpan<Byte> contentProperties, UInt64? uncompressedOutStreamSize)
        {
            ICompressCoder? compressCoder = null;
            ISequentialInStream? sequentialInStream = null;
            ICompressGetInStreamProcessedSize? compressGetInStreamProcessedSize = null;
            ICompressSetOutStreamSize? compressSetOutStreamSize = null;
            ICompressSetDecoderProperties2? compressSetDecoderProperties2 = null;
            ICompressSetInStream? compressSetInStream = null;
            ICompressSetFinishMode? compressSetFinishMode = null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsInfo.CreateCompressCoder("PPMD", CoderType.Decoder);
                sequentialInStream = (ISequentialInStream)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressGetInStreamProcessedSize = (ICompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(ICompressGetInStreamProcessedSize));
                compressSetOutStreamSize = (ICompressSetOutStreamSize)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressSetOutStreamSize.SetOutStreamSize(uncompressedOutStreamSize);
                compressSetInStream = (ICompressSetInStream)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                compressSetInStream.SetInStream(compressedInStreamReader);
                compressSetDecoderProperties2 = (ICompressSetDecoderProperties2)compressCoder.QueryInterface(typeof(ICompressSetDecoderProperties2));
                compressSetDecoderProperties2.SetDecoderProperties2(contentProperties);
                if (properties.FinishMode.HasValue)
                {
                    compressSetFinishMode = (ICompressSetFinishMode)compressCoder.QueryInterface(typeof(ICompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }
                var decoder = new Ppmd7DecoderStream(sequentialInStream, compressGetInStreamProcessedSize);
                success = true;
                return decoder;
            }
            finally
            {
                if (!success)
                {
                    (compressGetInStreamProcessedSize as IDisposable)?.Dispose();
                    (sequentialInStream as IDisposable)?.Dispose();
                }
                (compressSetFinishMode as IDisposable)?.Dispose();
                (compressSetDecoderProperties2 as IDisposable)?.Dispose();
                (compressSetOutStreamSize as IDisposable)?.Dispose();
                (compressSetInStream as IDisposable)?.Dispose();
                (compressCoder as IDisposable)?.Dispose();
            }
        }
    }
}
