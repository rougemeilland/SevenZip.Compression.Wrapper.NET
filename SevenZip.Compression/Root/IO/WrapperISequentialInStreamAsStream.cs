using System;
using System.IO;

namespace SevenZip.IO
{
    class WrapperISequentialInStreamAsStream
        : Stream
    {
        private readonly ISequentialInStream _sourceStream;

        public WrapperISequentialInStreamAsStream(ISequentialInStream sourceStream)
        {
            _sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override Int64 Length => throw new NotSupportedException();

        public override Int64 Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush() => throw new NotSupportedException();

        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            if (buffer is null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (checked(offset + count) > buffer.Length)
                throw new ArgumentException($"{nameof(offset)} + {nameof(count)} exceeds {nameof(buffer.Length)}.");

            return _sourceStream.Read(new Span<Byte>(buffer, offset, count));
        }

        public override Int64 Seek(Int64 offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(Int64 value) => throw new NotSupportedException();

        public override void Write(Byte[] buffer, Int32 offset, Int32 count) => throw new NotSupportedException();
    }

}
