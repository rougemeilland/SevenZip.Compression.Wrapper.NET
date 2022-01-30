using System;

namespace SevenZip.IO
{
    /// <summary>
    /// An interface that serves as a data source for parsing coder headers.
    /// </summary>
    public interface ICoderHeaderReader
    {
        /// <summary>
        /// Reads 1 byte of data from the input stream.
        /// </summary>
        /// <returns>
        /// The read <see cref="Byte"/> value.
        /// </returns>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        Byte ReadByte();

        /// <summary>
        /// Read a little endian 16-bit integer from the input stream
        /// </summary>
        /// <returns>
        /// The read <see cref="UInt16"/> value.
        /// </returns>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        UInt16 ReadUInt16LE();

        /// <summary>
        /// Read a big endian 16-bit integer from the input stream
        /// </summary>
        /// <returns>
        /// The read <see cref="UInt16"/> value.
        /// </returns>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        UInt16 ReadUInt16BE();

        /// <summary>
        /// Read a little endian 32-bit integer from the input stream
        /// </summary>
        /// <returns>
        /// The read <see cref="UInt32"/> value.
        /// </returns>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        UInt32 ReadUInt32LE();

        /// <summary>
        /// Read a big endian 32-bit integer from the input stream
        /// </summary>
        /// <returns>
        /// The read <see cref="UInt32"/> value.
        /// </returns>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        UInt32 ReadUInt32BE();

        /// <summary>
        /// Read a little endian 64-bit integer from the input stream
        /// </summary>
        /// <returns>
        /// The read <see cref="UInt64"/> value.
        /// </returns>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        UInt64 ReadUInt64LE();

        /// <summary>
        /// Read a big endian 64-bit integer from the input stream
        /// </summary>
        /// <returns>
        /// The read <see cref="UInt64"/> value.
        /// </returns>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        UInt64 ReadUInt64BE();

        /// <summary>
        /// Reads the specified length of byte data from the input stream.
        /// </summary>
        /// <param name="buffer">
        /// It is a buffer for storing the read data.
        /// </param>
        /// <remarks>
        /// This method attempts to read as much data as the length of the <paramref name="buffer"/> from the input stream.
        /// If the end of the input stream is reached during reading, an exception will be notified.
        /// </remarks>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        void ReadBytes(Span<Byte> buffer);

        /// <summary>
        /// Reads the coder property data from the input stream and sets it in the coder.
        /// </summary>
        /// <exception cref="UnexpectedEndOfStreamException">
        /// The data could not be read because the stream reached the end while reading the data from the input stream.
        /// </exception>
        /// <remarks>
        /// The length of the coder property data depends on the coder.
        /// </remarks>
        void ReadProperty();

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
