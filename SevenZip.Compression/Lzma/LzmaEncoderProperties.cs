using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.Collections.Generic;

namespace SevenZip.Compression.Lzma
{
    /// <summary>
    /// A class of properties that can be specified for the LZMA encoder.
    /// </summary>
    public class LzmaEncoderProperties
        : ICoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public LzmaEncoderProperties()
        {
            Level = null;
            EndMarker = null;
            Affinity = null;
            ReduceSize = null;
            DictionarySize = null;
            LitContextBits = null;
            LitPosBits = null;
            PosStateBits = null;
            Algorithm = null;
            MatchFinder = MatchFinderType.None;
            NumFastBytes = null;
            MatchFinderCycles = null;
            NumThreads = null;
            ExpectedDataSize = null;
        }

        /// <summary>
        /// <para>
        /// It means the level of compression of the encoder and can be specified from <see cref="CompressionLevel.Level1"/> to <see cref="CompressionLevel.Level9"/>.
        /// </para>
        /// <para>
        /// The default value is null, which means <see cref="CompressionLevel.Normal"/> (equivalent to <see cref="CompressionLevel.Level5"/>).
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Generally, the higher the Level, the higher the compression rate, but the longer the time required for compression.
        /// </para>
        /// </remarks>
        public CompressionLevel? Level { get; set; }

        /// <summary>
        /// It means whether to write EOPM (marker indicating the end of LZMA encoded file) at the end of the encoded file.
        /// The default value is null, which means that EOPM is not written.
        /// If you want to change this behavior, set the following values. :
        /// <list type="bullet">
        /// <item>Set true when writing EOPM</item>
        /// <item>Set false to not write EOPM</item>
        /// </list>
        /// </summary>
        public bool? EndMarker { get; set; }

        /// <summary>
        /// <para>
        /// When the encoding is multithreaded, it means a set of bits that explicitly specifies the CPU cores assigned to the threads used.
        /// </para>
        /// <para>
        /// The default value is null, which means that all CPU cores are available.
        /// </para>
        /// </summary>
        public UInt64? Affinity { get; set; }

        /// <summary>
        /// <para>
        /// If the total size of the uncompressed data to be encoded is known, it is the size in bytes.
        /// </para>
        /// <para>
        /// The default value is null, which means that the length of the uncompressed data is unknown.
        /// </para>
        /// </summary>
        public UInt64? ReduceSize { get; set; }

