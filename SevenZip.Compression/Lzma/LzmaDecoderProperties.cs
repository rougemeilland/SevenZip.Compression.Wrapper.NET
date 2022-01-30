using System;

namespace SevenZip.Compression.Lzma
{
    /// <summary>
    /// A container class for LZMA decoder properties.
    /// </summary>
    public class LzmaDecoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public LzmaDecoderProperties()
        {
            FinishMode = null;
            InBufSize = null;
            OutBufSize = null;
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
        public bool? FinishMode { get; set; }

        /// <summary>
        /// <para>
        /// Means the size of the input buffer used in decoding.
        /// </para>
        /// <para>
        /// By default it is set to null, which means 1MB.
        /// If you want to change this value, set the size of the input buffer in bytes.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public UInt32? InBufSize { get; set; }

        /// <summary>
        /// <para>
        /// Means the size of the output buffer used in decoding.
        /// </para>
        /// <para>
        /// By default it is set to null, which means 1MB.
        /// If you want to change this value, set the size of the output buffer in bytes.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public UInt32? OutBufSize { get; set; }
    }
}
