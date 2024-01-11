using System;
using System.IO;
using Palmtree.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression.Lzma
{
    /// <summary>
    /// A class of LZMA decoders.
    /// </summary>
    public class LzmaDecoder
        : IDisposable, ICompressCoder, ICompressGetInStreamProcessedSize
    {
        /// <summary>
        /// The length of the property embedded in the compressed stream in LZMA format.
        /// </summary>
        public const Int32 CONTENT_PROPERTY_SIZE = LzmaConstants.CONTENT_PROPERTY_SIZE;

        private readonly CompressCoder _compressCoder;
        private readonly CompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;

        private Boolean _isDisposed;

        private LzmaDecoder(
            CompressCoder compressCoder,
            CompressGetInStreamProcessedSize compressGetInStreamProcessedSize)
        {
            _compressCoder = compressCoder;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _isDisposed = false;
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
        /// Create an instance of <see cref="LzmaDecoder"/> with default properties.
        /// </summary>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in LZMA format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="LzmaDecoder"/> created.
        /// </returns>
        public static LzmaDecoder CreateDecoder(ReadOnlySpan<Byte> contentProperties) => CreateDecoder(new LzmaDecoderProperties(), contentProperties);

        /// <summary>
        /// Create an instance of <see cref="LzmaDecoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the LZMA decoder.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in LZMA format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="LzmaDecoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static LzmaDecoder CreateDecoder(LzmaDecoderProperties properties, ReadOnlySpan<Byte> contentProperties)
        {
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (contentProperties.Length != CONTENT_PROPERTY_SIZE)
                throw new ArgumentException($"{nameof(contentProperties)} is not {CONTENT_PROPERTY_SIZE} bytes long.: length={contentProperties.Length}", nameof(contentProperties));

            var compressCoder = (CompressCoder?)null;
            var compressSetDecoderProperties2 = (CompressSetDecoderProperties2?)null;
            var compressSetFinishMode = (CompressSetFinishMode?)null;
            var compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize?)null;
            var compressSetBufSize = (CompressSetBufSize?)null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsCollection.Instance.CreateCompressCoder(LzmaConstants.CODER_NAME, CoderType.Decoder);
                compressSetDecoderProperties2 = (CompressSetDecoderProperties2)compressCoder.QueryInterface(typeof(CompressSetDecoderProperties2));
                compressSetDecoderProperties2.SetDecoderProperties2(contentProperties);
                if (properties.FinishMode is not null)
                {
                    compressSetFinishMode = (CompressSetFinishMode)compressCoder.QueryInterface(typeof(CompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }

                compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(CompressGetInStreamProcessedSize));
                if (properties.InBufSize is not null || properties.OutBufSize is not null)
                {
                    compressSetBufSize = (CompressSetBufSize)compressCoder.QueryInterface(typeof(CompressSetBufSize));
                    if (properties.InBufSize is not null)
                    {
                        // The streamIndex parameter is ignored in the LZMA decoder.
                        compressSetBufSize.SetInBufSize(0, properties.InBufSize.Value);
                    }

                    if (properties.OutBufSize is not null)
                    {
                        // The streamIndex parameter is ignored in the LZMA decoder.
                        compressSetBufSize.SetOutBufSize(0, properties.OutBufSize.Value);
                    }
                }

                var decoder = new LzmaDecoder(compressCoder, compressGetInStreamProcessedSize);
                success = true;
                return decoder;
            }
            finally
            {
                if (!success)
                {
                    compressGetInStreamProcessedSize?.Dispose();
                    compressCoder?.Dispose();
                }

                compressSetBufSize?.Dispose();
                compressSetFinishMode?.Dispose();
                compressSetDecoderProperties2?.Dispose();
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
        /// <para>
        /// Set the length in bytes of the compressed data.
        /// </para>
        /// <para>
        /// Set null if the length of the data is unknown.
        /// </para>
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
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 23.01" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="uncompressedOutStream"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="compressedInStream"/> does not support reading, or <paramref name="uncompressedOutStream"/> does not support writing.</exception>
        public void Code(Stream compressedInStream, Stream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64 inStreamProcessedCount, UInt64 outStreamProcessedCount)>? progress)
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
                compressedInStream,
                uncompressedOutStream,
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
        /// <para>
        /// Set the length in bytes of the compressed data.
        /// </para>
        /// <para>
        /// Set null if the length of the data is unknown.
        /// </para>
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
        /// and you must have an implementation of the <see cref="ISequentialInputByteStream"/> and <see cref="ISequentialOutputByteStream"/> interfaces in advance.</description></item>
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 23.01" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="uncompressedOutStream"/> is null.</exception>
        public void Code(ISequentialInputByteStream compressedInStream, ISequentialOutputByteStream uncompressedOutStream, UInt64? compressedInStreamSize, UInt64? uncompressedOutStreamSize, IProgress<(UInt64 inStreamProcessedCount, UInt64 outStreamProcessedCount)>? progress)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (uncompressedOutStream is null)
                throw new ArgumentNullException(nameof(uncompressedOutStream));

            _compressCoder.Code(
                compressedInStream,
                uncompressedOutStream,
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress);
        }

        /// <summary>
        /// Create a stream that reads and decodes data from a sequential input stream with default properties.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in LZMA format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="ICompressDecoderStreamWithICompressReadUnusedFromInBuf"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> is null.</exception>
        public static ICompressDecoderStream CreateDecoderStream(
            ISequentialInputByteStream compressedInStream,
            UInt64? uncompressedOutStreamSize,
            ReadOnlySpan<Byte> contentProperties,
            Boolean leaveOpen = false)
            => CreateDecoderStream(compressedInStream, new LzmaDecoderProperties(), uncompressedOutStreamSize, contentProperties, leaveOpen);

        /// <summary>
        /// Create a stream that reads and decodes data from a sequential input stream with default properties.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in LZMA format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="ICompressDecoderStreamWithICompressReadUnusedFromInBuf"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> is null.</exception>
        public static CompressDecoderDotNetStream CreateDecoderStream(
            Stream compressedInStream,
            UInt64? uncompressedOutStreamSize,
            ReadOnlySpan<Byte> contentProperties,
            Boolean leaveOpen = false)
            => CreateDecoderStream(compressedInStream, new LzmaDecoderProperties(), uncompressedOutStreamSize, contentProperties, leaveOpen);

        /// <summary>
        /// Create a stream that reads and decodes data from a sequential input stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a container object with properties that specify the behavior of the deflate decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in LZMA format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="ICompressDecoderStream"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        public static ICompressDecoderStream CreateDecoderStream(
            ISequentialInputByteStream compressedInStream,
            LzmaDecoderProperties properties,
            UInt64? uncompressedOutStreamSize,
            ReadOnlySpan<Byte> contentProperties,
            Boolean leaveOpen = false)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (contentProperties.Length != CONTENT_PROPERTY_SIZE)
                throw new ArgumentException($"{nameof(contentProperties)} is not {CONTENT_PROPERTY_SIZE} bytes long.: length={contentProperties.Length}", nameof(contentProperties));

            var (sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize) =
                GetParameterToCreateStream(
                    properties,
                    uncompressedOutStreamSize,
                    contentProperties,
                    setter => setter.SetInStream(compressedInStream.FromInputStreamToNativeDelegate()));
            return
                new DecoderPalmtreeStream(
                    compressedInStream,
                    sequentialInStream,
                    compressSetInStream,
                    compressGetInStreamProcessedSize,
                    leaveOpen);
        }

        /// <summary>
        /// Create a stream that reads and decodes data from a sequential input stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a container object with properties that specify the behavior of the deflate decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in LZMA format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="CompressDecoderDotNetStream"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="compressedInStream"/> does not support reading.</exception>
        public static CompressDecoderDotNetStream CreateDecoderStream(
            Stream compressedInStream,
            LzmaDecoderProperties properties,
            UInt64? uncompressedOutStreamSize,
            ReadOnlySpan<Byte> contentProperties,
            Boolean leaveOpen = false)
        {
            if (compressedInStream is null)
                throw new ArgumentNullException(nameof(compressedInStream));
            if (!compressedInStream.CanRead)
                throw new ArgumentException("The specified stream does not support reading.", nameof(compressedInStream));
            if (properties is null)
                throw new ArgumentNullException(nameof(properties));
            if (contentProperties.Length != CONTENT_PROPERTY_SIZE)
                throw new ArgumentException($"{nameof(contentProperties)} is not {CONTENT_PROPERTY_SIZE} bytes long.: length={contentProperties.Length}", nameof(contentProperties));

            var (sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize) =
                GetParameterToCreateStream(
                    properties,
                    uncompressedOutStreamSize,
                    contentProperties,
                    setter => setter.SetInStream(compressedInStream.FromInputStreamToNativeDelegate()));
            return
                new CompressDecoderDotNetStream(
                    compressedInStream,
                    sequentialInStream,
                    compressSetInStream,
                    compressGetInStreamProcessedSize,
                    leaveOpen);
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
                {
                    _compressGetInStreamProcessedSize.Dispose();
                    _compressCoder.Dispose();
                }

                _isDisposed = true;
            }
        }

        private static (SequentialInStream sequentialInStream, CompressSetInStream compressSetInStream, CompressGetInStreamProcessedSize compressGetInStreamProcessedSize) GetParameterToCreateStream(
            LzmaDecoderProperties properties,
            UInt64? uncompressedOutStreamSize,
            ReadOnlySpan<Byte> contentProperties,
            Action<ICompressSetInStream> inStreamSetter)
        {
            var compressCoder = (CompressCoder?)null;
            var compressSetDecoderProperties2 = (CompressSetDecoderProperties2?)null;
            var compressSetFinishMode = (CompressSetFinishMode?)null;
            var compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize?)null;
            var compressSetBufSize = (CompressSetBufSize?)null;
            var compressSetInStream = (CompressSetInStream?)null;
            var compressSetOutStreamSize = (CompressSetOutStreamSize?)null;
            var sequentialInStream = (SequentialInStream?)null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsCollection.Instance.CreateCompressCoder(LzmaConstants.CODER_NAME, CoderType.Decoder);
                compressSetDecoderProperties2 = (CompressSetDecoderProperties2)compressCoder.QueryInterface(typeof(CompressSetDecoderProperties2));
                compressSetDecoderProperties2.SetDecoderProperties2(contentProperties);
                if (properties.FinishMode is not null)
                {
                    compressSetFinishMode = (CompressSetFinishMode)compressCoder.QueryInterface(typeof(CompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }

                compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(CompressGetInStreamProcessedSize));
                if (properties.InBufSize is not null || properties.OutBufSize is not null)
                {
                    compressSetBufSize = (CompressSetBufSize)compressCoder.QueryInterface(typeof(CompressSetBufSize));
                    if (properties.InBufSize is not null)
                    {
                        // The streamIndex parameter is ignored in the LZMA decoder.
                        compressSetBufSize.SetInBufSize(0, properties.InBufSize.Value);
                    }

                    if (properties.OutBufSize is not null)
                    {
                        // The streamIndex parameter is ignored in the LZMA decoder.
                        compressSetBufSize.SetOutBufSize(0, properties.OutBufSize.Value);
                    }
                }

                compressSetInStream = (CompressSetInStream)compressCoder.QueryInterface(typeof(CompressSetInStream));
                inStreamSetter(compressSetInStream);
                compressSetOutStreamSize = (CompressSetOutStreamSize)compressCoder.QueryInterface(typeof(CompressSetOutStreamSize));
                compressSetOutStreamSize.SetOutStreamSize(uncompressedOutStreamSize);
                sequentialInStream = (SequentialInStream)compressCoder.QueryInterface(typeof(SequentialInStream));
                success = true;
                return (sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize);
            }
            finally
            {
                if (!success)
                {
                    sequentialInStream?.Dispose();
                    compressSetInStream?.Dispose();
                    compressGetInStreamProcessedSize?.Dispose();
                }

                compressSetOutStreamSize?.Dispose();
                compressSetBufSize?.Dispose();
                compressSetFinishMode?.Dispose();
                compressSetDecoderProperties2?.Dispose();
                compressCoder?.Dispose();
            }
        }
    }
}
