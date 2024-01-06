using System;

namespace SevenZip.Compression.Bzip2
{
    /// <summary>
    /// A container class for BZIP2 decoder properties.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Bzip2DecoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Bzip2DecoderProperties()
        {
            FinishMode = null;
            NumThreads = null;
        }

        /// <summary>
        /// <para>
        /// In decoding, it means whether to decode the entire input stream (full_decoding mode) or only part of the input stream (partial_decoding mode).
        /// </para>
        /// <para>
        /// The default value is null, which means partial_decoding mode.
        /// If you want to change this behavior, set the following values. :
        /// <list type="bullet">
        /// <item>Set true for full_decoding mode.</item>
        /// <item>Set false for partial_decoding mode.</item>
        /// </list>
        /// </para>
        /// </summary>
        public Boolean? FinishMode { get; set; }

        /// <summary>
        /// <para>
        /// Means whether decoding should be done in multithreaded mode.
        /// </para>
        /// <para>
        /// The default value is null, which means single-threaded mode.
        /// If you want to change this behavior, set the following values:
        /// <list type="bullet">
        /// <item>Set to 1 when decoding in single-thread mode.</item>
        /// <item>Set the number of threads used for decoding when decoding in multi-thread mode.</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: The BZip2 decoder only determines if the <see cref="NumThreads"/> value is less than 2 or not.
        /// So, for example, the BZip2 decoder behaves the same when the <see cref="NumThreads"/> value is 2 and 10.
        /// </remarks>
        public UInt32? NumThreads { get; set; }
    }
}
