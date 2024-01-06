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
            NumThreads = null;
            MemUsage = null;
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
        public Boolean? FinishMode { get; set; }

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
        /// <item><term>256KB or less</term><description>2MB + 384KB</description></item>
        /// <item><term>384KB</term><description>4MB + 448KB</description></item>
        /// <item><term>512KB</term><description>4MB + 448KB</description></item>
        /// <item><term>768KB</term><description>6.5MB</description></item>
        /// <item><term>1MB</term><description>8MB + 576KB</description></item>
        /// <item><term>1.5MB</term><description>12MB + 704KB</description></item>
        /// <item><term>2MB</term><description>16MB + 832KB</description></item>
        /// <item><term>3MB</term><description>25MB + 64KB</description></item>
        /// <item><term>4MB</term><description>33MB + 320KB</description></item>
        /// <item><term>6MB</term><description>49MB + 832KB</description></item>
        /// <item><term>8MB</term><description>66MB + 320KB</description></item>
        /// <item><term>12MB</term><description>99MB + 320KB</description></item>
        /// <item><term>16MB</term><description>132MB + 320KB</description></item>
        /// <item><term>24MB</term><description>198MB + 320KB</description></item>
        /// <item><term>32MB</term><description>264MB + 320KB</description></item>
        /// <item><term>48MB</term><description>396MB + 320KB</description></item>
        /// <item><term>64MB or more</term><description>528MB + 320KB</description></item>
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
    }
}
