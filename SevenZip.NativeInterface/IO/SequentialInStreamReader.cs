using System;

namespace SevenZip.NativeInterface.IO
{
    /// <summary>
    /// A delegate for a function that reads data from a stream.
    /// </summary>
    /// <param name="buffer">
    /// Set a buffer to read the data.
    /// </param>
    /// <returns>
    /// The length in bytes of the data actually read is returned.
    /// If the stream has reached the end and can no longer be read, 0 is returned. ..
    /// </returns>
    /// <remarks>
    /// <para>
    /// If buffer.Length is 0, no data will be read and 0 will be returned.
    /// </para>
    /// <para>
    /// If buffer.Length is greater than 0, data less than or equal to buffer.Length bytes is read from the input stream and the length in bytes of the data actually read is returned.
    /// At this time, if the end of the input stream is reached and no more data can be read, 0 is returned.
    /// </para>
    /// </remarks>  
    public delegate Int32 SequentialInStreamReader(Span<Byte> buffer);
}
