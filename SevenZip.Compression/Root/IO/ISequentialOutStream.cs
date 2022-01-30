using System;

namespace SevenZip.IO
{
    /// <summary>
    /// An interface that can be written to a sequential output stream.
    /// </summary>
    public interface ISequentialOutStream
    {
        /// <summary>
        /// Writes the data stored in the specified buffer to the output stream.
        /// </summary>
        /// <param name="data">
        /// A buffer that contains the data to be written.
        /// </param>
        /// <returns>
        /// If the length of data is 0, no writing is done and 0 is returned.
        /// If the length of data is greater than 0, at least 1 byte or more is written and the length of the actually written data is returned in bytes.
        /// </returns>
        Int32 Write(ReadOnlySpan<Byte> data);
    }
}
