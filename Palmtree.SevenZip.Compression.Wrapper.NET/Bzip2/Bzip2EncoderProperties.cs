﻿using System;
using System.Collections.Generic;

namespace SevenZip.Compression.Bzip2
{
    /// <summary>
    /// A class of properties that can be specified for the BZIP2 encoder.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Bzip2EncoderProperties
        : ICoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Bzip2EncoderProperties()
        {
            Affinity = null;
            DictionarySize = null;
            Level = null;
            NumPasses = null;
            NumThreads = null;
        }

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
        /// Means the size of the dictionary used for encoding.
        /// This property can be set to a value in the range <c>100,000 &lt;= <see cref="DictionarySize"/> &lt;= 900,000</c>.
        /// </para>
        /// <para>
        /// The default value is null, and the value used in that case depends on the value of the <see cref="Level"/> property.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The default value for <see cref="DictionarySize"/> is determined as follows:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Level"/></term><term>Default value of <see cref="DictionarySize"/></term>
        /// </listheader>
        /// <item><term><see cref="CompressionLevel.Level0"/></term><description>100,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level1"/></term><description>100,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level2"/></term><description>300,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level3"/></term><description>500,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level4"/></term><description>700,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level5"/></term><description>900,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level6"/></term><description>900,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level7"/></term><description>900,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level8"/></term><description>900,000</description></item>
        /// <item><term><see cref="CompressionLevel.Level9"/></term><description>900,000</description></item>
        /// </list>
        /// </remarks>
        public UInt32? DictionarySize { get; set; }

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
        /// <para>
        /// Means the number of encoder passes.
        /// This property can be set to a value in the range <c>1 &lt;= <see cref="NumPasses"/> &lt;= 10</c>.
        /// </para>
        /// <para>
        /// The default value is null, and the value used in that case depends on the value of the <see cref="Level"/> property.
        /// </para>
        /// <para>
        /// Usually, a big number gives a little bit better compression ratio and a slower compression process.
        /// </para>
        /// </summary>
        /// <remarks>
        /// The default value for <see cref="NumPasses"/> is determined as follows:
        /// <list type="table">
        /// <listheader>
        /// <term>Value of <see cref="Level"/></term><term>Default value of <see cref="NumPasses"/></term>
        /// </listheader>
        /// <item><term><see cref="CompressionLevel.Level0"/></term><description>1</description></item>
        /// <item><term><see cref="CompressionLevel.Level1"/></term><description>1</description></item>
        /// <item><term><see cref="CompressionLevel.Level2"/></term><description>1</description></item>
        /// <item><term><see cref="CompressionLevel.Level3"/></term><description>1</description></item>
        /// <item><term><see cref="CompressionLevel.Level4"/></term><description>1</description></item>
        /// <item><term><see cref="CompressionLevel.Level5"/></term><description>1</description></item>
        /// <item><term><see cref="CompressionLevel.Level6"/></term><description>1</description></item>
        /// <item><term><see cref="CompressionLevel.Level7"/></term><description>2</description></item>
        /// <item><term><see cref="CompressionLevel.Level8"/></term><description>2</description></item>
        /// <item><term><see cref="CompressionLevel.Level9"/></term><description>7</description></item>
        /// </list>
        /// </remarks>
        public UInt32? NumPasses { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of threads used for encoding.
        /// This property can be set to a value in the range <c>1 &lt;= <see cref="NumThreads"/> &lt;= 64</c>.
        /// </para>
        /// <para>
        /// The default value is null, which means 1.
        /// </para>
        /// <para>
        /// When compressing with multiple threads, each thread uses 32MB of memory for buffering.
        /// </para>
        /// </summary>
        public UInt32? NumThreads { get; set; }

        /// <inheritdoc/>
        public IEnumerable<(CoderPropertyId propertyId, Object propertryValue)> EnumerateProperties()
        {
            if (Affinity is not null)
                yield return (CoderPropertyId.Affinity, Affinity.Value);
            if (DictionarySize is not null)
                yield return (CoderPropertyId.DictionarySize, DictionarySize.Value);
            if (Level is not null)
                yield return (CoderPropertyId.Level, (UInt32)Level.Value);
            if (NumPasses is not null)
                yield return (CoderPropertyId.NumPasses, NumPasses.Value);
            if (NumThreads is not null)
                yield return (CoderPropertyId.NumThreads, NumThreads.Value);
        }
    }
}