        /// <summary>
        /// <para>
        /// Means the size in bytes of the dictionary used for encoding.
        /// This value should be in the following range:
        /// <list type="bullet">
        /// <item><term>For 32-bit process:</term><description>4KB &lt;= <see cref="DictionarySize"/> &lt;= 128MB</description></item>
        /// <item><term>For 64-bit process:</term><description>4KB &lt;= <see cref="DictionarySize"/> &lt;= 1536MB</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level9"/>:</term><description>64MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level8"/>:</term><description>64MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level7"/>:</term><description>32MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level6"/>:</term><description>32MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level5"/>:</term><description>16MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level4"/>:</term><description>8MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level3"/>:</term><description>4MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level2"/>:</term><description>1MB</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level1"/>:</term><description>256KB</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Under the following conditions, the value of <see cref="ReduceSize"/> is used as <see cref="DictionarySize"/> inside 7-zip:
        /// <list type="number">
        /// <item><see cref="ReduceSize"/> is set and</item>
        /// <item><see cref="ReduceSize"/> &gt;= 4MB and</item>
        /// <item><see cref="DictionarySize"/> &gt; <see cref="ReduceSize"/></item>
        /// </list>
        /// </para>
        /// <para>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </para>
        /// </remarks>
        public UInt64? DictionarySize { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Literal Context Bits used in the encoding.
        /// This value should be in the following range:
        /// <code>
        /// 0 &lt;= <see cref="LitContextBits"/> &lt;= 8
        /// </code>
        /// </para>
        /// <para>
        /// The default value is null, which means 3.
        /// </para>
        /// </summary>
        public UInt32? LitContextBits { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Literal Pos Bits used in the encoding.
        /// This value should be in the following range:
        /// <code>
        /// 0 &lt;= <see cref="LitPosBits"/> &lt;= 4
        /// </code>
        /// </para>
        /// <para>
        /// The default value is null, which means 0.
        /// </para>
        /// </summary>
        public UInt32? LitPosBits { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Pos Bits used in the encoding.
        /// <code>
        /// 0 &lt;= <see cref="PosStateBits"/> &lt;= 4
        /// </code>
        /// </para>
        /// <para>
        /// The default value is null, which means 2.
        /// </para>
        /// </summary>
        public UInt32? PosStateBits { get; set; }

        /// <summary>
        /// <para>
        /// Means the type of algorithm used for encoding.
        /// If its value is 0, the encoding time will be shorter, but the compression ratio will be lower.
        /// If the value is non-zero, the encoding will take longer, but the compression ratio will improve.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <item><term>For <see cref="CompressionLevel.Level1"/> &lt;= <see cref="Level"/> &lt;= <see cref="CompressionLevel.Level4"/>:</term><description>0</description></item>
        /// <item><term>For <see cref="CompressionLevel.Level5"/> &lt;= <see cref="Level"/> &lt;= <see cref="CompressionLevel.Level9"/>:</term><description>1</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public UInt32? Algorithm { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of fast bytes in the encoder.
        /// This value can be set to an integer from 5 to 273.
        /// </para>
        /// <para>
        /// Usually, a big number gives a little bit better compression ratio and a slower compression process.
        /// A large fast bytes parameter can significantly increase the compression ratio for files which contain long identical sequences of bytes.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <item><term>For <see cref="Level"/> >= <see cref="CompressionLevel.Level7"/> :</term><description>64</description></item>
        /// <item><term>Otherwise :</term><description>32</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public UInt32? NumFastBytes { get; set; }

        /// <summary>
        /// <para>
        /// Means the Match Finder algorithm used in the encoding.
        /// </para>
        /// <para>
        ///  The default value is MatchFinderType.None, which means the following values:
        ///  <list type="table">
        ///  <item><term><see cref="Algorithm"/> == 0:</term><description><see cref="MatchFinderType.HC5"/> </description></item>
        ///  <item><term>Otherwise:</term><description><see cref="MatchFinderType.BT4"/> </description></item>
        ///  </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public MatchFinderType MatchFinder { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Match Finder cycles.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <item><term>For <see cref="MatchFinder"/> == <see cref="MatchFinderType.BT2"/>:</term><description>16 + <see cref="NumFastBytes"/> / 2</description></item>
        /// <item><term>For <see cref="MatchFinder"/> == <see cref="MatchFinderType.BT3"/>:</term><description>16 + <see cref="NumFastBytes"/> / 2</description></item>
        /// <item><term>For <see cref="MatchFinder"/> == <see cref="MatchFinderType.BT4"/>:</term><description>16 + <see cref="NumFastBytes"/> / 2</description></item>
        /// <item><term>For <see cref="MatchFinder"/> == <see cref="MatchFinderType.BT5"/>:</term><description>16 + <see cref="NumFastBytes"/> / 2</description></item>
        /// <item><term>For <see cref="MatchFinder"/> == <see cref="MatchFinderType.HC4"/>:</term><description>8 + <see cref="NumFastBytes"/> / 4</description></item>
        /// <item><term>For <see cref="MatchFinder"/> == <see cref="MatchFinderType.HC5"/>:</term><description>8 + <see cref="NumFastBytes"/> / 4</description></item>
        /// </list>
        /// </para>
        /// </summary>
        public UInt32? MatchFinderCycles { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of threads used for encoding.
        /// The values that can be set are as follows.:
        /// <list type="table">
        /// <item>
        /// 1 &lt;= <see cref="NumThreads"/> &lt;= 2
        /// </item>
        /// </list>
        /// </para>
        /// <para>
        ///  The default value is null, which means 2.
        /// </para>
        /// </summary>
        public UInt32? NumThreads { get; set; }

        /// <summary>
        /// <para>
        /// Means the estimated size in bytes of uncompressed data read from the input stream.
        /// </para>
        /// <para>
        /// The default value is null, which means that the length of data read from the input stream is unknown.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value is used for encoder optimization.
        /// </para>
        /// <para>
        /// This value does not necessarily have to match the length of the data that can actually be read.
        /// </para>
        /// </remarks>
        public UInt64? ExpectedDataSize { get; set; }

        IEnumerable<(CoderPropertyId propertyId, object propertryValue)> ICoderProperties.EnumerateProperties()
        {
            if (Level.HasValue) yield return (CoderPropertyId.Level, Level.Value);
            if (EndMarker.HasValue)
                yield return (CoderPropertyId.EndMarker, EndMarker.Value);
            if (Affinity.HasValue)
                yield return (CoderPropertyId.Affinity, Affinity.Value);
            if (ReduceSize.HasValue)
                yield return (CoderPropertyId.ReduceSize, ReduceSize.Value);
            if (DictionarySize.HasValue)
                yield return (CoderPropertyId.DictionarySize, DictionarySize.Value);
            if (LitContextBits.HasValue)
                yield return (CoderPropertyId.LitContextBits, LitContextBits.Value);
            if (LitPosBits.HasValue)
                yield return (CoderPropertyId.LitPosBits, LitPosBits.Value);
            if (PosStateBits.HasValue)
                yield return (CoderPropertyId.PosStateBits, PosStateBits.Value);
            if (Algorithm.HasValue)
                yield return (CoderPropertyId.Algorithm, Algorithm.Value);
            if (MatchFinder != MatchFinderType.None)
                yield return (CoderPropertyId.MatchFinder, MatchFinder);
            if (NumFastBytes.HasValue)
                yield return (CoderPropertyId.NumFastBytes, NumFastBytes.Value);
            if (MatchFinderCycles.HasValue)
                yield return (CoderPropertyId.MatchFinderCycles, MatchFinderCycles.Value);
            if (NumThreads.HasValue)
                yield return (CoderPropertyId.NumThreads, NumThreads.Value);
            if (ExpectedDataSize.HasValue)
                yield return (CoderPropertyId.ExpectedDataSize, ExpectedDataSize.Value); ;
        }
    }
}
