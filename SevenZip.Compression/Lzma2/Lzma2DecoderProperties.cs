using System;

namespace SevenZip.Compression.Lzma2
{
    /// <summary>
    /// A container class for LZMA decoder properties.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Lzma2DecoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Lzma2DecoderProperties()
        {
            FinishMode = null;
            InBufSize = null;
            OutBufSize = null;
            NumThreads = null;
            MemUsage = null;
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
        public UInt32? OutBufSize { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of threads used by the decoder.
        /// The lower limit that can be set for this property is 1.
        /// The upper limit depends on the value of the <see cref="MemUsage"/> property and the value of the <see cref="Lzma2EncoderProperties.DictionarySize"/> property when the data to be decoded is encoded.
        /// </para>
        /// <para>
        /// The default value is null, which means 1.
        /// </para>
        /// </summary>
        /// <remarks>
        /// If the value of the <see cref="NumThreads"/> property is 2 or greater, the memory shown in the following table is required for each thread.
        /// And the total size of memory required by all threads must be less than or equal to the value of the <see cref="MemUsage"/> property.
        /// <list type="table">
        /// <listheader>
        /// <item><term>Dictionary size of the data to be decoded</term><term>Memory size required for each thread</term></item>
        /// </listheader>
        /// <item><description>256KB or less</description><description>2MB + 384KB</description></item>
        /// <item><description>384KB</description><description>4MB + 448KB</description></item>
        /// <item><description>512KB</description><description>4MB + 448KB</description></item>
        /// <item><description>768KB</description><description>6.5MB</description></item>
        /// <item><description>1MB</description><description>8MB + 576KB</description></item>
        /// <item><description>1.5MB</description><description>12MB + 704KB</description></item>
        /// <item><description>2MB</description><description>16MB + 832KB</description></item>
        /// <item><description>3MB</description><description>25MB + 64KB</description></item>
        /// <item><description>4MB</description><description>33MB + 320KB</description></item>
        /// <item><description>6MB</description><description>49MB + 832KB</description></item>
        /// <item><description>8MB</description><description>66MB + 320KB</description></item>
        /// <item><description>12MB</description><description>99MB + 320KB</description></item>
        /// <item><description>16MB</description><description>132MB + 320KB</description></item>
        /// <item><description>24MB</description><description>198MB + 320KB</description></item>
        /// <item><description>32MB</description><description>264MB + 320KB</description></item>
        /// <item><description>48MB</description><description>396MB + 320KB</description></item>
        /// <item><description>64MB or more</description><description>528MB + 320KB</description></item>
        /// </list>
        /// </remarks>
        public UInt32? NumThreads { get; set; }

        /// <summary>
        /// <para>
        /// Means the memory used by the decoder.
        /// </para>
        /// <para>
        /// The default value is null, which means 256MB.
        /// If you change this value, set the memory size in bytes.
        /// </para>
        /// </summary>
        public UInt64? MemUsage { get; set; }
    }
}
