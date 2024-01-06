using System;
using System.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression
{
    /// <summary>
    /// A decoder class that inherits from the <see cref="Stream"/> class and implements the <see cref="ICompressGetInStreamProcessedSize"/> interface.
    /// </summary>
    public class CompressDecoderDotNetStream
        : Stream, ICompressGetInStreamProcessedSize
    {
        private readonly Stream _baseStream;
        private readonly SequentialInStream _sequentialInStream;
        private readonly CompressSetInStream _compressSetInStream;
        private readonly CompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;
        private readonly Boolean _leaveOpen;
        private Boolean _isDisposed;

        internal CompressDecoderDotNetStream(
            Stream baseStream,
            SequentialInStream sequentialInStream,
            CompressSetInStream compressSetInStream,
            CompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            Boolean leaveOpen)
        {
            _baseStream = baseStream;
            _sequentialInStream = sequentialInStream;
            _compressSetInStream = compressSetInStream; // For the delegate set by SetInStream to remain valid, compressSetInStream must not be freed by the garbage collector.
            _compressGetInStreamProcessedSize = compressGetInStreamProcessedSize;
            _leaveOpen = leaveOpen;
            _isDisposed = false;
        }

        /// <inheritdoc/>
        public override Boolean CanRead => true;

        /// <inheritdoc/>
        public override Boolean CanSeek => false;

        /// <inheritdoc/>
        public override Boolean CanWrite => false;

        /// <inheritdoc/>
        public override Int64 Length => throw new NotSupportedException();

        /// <inheritdoc/>
        public override Int64 Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        /// <inheritdoc/>
        public override void Flush() { }

        /// <inheritdoc/>
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            return checked((Int32)_sequentialInStream.Read(buffer.AsSpan(offset, count)));
        }

        /// <inheritdoc/>
        public override Int64 Seek(Int64 offset, SeekOrigin origin) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void SetLength(Int64 value) => throw new NotSupportedException();

        /// <inheritdoc/>
        public override void Write(Byte[] buffer, Int32 offset, Int32 count) => throw new NotSupportedException();

        /// <inheritdoc/>
        public UInt64 InStreamProcessedSize
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().FullName);

                return _compressGetInStreamProcessedSize.InStreamProcessedSize;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _compressGetInStreamProcessedSize.Dispose();
                    _compressSetInStream.Dispose();
                    _sequentialInStream.Dispose();
                    if (!_leaveOpen)
                        _baseStream.Dispose();
                }

                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
