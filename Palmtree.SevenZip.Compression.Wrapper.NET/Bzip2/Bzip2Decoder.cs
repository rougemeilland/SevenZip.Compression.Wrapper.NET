using System;
using System.IO;
using Palmtree.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression.Bzip2
{
    /// <summary>
    /// A class of BZIP2 decoders.
    /// </summary>
    public partial class Bzip2Decoder
        : IDisposable, ICompressCoder, ICompressGetInStreamProcessedSize, ICompressReadUnusedFromInBuf
    {
        private readonly CompressCoder _compressCoder;
        private readonly CompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;
        private readonly CompressReadUnusedFromInBuf _compressReadUnusedFromInBuf;

        private Boolean _isDisposed;

        private Bzip2Decoder(
            CompressCoder compressCoder,
            CompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            CompressReadUnusedFromInBuf compressReadUnusedFromInBuf)
        {
            _isDisposed = false;
            _compressCoder = compressCoder;
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _compressReadUnusedFromInBuf = compressReadUnusedFromInBuf;
        }

        /// <summary>
        /// Create an instance of <see cref="Bzip2Decoder"/> with default properties.
        /// </summary>
        /// <returns>
        /// It is an instance of <see cref="Bzip2Decoder"/> created.
        /// </returns>
        public static Bzip2Decoder CreateDecoder() => CreateDecoder(new Bzip2DecoderProperties());

        /// <summary>
        /// Create an instance of <see cref="Bzip2Decoder"/>.
        /// </summary>
        /// <param name="properties">
        /// Set a property container object to customize the behavior of the BZIP2 decoder.
        /// </param>
        /// <returns>
        /// It is an instance of <see cref="Bzip2Decoder"/> created.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="properties"/> is null.</exception>
        public static Bzip2Decoder CreateDecoder(Bzip2DecoderProperties properties)
        {
            ArgumentNullException.ThrowIfNull(properties);

            var compressCoder = (CompressCoder?)null;
            var compressSetFinishMode = (CompressSetFinishMode?)null;
            var compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize?)null;
            var compressReadUnusedFromInBuf = (CompressReadUnusedFromInBuf?)null;
            var compressSetCoderMt = (CompressSetCoderMt?)null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsCollection.Instance.CreateCompressCoder(Bzip2Constants.CODER_NAME, CoderType.Decoder);
                compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(CompressGetInStreamProcessedSize));
                compressReadUnusedFromInBuf = (CompressReadUnusedFromInBuf)compressCoder.QueryInterface(typeof(CompressReadUnusedFromInBuf));
                if (properties.FinishMode is not null)
                {
                    compressSetFinishMode = (CompressSetFinishMode)compressCoder.QueryInterface(typeof(CompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }

                if (properties.NumThreads is not null)
                {
                    compressSetCoderMt = (CompressSetCoderMt)compressCoder.QueryInterface(typeof(CompressSetCoderMt));
                    compressSetCoderMt.SetNumberOfThreads(properties.NumThreads.Value);
                }

                var decoder =
                    new Bzip2Decoder(
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
                    compressReadUnusedFromInBuf?.Dispose();
                    compressGetInStreamProcessedSize?.Dispose();
                    compressCoder?.Dispose();
                }

                compressSetCoderMt?.Dispose();
                compressSetFinishMode?.Dispose();
            }
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with BZIP2, and write the uncompressed data to the output stream.
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
            ArgumentNullException.ThrowIfNull(uncompressedOutStream);

            _compressCoder.Code(
                compressedInStream,
                uncompressedOutStream,
                compressedInStreamSize,
                uncompressedOutStreamSize,
                progress);
        }

        /// <summary>
        /// Read the compressed data from the input stream, decode it with BZIP2, and write the uncompressed data to the output stream.
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
        /// and you must have an implementation of the <see cref="ISequentialInputByteStream"/> and <see cref="ISequentialOutputByteStream"/> interfaces in advance.</description></item>
        /// <item><description>The meaning of the parameter set in the Code method is based on "7-zip 23.01" and may be changed in the future.</description></item>
        /// </list>
        /// </remarks>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="uncompressedOutStream"/> is null.</exception>
        /// <inheritdoc/>
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
        /// The number of bytes of processed data in the decoder's input stream.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        public UInt64 InStreamProcessedSize
        {
            get
            {
                ObjectDisposedException.ThrowIf(_isDisposed, this);

                return _compressGetInStreamProcessedSize.InStreamProcessedSize;
            }
        }

        /// <summary>
        /// Reads the remaining data after processing Bzip2Decoder.Code() from the input stream.
        /// </summary>
        /// <param name="data">
        /// A buffer for storing the data to be read.
        /// </param>
        /// <returns>
        /// The length in bytes of the data actually read.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The decoder has already been disposed.</exception>
        public UInt32 ReadUnusedFromInBuf(Span<Byte> data)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            return _compressReadUnusedFromInBuf.ReadUnusedFromInBuf(data);
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
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="ICompressDecoderStreamWithICompressReadUnusedFromInBuf"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> is null.</exception>
        public static ICompressDecoderStreamWithICompressReadUnusedFromInBuf CreateDecoderStream(
            ISequentialInputByteStream compressedInStream,
            UInt64? uncompressedOutStreamSize,
            Boolean leaveOpen = false)
            => CreateDecoderStream(compressedInStream, new Bzip2DecoderProperties(), uncompressedOutStreamSize, leaveOpen);

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
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="ICompressDecoderStreamWithICompressReadUnusedFromInBuf"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> is null.</exception>
        public static CompressDecoderDotNetStreamWithICompressReadUnusedFromInBuf CreateDecoderStream(
            Stream compressedInStream,
            UInt64? uncompressedOutStreamSize,
            Boolean leaveOpen = false)
            => CreateDecoderStream(compressedInStream, new Bzip2DecoderProperties(), uncompressedOutStreamSize, leaveOpen);

        /// <summary>
        /// Create a stream that reads and decodes data from a sequential input stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a container object with properties that specify the behavior of the BZIP2 decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="ICompressDecoderStreamWithICompressReadUnusedFromInBuf"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        public static ICompressDecoderStreamWithICompressReadUnusedFromInBuf CreateDecoderStream(
            ISequentialInputByteStream compressedInStream,
            Bzip2DecoderProperties properties,
            UInt64? uncompressedOutStreamSize,
            Boolean leaveOpen = false)
        {
            ArgumentNullException.ThrowIfNull(compressedInStream);
            ArgumentNullException.ThrowIfNull(properties);

            var (sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize, compressReadUnusedFromInBuf) =
                GetParameterToCreateStream(
                    properties,
                    uncompressedOutStreamSize,
                    setter => setter.SetInStream(compressedInStream.FromInputStreamToNativeDelegate()));
            return
                new DecoderPalmtreeStreamWithICompressReadUnusedFromInBuf(
                    compressedInStream,
                    sequentialInStream,
                    compressSetInStream,
                    compressGetInStreamProcessedSize,
                    compressReadUnusedFromInBuf,
                    leaveOpen);
        }

        /// <summary>
        /// Create a stream that reads and decodes data from a sequential input stream.
        /// </summary>
        /// <param name="compressedInStream">
        /// Set the input stream to read the compressed data.
        /// </param>
        /// <param name="properties">
        /// Set a container object with properties that specify the behavior of the BZIP2 decoder.
        /// </param>
        /// <param name="uncompressedOutStreamSize">
        /// Set the length in bytes of the uncompressed data.
        /// Set null if the length of the uncompressed data is unknown.
        /// </param>
        /// <param name="leaveOpen">
        /// true if <paramref name="compressedInStream"/> remains unreleased after this instance is disposed, false otherwise.
        /// </param>
        /// <returns>
        /// The created <see cref="CompressDecoderDotNetStreamWithICompressReadUnusedFromInBuf"/> object.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="compressedInStream"/> or <paramref name="properties"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="compressedInStream"/> does not support reading.</exception>
        public static CompressDecoderDotNetStreamWithICompressReadUnusedFromInBuf CreateDecoderStream(
            Stream compressedInStream,
            Bzip2DecoderProperties properties,
            UInt64? uncompressedOutStreamSize,
            Boolean leaveOpen = false)
        {
            ArgumentNullException.ThrowIfNull(compressedInStream);
            if (!compressedInStream.CanRead)
                throw new ArgumentException("The specified stream does not support reading.", nameof(compressedInStream));
            ArgumentNullException.ThrowIfNull(properties);

            var (sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize, compressReadUnusedFromInBuf) =
                GetParameterToCreateStream(
                    properties,
                    uncompressedOutStreamSize,
                    setter => setter.SetInStream(compressedInStream.FromInputStreamToNativeDelegate()));
            return
                new CompressDecoderDotNetStreamWithICompressReadUnusedFromInBuf(
                    compressedInStream,
                    sequentialInStream,
                    compressSetInStream,
                    compressGetInStreamProcessedSize,
                    compressReadUnusedFromInBuf,
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
                    _compressReadUnusedFromInBuf.Dispose();
                    _compressReadUnusedFromInBuf.Dispose();
                    _compressGetInStreamProcessedSize.Dispose();
                    _compressCoder.Dispose();
                }

                _isDisposed = true;
            }
        }

        private static (SequentialInStream sequentialInStream, CompressSetInStream compressSetInStream, CompressGetInStreamProcessedSize compressGetInStreamProcessedSize, CompressReadUnusedFromInBuf compressReadUnusedFromInBuf) GetParameterToCreateStream(
            Bzip2DecoderProperties properties,
            UInt64? uncompressedOutStreamSize,
            Action<ICompressSetInStream> inStreamSetter)
        {
            var compressCoder = (CompressCoder?)null;
            var compressSetFinishMode = (CompressSetFinishMode?)null;
            var compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize?)null;
            var compressReadUnusedFromInBuf = (CompressReadUnusedFromInBuf?)null;
            var compressSetInStream = (CompressSetInStream?)null;
            var compressSetOutStreamSize = (CompressSetOutStreamSize?)null;
            var sequentialInStream = (SequentialInStream?)null;
            var compressSetCoderMt = (CompressSetCoderMt?)null;
            var success = false;
            try
            {
                compressCoder = CompressCodecsCollection.Instance.CreateCompressCoder(Bzip2Constants.CODER_NAME, CoderType.Decoder);
                compressGetInStreamProcessedSize = (CompressGetInStreamProcessedSize)compressCoder.QueryInterface(typeof(CompressGetInStreamProcessedSize));
                compressReadUnusedFromInBuf = (CompressReadUnusedFromInBuf)compressCoder.QueryInterface(typeof(CompressReadUnusedFromInBuf));
                compressSetInStream = (CompressSetInStream)compressCoder.QueryInterface(typeof(CompressSetInStream));
                inStreamSetter(compressSetInStream);
                compressSetOutStreamSize = (CompressSetOutStreamSize)compressCoder.QueryInterface(typeof(CompressSetOutStreamSize));
                compressSetOutStreamSize.SetOutStreamSize(uncompressedOutStreamSize);
                sequentialInStream = (SequentialInStream)compressCoder.QueryInterface(typeof(SequentialInStream));
                if (properties.FinishMode is not null)
                {
                    compressSetFinishMode = (CompressSetFinishMode)compressCoder.QueryInterface(typeof(CompressSetFinishMode));
                    compressSetFinishMode.SetFinishMode(properties.FinishMode.Value);
                }

                if (properties.NumThreads is not null)
                {
                    compressSetCoderMt = (CompressSetCoderMt)compressCoder.QueryInterface(typeof(CompressSetCoderMt));
                    compressSetCoderMt.SetNumberOfThreads(properties.NumThreads.Value);
                }

                success = true;
                return (sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize, compressReadUnusedFromInBuf);
            }
            finally
            {
                if (!success)
                {
                    compressReadUnusedFromInBuf?.Dispose();
                    compressGetInStreamProcessedSize?.Dispose();
                    compressSetInStream?.Dispose();
                    sequentialInStream?.Dispose();
                }

                compressSetOutStreamSize?.Dispose();
                compressSetCoderMt?.Dispose();
                compressSetFinishMode?.Dispose();
                compressCoder?.Dispose();
            }
        }
    }
}
