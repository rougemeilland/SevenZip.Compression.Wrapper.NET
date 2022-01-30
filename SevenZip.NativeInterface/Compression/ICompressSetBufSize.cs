using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to set the size of the coder's I/O buffer.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400350000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetBufSize
        : IUnknown
    {
        /// <summary>
        /// Sets the input buffer size for the coder.
        /// </summary>
        /// <param name="streamIndex">
        /// For multistream, this is the stream index number.
        /// For single stream, <paramref name="streamIndex"/> is ignored.
        /// </param>
        /// <param name="size">
        /// Sets the size of the stream's input buffer in bytes.
        /// </param>
        void SetInBufSize(UInt32 streamIndex, UInt32 size);

        /// <summary>
        /// Sets the size of the output buffer for the coder.
        /// </summary>
        /// <param name="streamIndex">
        /// For multistream, this is the stream index number.
        /// For single stream, <paramref name="streamIndex"/> is ignored.
        /// </param>
        /// <param name="size">
        /// Sets the size of the stream's output buffer in bytes.
        /// </param>
        void SetOutBufSize(UInt32 streamIndex, UInt32 size);
    }
}
