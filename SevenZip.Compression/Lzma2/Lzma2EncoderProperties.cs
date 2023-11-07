using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.Collections.Generic;

namespace SevenZip.Compression.Lzma2
{
    /// <summary>
    /// A class of properties that can be specified for the LZMA2 encoder.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Lzma2EncoderProperties
        : ICoderProperties
    {
        /// <summary>
        /// A constant set to <see cref="BlockSize"/> when treating the entire data as one block (solid mode).
        /// </summary>
        public const UInt64 BlockSizeSolid = UInt64.MaxValue;

        /// <summary>
        /// The default constructor.
        /// </summary>
        public Lzma2EncoderProperties()
        {
            BlockSize = null;
            NumThreads = null;
            Level = null;
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
            ExpectedDataSize = null;
        }

        /// <summary>
        /// <para>
        /// Means the number of bytes in the size of the block to encode.
        /// If the constant <see cref="BlockSizeSolid"/> is set, all the data to be encoded will be treated as one block.
        /// </para>
        /// <para>
        /// The default value is null, and the value used in that case depends on the value of the <see cref="DictionarySize"/> property.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The default value for <see cref="BlockSize"/> is determined as follows:
        /// <list type="number">
        /// <item>
        /// <description>
        /// <para>
        /// <c><see cref="BlockSize"/> = ⌈<see cref="DictionarySize"/> * 4 / 1MB ⌉ * 1MB</c>;
        /// </para>
        /// <para>
        /// (Note: <c>⌈X⌉</c> means the smallest integer greater than or equal to x.)
        /// </para>
        /// </description>
        /// </item>
        /// <item>
        /// <description><c>if (<see cref="BlockSize"/> &lt; 1MB) <see cref="BlockSize"/> = 1MB;</c></description>
        /// </item>
        /// <item>
        /// <description><c>if (<see cref="BlockSize"/> &gt; 256MB) <see cref="BlockSize"/> = 256MB</c>;</description>
        /// </item>
        /// <item>
        /// <description><c>if (<see cref="BlockSize"/> &lt; <see cref="DictionarySize"/>) <see cref="BlockSize"/> = <see cref="DictionarySize"/>;</c></description>
        /// </item>
        /// </list>
        /// </remarks>
        public UInt64? BlockSize { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of threads used for encoding.
        /// The values that can be set depend on <see cref="Algorithm"/> and <see cref="MatchFinder"/> values.
        /// </para>
        /// <para>
        /// The default value is null, which means that it depends on the  <see cref="Algorithm"/> and <see cref="MatchFinder"/> values.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// See the following table for the default values for <see cref="NumThreads"/> and the range of values that can be set.
        /// <list type="bullet">
        /// <item>
        /// <term>For <see cref="Algorithm"/> == 0 :</term>
        /// <description>
        /// <list type="table">
        /// <listheader>
        /// <term>Default value</term>
        /// <term>Minimum Value</term>
        /// <term>Maximum Value</term>
        /// </listheader>
        /// <item><description>1</description><description>1</description><description>64</description></item>
        /// </list>
        /// </description>
        /// </item>
        /// <item>
        /// <term>For <see cref="Algorithm"/> != 0 :</term>
        /// <description>
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="MatchFinder"/></term>
        /// <term>Default value</term>
        /// <term>Minimum Value</term>
        /// <term>Maximum Value</term>
        /// </listheader>
        /// <item><description><see cref="MatchFinderType.BT2"/></description><description>2</description><description>2</description><description>128</description></item>
        /// <item><description><see cref="MatchFinderType.BT3"/></description><description>2</description><description>2</description><description>128</description></item>
        /// <item><description><see cref="MatchFinderType.BT4"/></description><description>2</description><description>2</description><description>128</description></item>
        /// <item><description><see cref="MatchFinderType.BT5"/></description><description>2</description><description>2</description><description>128</description></item>
        /// <item><description><see cref="MatchFinderType.HC4"/></description><description>1</description><description>1</description><description>64</description></item>
        /// <item><description><see cref="MatchFinderType.HC5"/></description><description>1</description><description>1</description><description>64</description></item>
        /// </list>
        /// </description>
        /// </item>
        /// </list>
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If <see cref="BlockSize"/> is set to the constant <see cref="BlockSizeSolid"/>, the encoding will be performed in a single thread regardless of the value of <see cref="NumThreads"/>.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If the value of <see cref="ReduceSize"/> is explicitly set and <c><see cref="ReduceSize"/> &gt; <see cref="NumThreads"/> * <see cref="BlockSize"/></c>, then <c>⌈<see cref="ReduceSize"/> / <see cref="BlockSize"/>⌉</c> is used as the value of NumThread.
        /// (Reference: ⌈ x ⌉ means the smallest integer greater than or equal to x.)
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        public UInt32? NumThreads { get; set; }

        /// <summary>
        /// <para>
        /// Means the Match Finder algorithm used in the encoding.
        /// </para>
        /// <para>
        /// The default value is null, and the value used in that case depends on the value of the <see cref="Level"/> property.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default value is MatchFinderType.None, which means the following values:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Algorithm"/></term><term>Default value of <see cref="MatchFinder"/></term>
        /// </listheader>
        /// <item><description>0</description><description><see cref="MatchFinderType.HC5"/></description></item>
        /// <item><description>(Otherwise)</description><description><see cref="MatchFinderType.BT4"/></description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public MatchFinderType MatchFinder { get; set; }

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
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// Under the following conditions, the value of <see cref="ReduceSize"/> is used as <see cref="DictionarySize"/> inside 7-zip:
        /// <list type="number">
        /// <item><see cref="ReduceSize"/> is set and</item>
        /// <item><see cref="ReduceSize"/> &gt;= 4MB and</item>
        /// <item><see cref="DictionarySize"/> &gt; <see cref="ReduceSize"/></item>
        /// </list>
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// If BlockSize is explicitly set and <c><see cref="DictionarySize"/> &gt; <see cref="BlockSize"/></c>, the value of <see cref="BlockSize"/> will be used instead as the value of <see cref="DictionarySize"/>.
        /// </description>
        /// </item>
        /// </list>
        /// </remarks>
        public UInt64? DictionarySize { get; set; }

        /// <summary>
        /// <para>
        /// It means the level of compression of the encoder and can be specified from <see cref="CompressionLevel.Level1"/> to <see cref="CompressionLevel.Level9"/>.
        /// </para>
        /// <para>
        /// The default value is null, which means <see cref="CompressionLevel.Normal"/> (equivalent to <see cref="CompressionLevel.Level5"/>).
        /// </para>
        /// </summary>
        /// <remarks>
        /// Generally, the higher the Level, the higher the compression rate, but the longer the time required for compression.
        /// </remarks>
        public CompressionLevel? Level { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of fast bytes in the encoder.
        /// This property can be set to a value in the range <c>5 &lt;= <see cref="NumFastBytes"/> &lt;= 273</c>.
        /// </para>
        /// <para>
        /// The default value is null, and the value used in that case depends on the value of the <see cref="Level"/> property.
        /// </para>
        /// <para>
        /// Usually, a big number gives a little bit better compression ratio and a slower compression process.
        /// A large fast bytes parameter can significantly increase the compression ratio for files which contain long identical sequences of bytes.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The default value for <see cref="NumFastBytes"/> is determined as follows:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Level"/></term><term>Default value of <see cref="NumFastBytes"/></term>
        /// </listheader>
        /// <item><description><see cref="CompressionLevel.Level9"/></description><description>64</description></item>
        /// <item><description><see cref="CompressionLevel.Level8"/></description><description>64</description></item>
        /// <item><description><see cref="CompressionLevel.Level7"/></description><description>64</description></item>
        /// <item><description>(Otherwise)</description><description>32</description></item>
        /// </list>
        /// </remarks>
        public UInt32? NumFastBytes { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Match Finder cycles.
        /// This property can be set to a value in the range <c>1 &lt;= <see cref="MatchFinderCycles"/> &lt;= (1 &lt;&lt; 30)</c>.
        /// </para>
        /// <para>
        /// The default value is null, and the value used in that case depends on the value of the <see cref="MatchFinder"/> property.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The default value for <see cref="MatchFinderCycles"/> is determined as follows:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="MatchFinder"/></term><term>Default value of <see cref="MatchFinderCycles"/></term>
        /// </listheader>
        /// <item><description><see cref="MatchFinderType.BT2"/></description><description><c>16 + <see cref="NumFastBytes"/> / 2</c></description></item>
        /// <item><description><see cref="MatchFinderType.BT3"/></description><description><c>16 + <see cref="NumFastBytes"/> / 2</c></description></item>
        /// <item><description><see cref="MatchFinderType.BT4"/></description><description><c>16 + <see cref="NumFastBytes"/> / 2</c></description></item>
        /// <item><description><see cref="MatchFinderType.BT5"/></description><description><c>16 + <see cref="NumFastBytes"/> / 2</c></description></item>
        /// <item><description><see cref="MatchFinderType.HC4"/></description><description><c>8 + <see cref="NumFastBytes"/> / 4</c></description></item>
        /// <item><description><see cref="MatchFinderType.HC5"/></description><description><c>8 + <see cref="NumFastBytes"/> / 4</c></description></item>
        /// </list>
        /// </remarks>
        public UInt32? MatchFinderCycles { get; set; }

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
        public UInt32? Algorithm { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Pos Bits used in the encoding.
        /// This property can be set to a value in the range <c>0 &lt;= <see cref="PosStateBits"/> &lt;= 4</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means 2.
        /// </para>
        /// </summary>
        public UInt32? PosStateBits { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Literal Pos Bits used in the encoding.
        /// This property can be set to a value in the range <c>0 &lt;= <see cref="LitPosBits"/> &lt;= 4</c>.
        /// However, it must be <c><see cref="LitPosBits"/> + <see cref="LitContextBits"/> &lt;= 4</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means 0.
        /// </para>
        /// </summary>
        public UInt32? LitPosBits { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of Literal Context Bits used in the encoding.
        /// This property can be set to a value in the range <c>0 &lt;= <see cref="LitContextBits"/> &lt;= 4</c>.
        /// However, it must be <c><see cref="LitPosBits"/> + <see cref="LitContextBits"/> &lt;= 4</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means 3.
        /// </para>
        /// </summary>
        public UInt32? LitContextBits { get; set; }

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
            if (BlockSize.HasValue)
                yield return (CoderPropertyId.Level, BlockSize.Value);
            if (NumThreads.HasValue)
                yield return (CoderPropertyId.NumThreads, NumThreads.Value);
            if (Level.HasValue)
                yield return (CoderPropertyId.Level, (UInt32)Level.Value);
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
            if (ExpectedDataSize.HasValue)
                yield return (CoderPropertyId.ExpectedDataSize, ExpectedDataSize.Value); ;
        }
    }
}
