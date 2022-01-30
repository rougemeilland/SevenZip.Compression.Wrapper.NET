using System;

namespace SevenZip.IO
{
    /// <summary>
    /// An interface that allows you to configure the coder header.
    /// </summary>
    public interface ICoderHeaderWriter
    {
        /// <summary>
        /// Writes 1 byte of data to the output stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Byte"/> value to write.
        /// </param>
        void WriteByte(Byte value);

        /// <summary>
        /// Writes a 16-bit integer in little endian format to the output stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="UInt16"/> value to write.
        /// </param>
        void WriteUInt16LE(UInt16 value);

        /// <summary>
        /// Writes a 16-bit integer in big endian format to the output stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="UInt16"/> value to write.
        /// </param>
        void WriteUInt16BE(UInt16 value);

        /// <summary>
        /// Writes a 32-bit integer in little endian format to the output stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="UInt32"/> value to write.
        /// </param>
        void WriteUInt32LE(UInt32 value);

        /// <summary>
        /// Writes a 32-bit integer in big endian format to the output stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="UInt32"/> value to write.
        /// </param>
        void WriteUInt32BE(UInt32 value);

        /// <summary>
        /// Writes a 64-bit integer in little endian format to the output stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="UInt64"/> value to write.
        /// </param>

        void WriteUInt64LE(UInt64 value);

        /// <summary>
        /// Writes a 64-bit integer in big endian format to the output stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="UInt64"/> value to write.
        /// </param>
        void WriteUInt64BE(UInt64 value);

        /// <summary>
        /// Writes byte data to the output stream.
        /// </summary>
        /// <param name="data">
        /// This is the data to write.
        /// </param>
        void WriteBytes(ReadOnlySpan<Byte> data);

        /// <summary>
        /// Gets the coder's properties and writes its byte data to the output stream.
        /// </summary>
        /// <remarks>
        /// The length of the byte data in the coder properties depends on the coder.
        /// </remarks>
        void WriteProperty();

        /// <summary>
        /// It specifies the length of the input stream.
        /// </summary>
        /// <param name="inStreamSize">
        /// The length in bytes of the input stream.
        /// </param>
        void SetInStreamSize(UInt64 inStreamSize);

        /// <summary>
        /// It specifies the length of the output stream.
        /// </summary>
        /// <param name="outStreamSize">
        /// The length in bytes of the output stream.
        /// </param>
        void SetOutStreamSize(UInt64 outStreamSize);
    }
}
