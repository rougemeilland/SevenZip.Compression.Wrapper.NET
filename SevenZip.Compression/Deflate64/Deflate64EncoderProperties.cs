using SevenZip.NativeInterface;
using System;
using System.Collections.Generic;

namespace SevenZip.Compression.Deflate64
{
    /// <summary>
    /// A class of properties that can be specified for the Deflate64 encoder.
    /// </summary>
    /// <remarks>
    /// Note: This specification is based on 7-Zip 21.07 and is subject to change in future versions.
    /// </remarks>
    public class Deflate64EncoderProperties
        : ICoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Deflate64EncoderProperties()
        {
            Level = null;
            NumPasses = null;
            NumFastBytes = null;
            MatchFinderCycles = null;
            Algorithm = null;
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
        /// Means the number of encoder passes.
        /// This property can be set to a value in the range <c>1 &lt;= <see cref="NumPasses"/> &lt;= 15</c>.
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
        /// <item><description><see cref="CompressionLevel.Level9"/></description><description>10</description></item>
        /// <item><description><see cref="CompressionLevel.Level8"/></description><description>3</description></item>
        /// <item><description><see cref="CompressionLevel.Level7"/></description><description>3</description></item>
        /// <item><description>(Otherwise)</description><description>1</description></item>
        /// </list>
        /// </remarks>
        public UInt32? NumPasses { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of fast bytes in the encoder.
        /// This property can be set to a value in the range <c>3 &lt;= <see cref="NumFastBytes"/> &lt;= 258</c>.
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
        /// <item><description><see cref="CompressionLevel.Level9"/></description><description>128</description></item>
        /// <item><description><see cref="CompressionLevel.Level8"/></description><description>64</description></item>
        /// <item><description><see cref="CompressionLevel.Level7"/></description><description>64</description></item>
        /// <item><description>(Otherwise)</description><description>32</description></item>
        /// </list>
        /// </remarks>
        public UInt32? NumFastBytes { get; set; }

        /// <summary>
        /// <para>
        /// Means the number of match finder cycles.
        /// </para>
        /// <para>
        /// The default value is null, which means (15 + <see cref="NumFastBytes"/> / 2).
        /// </para>
        /// </summary>
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
        /// This value is ignored.
        /// </summary>
        public UInt32? NumThreads { get; set; }

        IEnumerable<(CoderPropertyId propertyId, object propertryValue)> ICoderProperties.EnumerateProperties()
        {
            if (Level.HasValue)
                yield return (CoderPropertyId.Level, (UInt32)Level.Value);
            if (NumPasses.HasValue)
                yield return (CoderPropertyId.NumPasses, NumPasses.Value);
            if (NumFastBytes.HasValue)
                yield return (CoderPropertyId.NumFastBytes, NumFastBytes.Value);
            if (MatchFinderCycles.HasValue)
                yield return (CoderPropertyId.MatchFinderCycles, MatchFinderCycles.Value);
            if (Algorithm.HasValue)
                yield return (CoderPropertyId.Algorithm, Algorithm.Value);
            if (NumThreads.HasValue)
                yield return (CoderPropertyId.NumThreads, NumThreads.Value); ;
        }
    }
}
