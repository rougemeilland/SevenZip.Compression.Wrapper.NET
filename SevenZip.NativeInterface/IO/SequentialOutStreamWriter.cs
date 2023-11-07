using System;

namespace SevenZip.NativeInterface.IO
{
    /// <summary>
    /// A delegate for a function that writes data to a stream.
    /// </summary>
    /// <param name="data">
    /// Set the buffer that stores the data to be written.
    /// </param>
    /// <returns>
    /// Returns the byte length of the data that could actually be written.
    /// </returns>
    /// <remarks>
    /// <para>
    /// If buffer.Length is 0, no data is written and 0 is returned.
    /// </para>
    /// <para>
    /// If buffer.Length is greater than 0, data of 1 byte or more and buffer.Length bytes or less is written to the output stream, and the length in bytes of the data actually written is returned.
    /// </para>
    /// </remarks>
    public delegate Int32 SequentialOutStreamWriter(ReadOnlySpan<Byte> data);
}
