﻿#if IS_SUPPORTED_SEVENZIP_PPMD8
using System;
using System.Collections.Generic;

namespace SevenZip.Compression.Ppmd8
{
    /// <summary>
    /// A class of properties that can be specified for the PPMd8 (PPMd version I) encoder.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Ppmd8EncoderProperties
        : ICoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Ppmd8EncoderProperties()
        {
            Algorithm = PPMd8RestoreMethod.None;
            Level = null;
            NumThreads = null;
            Order = null;
            ReduceSize = null;
            UsedMemorySize = null;
        }

        /// <summary>
        /// <para>
        /// &lt;unknown&gt;
        /// </para>
        /// <para>
        /// The default value is <see cref="PPMd8RestoreMethod.None"/>, which means the following values:
        /// <list type="table">
        /// <item><term><see cref="Level"/> &lt;= <see cref="CompressionLevel.Level6"/>:</term><description><see cref="PPMd8RestoreMethod.Restart"/></description></item>
        /// <item><term><see cref="Level"/> &gt;= <see cref="CompressionLevel.Level7"/>:</term><description><see cref="PPMd8RestoreMethod.CutOff"/></description></item>
        /// </list>
        /// </para>
        /// </summary>
        public PPMd8RestoreMethod Algorithm { get; set; }

        /// <summary>
        /// <para>
        /// It means the level of compression of the encoder and can be specified from <see cref="CompressionLevel.Level0"/> to <see cref="CompressionLevel.Level9"/>.
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
        /// This value is ignored.
        /// </summary>
        public UInt32? NumThreads { get; set; }

        /// <summary>
        /// <para>
        /// Means the value of the model order of PPMd.
        /// The range of values is <c>2 &lt;= <see cref="Order"/> &lt;= 16</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Level"/></term>Default value of <see cref="Order"/><term></term>
        /// </listheader>
        /// <item><term><see cref="CompressionLevel.Level0"/></term><description>4</description></item>
        /// <item><term><see cref="CompressionLevel.Level1"/></term><description>4</description></item>
        /// <item><term><see cref="CompressionLevel.Level2"/></term><description>5</description></item>
        /// <item><term><see cref="CompressionLevel.Level3"/></term><description>6</description></item>
        /// <item><term><see cref="CompressionLevel.Level4"/></term><description>7</description></item>
        /// <item><term><see cref="CompressionLevel.Level5"/></term><description>8</description></item>
        /// <item><term><see cref="CompressionLevel.Level6"/></term><description>9</description></item>
        /// <item><term><see cref="CompressionLevel.Level7"/></term><description>10</description></item>
        /// <item><term><see cref="CompressionLevel.Level8"/></term><description>11</description></item>
        /// <item><term><see cref="CompressionLevel.Level9"/></term><description>12</description></item>
        /// </list>
        /// </para>
        /// </summary>
        public UInt32? Order { get; set; }

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
        /// Means the size in bytes of memory used in the encoding.
        /// The range of values is <c>1MB &lt;= <see cref="UsedMemorySize"/> &lt;= 256MB</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Level"/></term>Default value of <see cref="UsedMemorySize"/><term></term>
        /// </listheader>
        /// <item><term><see cref="CompressionLevel.Level0"/></term><description>1MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level1"/></term><description>1MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level2"/></term><description>2MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level3"/></term><description>4MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level4"/></term><description>8MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level5"/></term><description>16MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level6"/></term><description>32MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level7"/></term><description>64MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level8"/></term><description>128MB</description></item>
        /// <item><term><see cref="CompressionLevel.Level9"/></term><description>256MB</description></item>
        /// </list>
        /// </para>
        /// </summary>
        public UInt32? UsedMemorySize { get; set; }

        IEnumerable<(CoderPropertyId propertyId, Object propertryValue)> ICoderProperties.EnumerateProperties()
        {
            if (Algorithm != PPMd8RestoreMethod.None)
                yield return (CoderPropertyId.Algorithm, (UInt32)Algorithm);
            if (Level is not null)
                yield return (CoderPropertyId.Level, (UInt32)Level.Value);
            if (NumThreads is not null)
                yield return (CoderPropertyId.NumThreads, NumThreads.Value);
            if (Order is not null)
                yield return (CoderPropertyId.Order, Order.Value);
            if (ReduceSize is not null)
                yield return (CoderPropertyId.ReduceSize, ReduceSize.Value);
            if (UsedMemorySize is not null)
                yield return (CoderPropertyId.UsedMemorySize, UsedMemorySize.Value);
        }
    }
}
#endif
