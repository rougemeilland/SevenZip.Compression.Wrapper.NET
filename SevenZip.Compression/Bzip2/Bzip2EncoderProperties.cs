using SevenZip.NativeInterface;
using System;
using System.Collections.Generic;

namespace SevenZip.Compression.Bzip2
{
    /// <summary>
    /// A class of properties that can be specified for the BZIP2 encoder.
    /// </summary>
    public class Bzip2EncoderProperties
        : ICoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Bzip2EncoderProperties()
        {
            Level = null;
            Affinity = null;
            DictionarySize = null;
            NumPasses = null;
            NumThreads = null;
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
        /// Means the number of encoder passes.
        /// This value can be an integer from 1 to 10.
        /// Usually, a big number gives a little bit better compression ratio and a slower compression process.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level9"/>:</term><description>7</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level8"/>:</term><description>2</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level7"/>:</term><description>2</description></item>
        /// <item><term>Otherwise :</term><description>1</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public UInt32? NumPasses { get; set; }

        /// <summary>
        /// <para>
        /// Means the size of the dictionary used for encoding.
        /// This value can be set from 100000 to 900000.
        /// </para>
        /// <para>
        /// The default value is null, which means the following values:
        /// <list type="table">
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level1"/>:</term><description>100000</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level2"/>:</term><description>300000</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level3"/>:</term><description>500000</description></item>
        /// <item><term>For <see cref="Level"/> == <see cref="CompressionLevel.Level4"/>:</term><description>700000</description></item>
        /// <item><term>Otherwise :</term><description>900000</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public UInt32? DictionarySize { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of threads used for encoding.
        /// This value can be set to an integer from 1 to 64.
        /// </para>
        /// <para>
        /// The default value is null, which means 1.
        /// </para>
        /// <para>
        /// When compressing with multiple threads, each thread uses 32MB of memory for buffering.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
        /// </remarks>
        public UInt32? NumThreads { get; set; }

        IEnumerable<(CoderPropertyId propertyId, object propertryValue)> ICoderProperties.EnumerateProperties()
        {
            if (Affinity.HasValue)
                yield return (CoderPropertyId.Affinity, Affinity.Value);
            if (DictionarySize.HasValue)
                yield return (CoderPropertyId.DictionarySize, DictionarySize.Value);
            if (Level.HasValue)
                yield return (CoderPropertyId.Level, Level.Value);
            if (NumPasses.HasValue)
                yield return (CoderPropertyId.NumPasses, NumPasses.Value);
            if (NumThreads.HasValue)
                yield return (CoderPropertyId.NumThreads, NumThreads.Value);
        }
    }
}
