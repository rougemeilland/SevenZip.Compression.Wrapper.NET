using System;

namespace SevenZip.Compression
{
    /// <summary>
    /// An enumeration that shows how high the encoder compression ratio is.
    /// The value of this enumeration is expressed in 9 steps from Level 1 to Level 9.
    /// </summary>
    public enum CompressionLevel
        : UInt32
    {
        /// <summary>
        /// The encoder compression ratio is Level 0.
        /// </summary>
        Level0 = 0,

        /// <summary>
        /// The encoder compression ratio is Level 1.
        /// </summary>
        Level1 = 1,

        /// <summary>
        /// The encoder compression ratio is Level 2.
        /// </summary>
        Level2 = 2,

        /// <summary>
        /// The encoder compression ratio is Level 3.
        /// </summary>
        Level3 = 3,

        /// <summary>
        /// The encoder compression ratio is Level 4.
        /// </summary>
        Level4 = 4,

        /// <summary>
        /// The encoder compression ratio is Level 5.
        /// </summary>
        Level5 = 5,

        /// <summary>
        /// The encoder compression ratio is Level 6.
        /// </summary>
        Level6 = 6,

        /// <summary>
        /// The encoder compression ratio is Level 7.
        /// </summary>
        Level7 = 7,

        /// <summary>
        /// The encoder compression ratio is Level 8.
        /// </summary>
        Level8 = 8,

        /// <summary>
        /// The encoder compression ratio is Level 9.
        /// </summary>
        Level9 = 9,

        /// <summary>
        /// The fastest compression.
        /// This is equivalent to Level 1.
        /// </summary>
        Fastest = Level0,

        /// <summary>
        /// Fast compression.
        /// This is equivalent to Level 3.
        /// </summary>
        Fast = Level3,

        /// <summary>
        /// Normal compression.
        /// This is equivalent to Level 5.
        /// </summary>
        Normal = Level5,

        /// <summary>
        /// Maximum compression.
        /// This is equivalent to Level 7.
        /// </summary>
        Maximum = Level7,

        /// <summary>
        /// Ultra compression.
        /// This is equivalent to Level 9.
        /// </summary>
        Ultra = Level9,
    }
}
