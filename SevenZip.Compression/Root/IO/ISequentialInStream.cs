using System;

namespace SevenZip.IO
{
    /// <summary>
    /// An interface that can be read from a sequential input stream.
    /// </summary>
    public interface ISequentialInStream
    {
        /// <summary>
        /// Reads data from the input stream and stores it in the specified buffer.
        /// </summary>
        /// <param name="data">
        /// A buffer for storing the data to be read.
        /// </param>
        /// <returns>
        /// The length in bytes of the data actually read.
        /// 0 is returned in any of the following cases:
        /// <list type="bullet">
        /// <item>When the end of the stream is reached and no more can be read. </item>
        /// <item>If the length of <paramref name="data"/> is 0.</item>
        /// </list>
        /// 
        /// </returns>
        Int32 Read(Span<Byte> data);
    }
}
