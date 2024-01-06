namespace SevenZip.Compression
{
    /// <summary>
    /// An enumeration that indicates the type of match finder used by the encoder.
    /// </summary>
    public enum MatchFinderType
    {
        /// <summary>
        /// The matchfinder type is not specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents a matchfinder that uses a 32-bit length hash code.
        /// </summary>
        HC4,

        /// <summary>
        /// Represents a matchfinder that uses a 40-bit length hash code.
        /// </summary>
        HC5,

        /// <summary>
        /// Represents a matchfinder to search through a binary tree using a 16-bit long hash code.
        /// </summary>
        BT2,

        /// <summary>
        /// Represents a matchfinder to search through a binary tree using a 24-bit long hash code.
        /// </summary>
        BT3,

        /// <summary>
        /// Represents a matchfinder to search through a binary tree using a 32-bit long hash code.
        /// </summary>
        BT4,

        /// <summary>
        /// Represents a matchfinder to search through a binary tree using a 40-bit long hash code.
        /// </summary>
        BT5,
    }
}
