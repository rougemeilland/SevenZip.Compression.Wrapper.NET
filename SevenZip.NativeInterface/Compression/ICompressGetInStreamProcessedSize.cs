using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// It is an interface that can get the processed data amount of the input stream of the coder.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400240000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressGetInStreamProcessedSize
        : IUnknown
    {
        /// <summary>
        /// The number of bytes of processed data in the coder's input stream.
        /// </summary>
        UInt64 InStreamProcessedSize { get; }

    }
}
