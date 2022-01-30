namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// A delegate for a callback function that receives codec information.
    /// </summary>
    /// <param name="codec">
    /// An <see cref="ICompressCodecInfo"/> object that represents the received codec information.
    /// </param>
    public delegate void CompressCodecInfoGetter(ICompressCodecInfo codec);
}
