using System;
using System.Threading;
using System.Threading.Tasks;
using Palmtree.IO;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression
{
    internal class DecoderPalmtreeStream
        : ICompressDecoderStream
    {
        private readonly ISequentialInputByteStream _baseStream;
        private readonly SequentialInStream _sequentialInStream;
        private readonly CompressSetInStream _compressSetInStream;
        private readonly CompressGetInStreamProcessedSize _compressGetInStreamProcessedSize;
        private readonly Boolean _leaveOpen;
        private Boolean _isDisposed;

        public DecoderPalmtreeStream(
            ISequentialInputByteStream baseStream,
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
        }

        public Int32 Read(Span<Byte> buffer)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            return checked((Int32)_sequentialInStream.Read(buffer));
        }

        public Task<Int32> ReadAsync(Memory<Byte> buffer, CancellationToken cancellationToken = default)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            cancellationToken.ThrowIfCancellationRequested();
            return Task.Run(() => checked((Int32)_sequentialInStream.Read(buffer.Span)));
        }

        public UInt64 InStreamProcessedSize
        {
            get
            {
                ObjectDisposedException.ThrowIf(_isDisposed, this);

                return _compressGetInStreamProcessedSize.InStreamProcessedSize;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _compressGetInStreamProcessedSize.Dispose();
                    _sequentialInStream.Dispose();
                    if (!_leaveOpen)
                        _baseStream.Dispose();
                }

                _isDisposed = true;
            }
        }

        protected virtual async Task DisposeAsyncCore()
        {
            if (!_isDisposed)
            {
                _compressGetInStreamProcessedSize.Dispose();
                _compressSetInStream.Dispose();
                _sequentialInStream.Dispose();
                if (!_leaveOpen)
                    await _baseStream.DisposeAsync().ConfigureAwait(false);
                _isDisposed = true;
            }
        }
    }
}
