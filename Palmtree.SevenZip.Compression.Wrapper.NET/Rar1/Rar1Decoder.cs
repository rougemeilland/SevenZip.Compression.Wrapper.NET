using System;
using System.IO;
using Palmtree.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression.Rar1
{
    /// <summary>
    /// A class of Rar1 decoders.
    /// </summary>
    public class Rar1Decoder
        : IDisposable
    {
        /// <summary>
        /// The length of the property embedded in the compressed stream in Rar1 format.
        /// </summary>
        public const Int32 CONTENT_PROPERTY_SIZE = Rar1Constants.CONTENT_PROPERTY_SIZE;

        private readonly CompressCoder _compressCoder;

        private Boolean _isDisposed;

        private Rar1Decoder(CompressCoder compressCoder)
        {
            _isDisposed = false;
            _compressCoder = compressCoder;
        }

        /// <summary>
        /// Create an instance of <see cref="Rar1Decoder"/> with default properties.
        /// </summary>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in Rar1 format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Rar1Decoder"/> created.
        /// </returns>
        public static Rar1Decoder CreateDecoder(ReadOnlySpan<Byte> contentProperties) => CreateDecoder(new Rar1DecoderProperties(), contentProperties);

        /// <summary>
        /// Create an instance of <see cref="Rar1Decoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the Rar1 decoder.
        /// </param>
        /// <param name="contentProperties">
        /// <para>
        /// Set the data that represents the parameters of the compressed data in Rar1 format.
        /// </para>
        /// <para>
        /// See the following documents for the meaning of this parameter.:
        /// "<seealso href="https://github.com/rougemeilland/SevenZip.Compression.Wrapper.NET/blob/main/docs/AboutContentProperty_en.md">About content property</seealso>"
        /// </para>
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Rar1Decoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static Rar1Decoder CreateDecoder(Rar1DecoderProperties properties, ReadOnlySpan<Byte> contentProperties)
        {
            ArgumentNullException.ThrowIfNull(properties);
            if (contentProperties.Length != CONTENT_PROPERTY_SIZE)
                throw new ArgumentException($"{nameof(contentProperties)} is not {CONTENT_PROPERTY_SIZE} bytes long.: length={contentProperties.Length}", nameof(contentProperties));

            var compressCoder = (CompressCoder?)null;
            var compressSetDecoderProperties2 = (CompressSetDecoderProperties2?)null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsCollection.Instance.CreateCompressCoder(Rar1Constants.CODER_NAME, CoderType.Decoder);
                compressSetDecoderProperties2 = (CompressSetDecoderProperties2)compressCoder.QueryInterface(typeof(CompressSetDecoderProperties2));
                compressSetDecoderProperties2.SetDecoderProperties2(contentProperties);
                var decoder = new Rar1Decoder(compressCoder);
                success = true;
                return decoder;
            }
            finally
            {
                if (!success)
                    compressCoder?.Dispose();
                compressSetDecoderProperties2?.Dispose();
            }
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with Rar1, and write the uncompressed data to the output stream.
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
        /// <b>Do not set this parameter to null.</b>
        /// </para>
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data.
        /// </para>
        /// <para>
        /// <b>Do not set this parameter to null.</b>
        /// </para>
        /// </param>
        /// <param name="progress">
        /// <b>The value of this parameter is ignored.</b>
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
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            ArgumentNullException.ThrowIfNull(compressedInStream);
            if (!compressedInStream.CanRead)
                throw new ArgumentException("The specified stream does not support reading.", nameof(compressedInStream));
            ArgumentNullException.ThrowIfNull(uncompressedOutStream);
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
        /// Read the compressed data from the input stream, decode it with Rar1, and write the uncompressed data to the output stream.
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
        /// <b>Do not set this parameter to null.</b>
        /// </para>
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// <para>
        /// Set the length in bytes of the uncompressed data.
        /// </para>
        /// <para>
        /// <b>Do not set this parameter to null.</b>
        /// </para>
        /// </param>
        /// <param name="progress">
        /// <b>The value of this parameter is ignored.</b>
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
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            ArgumentNullException.ThrowIfNull(compressedInStream);
            ArgumentNullException.ThrowIfNull(uncompressedOutStream);

            _compressCoder.Code(
                compressedInStream,
                uncompressedOutStream,
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress);
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
                    _compressCoder.Dispose();
                _isDisposed = true;
            }
        }
    }
}
