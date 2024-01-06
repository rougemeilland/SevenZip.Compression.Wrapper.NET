using Palmtree.IO;

namespace SevenZip.Compression
{
    /// <summary>
    /// An interface for decoder streams that supports the <see cref="ISequentialInputByteStream"/> and <see cref="ICompressGetInStreamProcessedSize"/> interfaces.
    /// </summary>
    public interface ICompressDecoderStream
        : ISequentialInputByteStream, ICompressGetInStreamProcessedSize
    {
    }
}
