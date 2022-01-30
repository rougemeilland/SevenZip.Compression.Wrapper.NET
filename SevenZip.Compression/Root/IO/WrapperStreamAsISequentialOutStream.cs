using System;
using System.IO;

namespace SevenZip.IO
{
    class WrapperStreamAsISequentialOutStream
        : ISequentialOutStream
    {
        private readonly Stream _sourceStream;

        public WrapperStreamAsISequentialOutStream(Stream sourceStream)
        {
            _sourceStream = sourceStream ?? throw new ArgumentNullException(nameof(sourceStream));
            if (!_sourceStream.CanWrite)
                throw new NotSupportedException();
        }

        public Int32 Write(ReadOnlySpan<Byte> data)
        {
            _sourceStream.Write(data);
            return data.Length;
        }
    }
}
