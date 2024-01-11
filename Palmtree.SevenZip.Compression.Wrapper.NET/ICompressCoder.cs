using System;
using Palmtree.IO;

namespace SevenZip.Compression
{
    /// <summary>
    /// An interface that provides a way to code between streams.
    /// </summary>
    public interface ICompressCoder
    {
        /// <summary>
        /// Coding from one stream to another.
        /// </summary>
        /// <param name="sequentialInStream">
        /// The input stream for coder.
        /// See <see cref="ISequentialInputByteStream"/> for more information.
        /// </param>
        /// <param name="sequentialOutStream">
        /// The output stream for coder.
        /// See <see cref="ISequentialOutputByteStream"/> for more information.
        /// </param>
        /// <param name="inSize">
        /// Gives the length of the input stream in bytes.
        /// If you omit the length specification, give null instead.
        /// </param>
        /// <param name="outSize">
        /// Gives the length of the output stream in bytes.
        /// If you omit the length specification, give null instead.
        /// </param>
        /// <param name="progress">
        /// An object for receiving input and output progress notifications.
        /// Null if no progress notifications should be received.
        /// </param>
        void Code(ISequentialInputByteStream sequentialInStream, ISequentialOutputByteStream sequentialOutStream, UInt64? inSize, UInt64? outSize, IProgress<(UInt64 inStreamProcessedCount, UInt64 outStreamProcessedCount)>? progress);
    }
}
