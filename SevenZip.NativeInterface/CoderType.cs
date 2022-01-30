namespace SevenZip.NativeInterface
{
    /// <summary>
    /// An enumeration that indicates the type of coder.
    /// </summary>
    public enum CoderType
    {
        /// <summary>
        /// The type of coder is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The coder is a decoder.
        /// </summary>
        Decoder,

        /// <summary>
        /// The coder is an encoder.
        /// </summary>
        Encoder,
    }
}
