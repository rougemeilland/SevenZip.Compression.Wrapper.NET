using SevenZip.NativeInterface.IO;
using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// It is an interface that can read the remaining data after processing <see cref="ICompressCoder.Code(SequentialInStreamReader, SequentialOutStreamWriter, UInt64?, UInt64?, CompressProgressInfoReporter?)"/> from the input stream.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400290000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressReadUnusedFromInBuf
        : IUnknown
    {
        /// <summary>
        /// Reads the remaining data after processing <see cref="ICompressCoder.Code(SequentialInStreamReader, SequentialOutStreamWriter, UInt64?, UInt64?, CompressProgressInfoReporter?)"/> from the input stream.
        /// </summary>
        /// <param name="data">
        /// Set a buffer to store the data to be read.
        /// </param>
        /// <returns>
        /// The length in bytes of the data actually read.
        /// </returns>
        UInt32 ReadUnusedFromInBuf(Span<Byte> data);
    }
}
