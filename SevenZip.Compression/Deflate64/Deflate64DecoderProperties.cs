namespace SevenZip.Compression.Deflate64
{
    /// <summary>
    /// A container class for Deflate64 decoder properties.
    /// </summary>
    public class Deflate64DecoderProperties
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public Deflate64DecoderProperties()
        {
            FinishMode = null;
        }

        /// <summary>
        /// <para>
        /// In decoding, it means whether to decode the entire input stream (full_decoding mode) or only part of the input stream (partial_decoding mode).
        /// </para>
        /// <para>
        /// The default value is null, which means partial_decoding mode.
        /// If you want to change this behavior, set the following values. :
        /// <list type="bullet">
        /// <item>Set true for full_decoding mode.</item>
        /// <item>Set false for partial_decoding mode.</item>
        /// </list>
        /// </para>
        /// </summary>
        public bool? FinishMode { get; set; }
    }
}
