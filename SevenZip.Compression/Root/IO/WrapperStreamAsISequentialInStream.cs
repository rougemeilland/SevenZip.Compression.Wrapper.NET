using System;
using System.IO;

namespace SevenZip.IO
{
    class WrapperStreamAsISequentialInStream
        : ISequentialInStream
    {
        private readonly Stream _sourceStream;

        public WrapperStreamAsISequentialInStream(Stream sourceStream)
        {
            _sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            if (!_sourceStream.CanRead)
                throw new NotSupportedException();
        }

        public Int32 Read(Span<Byte> data) => _sourceStream.Read(data);
    }
}
