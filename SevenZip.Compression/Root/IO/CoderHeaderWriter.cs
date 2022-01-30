using System;
using System.IO;

namespace SevenZip.IO
{
    class CoderHeaderWriter
          : ICoderHeaderWriter
    {
        private delegate void BytesDataWriter(ReadOnlySpan<Byte> data);
        private delegate void CoderPropertySetter();

        private BytesDataWriter _bytesDataWriter;
        private CoderPropertySetter _coderPropertySetter;
        private UInt64? _inStreamSize;
        private UInt64? _outStreamSize;

        private CoderHeaderWriter(BytesDataWriter bytesDataWriter, CoderPropertySetter coderPropertySetter)
        {
            _bytesDataWriter = bytesDataWriter;
            _coderPropertySetter = coderPropertySetter;
            _inStreamSize = null;
            _outStreamSize = null;
        }

        public UInt64? InStreamSize => _inStreamSize;
        public UInt64? OutStreamSize => _outStreamSize;

        public static CoderHeaderWriter Create(Stream outStream, Action coderPropertySetter) =>
            new(
                buffer => outStream.Write(buffer),
                () => coderPropertySetter());

        public static CoderHeaderWriter Create(ISequentialOutStream outStream, Action coderPropertySetter) =>
            new(
                buffer => outStream.Write(buffer),
                () => coderPropertySetter());

        void ICoderHeaderWriter.WriteByte(Byte value)
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(Byte)];
            buffer[0] = value;
            WriteBytes(buffer);
        }

        void ICoderHeaderWriter.WriteUInt16BE(UInt16 value)
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt16)];
            buffer[0] = (Byte)(value >> (8 * 1));
            buffer[1] = (Byte)(value >> (8 * 0));
            WriteBytes(buffer);
        }

        void ICoderHeaderWriter.WriteUInt16LE(UInt16 value)
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt16)];
            buffer[0] = (Byte)(value >> (8 * 0));
            buffer[1] = (Byte)(value >> (8 * 1));
            WriteBytes(buffer);
        }

        void ICoderHeaderWriter.WriteUInt32BE(UInt32 value)
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt32)];
            buffer[0] = (Byte)(value >> (8 * 3));
            buffer[1] = (Byte)(value >> (8 * 2));
            buffer[2] = (Byte)(value >> (8 * 1));
            buffer[3] = (Byte)(value >> (8 * 0));
            WriteBytes(buffer);
        }

        void ICoderHeaderWriter.WriteUInt32LE(UInt32 value)
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt32)];
            buffer[0] = (Byte)(value >> (8 * 0));
            buffer[1] = (Byte)(value >> (8 * 1));
            buffer[2] = (Byte)(value >> (8 * 2));
            buffer[3] = (Byte)(value >> (8 * 3));
            WriteBytes(buffer);
        }

        void ICoderHeaderWriter.WriteUInt64BE(UInt64 value)
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt64)];
            buffer[0] = (Byte)(value >> (8 * 7));
            buffer[1] = (Byte)(value >> (8 * 6));
            buffer[2] = (Byte)(value >> (8 * 5));
            buffer[3] = (Byte)(value >> (8 * 4));
            buffer[4] = (Byte)(value >> (8 * 3));
            buffer[5] = (Byte)(value >> (8 * 2));
            buffer[6] = (Byte)(value >> (8 * 1));
            buffer[7] = (Byte)(value >> (8 * 0));
            WriteBytes(buffer);
        }

        void ICoderHeaderWriter.WriteUInt64LE(UInt64 value)
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt64)];
            buffer[0] = (Byte)(value >> (8 * 0));
            buffer[1] = (Byte)(value >> (8 * 1));
            buffer[2] = (Byte)(value >> (8 * 2));
            buffer[3] = (Byte)(value >> (8 * 3));
            buffer[4] = (Byte)(value >> (8 * 4));
            buffer[5] = (Byte)(value >> (8 * 5));
            buffer[6] = (Byte)(value >> (8 * 6));
            buffer[7] = (Byte)(value >> (8 * 7));
            WriteBytes(buffer);
        }

        void ICoderHeaderWriter.WriteProperty() => _coderPropertySetter();
        public void WriteBytes(ReadOnlySpan<Byte> data) => _bytesDataWriter(data);
        void ICoderHeaderWriter.SetInStreamSize(UInt64 inStreamSize) => _inStreamSize = inStreamSize;
        void ICoderHeaderWriter.SetOutStreamSize(UInt64 outStreamSize) => _outStreamSize = outStreamSize;
    }
}
