using System;
using System.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression
{
    /// <summary>
    /// A decoder class that inherits from the <see cref="Stream"/> class and implements the <see cref="ICompressGetInStreamProcessedSize"/> and <see cref="ICompressReadUnusedFromInBuf"/> interfaces.
    /// </summary>
    public class CompressDecoderDotNetStreamWithICompressReadUnusedFromInBuf
        : CompressDecoderDotNetStream, ICompressReadUnusedFromInBuf
    {
        private readonly CompressReadUnusedFromInBuf _compressReadUnusedFromInBuf;
        private Boolean _isDisposed;

        internal CompressDecoderDotNetStreamWithICompressReadUnusedFromInBuf(
            Stream baseStream,
            SequentialInStream sequentialInStream,
            CompressSetInStream compressSetInStream,
            CompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            CompressReadUnusedFromInBuf compressReadUnusedFromInBuf,
            Boolean leaveOpen)
            : base(baseStream, sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize, leaveOpen)
        {
            _compressReadUnusedFromInBuf = compressReadUnusedFromInBuf;
            _isDisposed = false;
        }

        /// <inheritdoc/>
        public UInt32 ReadUnusedFromInBuf(Span<Byte> data)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            return _compressReadUnusedFromInBuf.ReadUnusedFromInBuf(data);
        }

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                    _compressReadUnusedFromInBuf.Dispose();
                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
