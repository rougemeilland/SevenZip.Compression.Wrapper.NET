using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.Collections.Generic;

namespace SevenZip.Compression.Ppmd7
{
    /// <summary>
    /// A class of properties that can be specified for the PPMd7 (PPMd version H) encoder.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Ppmd7EncoderProperties
        : ICoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Ppmd7EncoderProperties()
        {
            ReduceSize = null;
            UsedMemorySize = null;
            Order = null;
            NumThreads = null;
            Level = null;
        }

        /// <summary>
        /// <para>
        /// If the total size of the uncompressed data to be encoded is known, it is the size in bytes.
        /// </para>
        /// <para>
        /// The default value is null, which means that the length of the uncompressed data is unknown.
        /// </para>
        /// </summary>
        public UInt64? ReduceSize { get; set; }

        // ReduceSizeが指定されている場合、上限値は ReduceSize の影響を受ける。(1 << i) / 16 >= ReduceSize を満たす最小の(1 << i)がUsedMemorySizeの上限値となる。(ただし 16 <= i <= 31)
        /// <summary>
        /// <para>
        /// Means the size of memory used by PPMd.
        /// This property can be a multiple of 4 and set to a value in the range <c>64KB &lt;= <see cref="UsedMemorySize"/> &lt;= 4GB</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Level"/></term>Default value of <see cref="Order"/><term></term>
        /// </listheader>
        /// <item><description><see cref="CompressionLevel.Level1"/></description><description>1MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level2"/></description><description>2MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level3"/></description><description>4MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level4"/></description><description>8MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level5"/></description><description>16MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level6"/></description><description>32MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level7"/></description><description>64MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level8"/></description><description>128MB</description></item>
        /// <item><description><see cref="CompressionLevel.Level9"/></description><description>256MB</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// 
        public UInt64? UsedMemorySize { get; set; } // , MemSize / 16 > ReduceSize の場合は ReduceSize <= 1 << i (12 <= i <= 27)を満たす最小のiに対し 1 << 1 がUsedMemorySizeとなる

        /// <summary>
        /// <para>
        /// Means the model order for PPMd.
        /// The range of values that can be set is <c>2 &lt;= Order &lt;= 32</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Level"/></term>Default value of <see cref="Order"/><term></term>
        /// </listheader>
        /// <item><description><see cref="CompressionLevel.Level1"/></description><description>4</description></item>
        /// <item><description><see cref="CompressionLevel.Level2"/></description><description>4</description></item>
        /// <item><description><see cref="CompressionLevel.Level3"/></description><description>5</description></item>
        /// <item><description><see cref="CompressionLevel.Level4"/></description><description>5</description></item>
        /// <item><description><see cref="CompressionLevel.Level5"/></description><description>6</description></item>
        /// <item><description><see cref="CompressionLevel.Level6"/></description><description>8</description></item>
        /// <item><description><see cref="CompressionLevel.Level7"/></description><description>16</description></item>
        /// <item><description><see cref="CompressionLevel.Level8"/></description><description>24</description></item>
        /// <item><description><see cref="CompressionLevel.Level9"/></description><description>32</description></item>
        /// </list>
        /// </para>
        /// </summary>
        public UInt32? Order { get; set; }

        /// <summary>
        /// The value of this property is ignored.
        /// </summary>
        public UInt32? NumThreads { get; set; }

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

        IEnumerable<(CoderPropertyId propertyId, object propertryValue)> ICoderProperties.EnumerateProperties()
        {
            if (ReduceSize.HasValue)
                yield return (CoderPropertyId.ReduceSize, ReduceSize.Value);
            if (UsedMemorySize.HasValue)
                yield return (CoderPropertyId.UsedMemorySize, UsedMemorySize.Value);
            if (Order.HasValue)
                yield return (CoderPropertyId.Order, Order.Value);
            if (NumThreads.HasValue)
                yield return (CoderPropertyId.NumThreads, NumThreads.Value);
            if (Level.HasValue)
                yield return (CoderPropertyId.Level, (UInt32)Level.Value);
        }
    }
}
