using System;
using System.IO;
using SevenZip.NativeInterface.IO;

namespace SevenZip.IO
{
    class CoderHeaderReader
        : ICoderHeaderReader
    {
        private delegate void CoderPropertySetter();

        private SequentialInStreamReader _bytesDataReader;
        private CoderPropertySetter _coderPropertySetter;
        private UInt64? _inStreamSize;
        private UInt64? _outStreamSize;

        private CoderHeaderReader(SequentialInStreamReader bytesDataReader, CoderPropertySetter coderPropertySetter)
        {
            _bytesDataReader = bytesDataReader;
            _coderPropertySetter = coderPropertySetter;
            _inStreamSize = null;
            _outStreamSize = null;
        }

        public UInt64? InStreamSize => _inStreamSize;
        public UInt64? OutStreamSize => _outStreamSize;

        public static CoderHeaderReader Create(SequentialInStreamReader inStreamReader, Action coderPropertySetter) =>
            new CoderHeaderReader(
                inStreamReader,
                () => coderPropertySetter());

        Byte ICoderHeaderReader.ReadByte()
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(Byte)];
            ReadBytes(buffer);
            return buffer[0];
        }

        UInt16 ICoderHeaderReader.ReadUInt16BE()
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt16)];
            ReadBytes(buffer);
            return
                (UInt16)(
                    buffer[0] << (8 * 1)
                    | buffer[1] << (8 * 0));
        }

        UInt16 ICoderHeaderReader.ReadUInt16LE()
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt16)];
            ReadBytes(buffer);
            return
                (UInt16)(
                    buffer[0] << (8 * 0)
                    | buffer[1] << (8 * 1));
        }

        UInt32 ICoderHeaderReader.ReadUInt32BE()
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt32)];
            ReadBytes(buffer);
            return
                (UInt32)buffer[0] << (8 * 3)
                | (UInt32)buffer[1] << (8 * 2)
                | (UInt32)buffer[2] << (8 * 1)
                | (UInt32)buffer[3] << (8 * 0);
        }

        UInt32 ICoderHeaderReader.ReadUInt32LE()
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt32)];
            ReadBytes(buffer);
            return
                (UInt32)buffer[0] << (8 * 0)
                | (UInt32)buffer[1] << (8 * 1)
                | (UInt32)buffer[2] << (8 * 2)
                | (UInt32)buffer[3] << (8 * 3);
        }

        UInt64 ICoderHeaderReader.ReadUInt64BE()
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt64)];
            ReadBytes(buffer);
            return
                (UInt64)buffer[0] << (8 * 7)
                | (UInt64)buffer[1] << (8 * 6)
                | (UInt64)buffer[2] << (8 * 5)
                | (UInt64)buffer[3] << (8 * 4)
                | (UInt64)buffer[4] << (8 * 3)
                | (UInt64)buffer[5] << (8 * 2)
                | (UInt64)buffer[6] << (8 * 1)
                | (UInt64)buffer[7] << (8 * 0);
        }

        UInt64 ICoderHeaderReader.ReadUInt64LE()
        {
            Span<Byte> buffer = stackalloc Byte[sizeof(UInt64)];
            ReadBytes(buffer);
            return
                (UInt64)buffer[0] << (8 * 0)
                | (UInt64)buffer[1] << (8 * 1)
                | (UInt64)buffer[2] << (8 * 2)
                | (UInt64)buffer[3] << (8 * 3)
                | (UInt64)buffer[4] << (8 * 4)
                | (UInt64)buffer[5] << (8 * 5)
                | (UInt64)buffer[6] << (8 * 6)
                | (UInt64)buffer[7] << (8 * 7);
        }

        void ICoderHeaderReader.ReadProperty() => _coderPropertySetter();
        public void ReadBytes(Span<Byte> buffer) => _bytesDataReader.ReadBytes(buffer);
        void ICoderHeaderReader.SetInStreamSize(UInt64 inStreamSize) => _inStreamSize = inStreamSize;
        void ICoderHeaderReader.SetOutStreamSize(UInt64 outStreamSize) => _outStreamSize = outStreamSize;
    }
}
