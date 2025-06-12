using System;
using System.Threading.Tasks;
using Palmtree.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression
{
    internal sealed class DecoderPalmtreeStreamWithICompressReadUnusedFromInBuf
        : DecoderPalmtreeStream, ICompressDecoderStreamWithICompressReadUnusedFromInBuf
    {
        private readonly CompressReadUnusedFromInBuf _compressReadUnusedFromInBuf;
        private Boolean _isDisposed;

        public DecoderPalmtreeStreamWithICompressReadUnusedFromInBuf(
            ISequentialInputByteStream baseStream,
            SequentialInStream sequentialInStream,
            CompressSetInStream compressSetInStream,
            CompressGetInStreamProcessedSize compressGetInStreamProcessedSize,
            CompressReadUnusedFromInBuf compressReadUnusedFromInBuf,
            Boolean leaveOpen)
            : base(baseStream, sequentialInStream, compressSetInStream, compressGetInStreamProcessedSize, leaveOpen)
        {
            _compressReadUnusedFromInBuf = compressReadUnusedFromInBuf;
        }

        public UInt32 ReadUnusedFromInBuf(Span<Byte> data)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            return _compressReadUnusedFromInBuf.ReadUnusedFromInBuf(data);
        }

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

        protected override Task DisposeAsyncCore()
        {
            if (!_isDisposed)
            {
                _compressReadUnusedFromInBuf.Dispose();
                _isDisposed = true;
            }

            return base.DisposeAsyncCore();
        }
    }
}
