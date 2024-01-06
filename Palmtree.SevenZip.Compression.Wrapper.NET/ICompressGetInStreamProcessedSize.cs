using System;

namespace SevenZip.Compression
{
    /// <summary>
    /// It is an interface that can get the processed data amount of the input stream of the coder.
    /// </summary>
    public interface ICompressGetInStreamProcessedSize
    {
        /// <summary>
        /// The number of bytes of processed data in the coder's input stream.
        /// </summary>
        UInt64 InStreamProcessedSize { get; }
    }
}
