using Palmtree.IO;

namespace SevenZip.Compression
{
    /// <summary>
    /// An interface for decoder streams that supports the <see cref="ISequentialInputByteStream"/> interface, the <see cref="ICompressGetInStreamProcessedSize"/> interface, and the <see cref="ICompressReadUnusedFromInBuf"/> interface.
    /// </summary>
    public interface ICompressDecoderStreamWithICompressReadUnusedFromInBuf
        : ICompressDecoderStream, ICompressReadUnusedFromInBuf
    {
    }
}
