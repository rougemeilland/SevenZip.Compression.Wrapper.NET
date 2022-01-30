using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to specify the maximum size of the output stream.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400340000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetOutStreamSize
        : IUnknown
    {
        /// <summary>
        /// Sets the maximum size of the destination stream.
        /// </summary>
        /// <param name="outSize">
        /// Sets the maximum size of the destination stream in bytes.
        /// </param>
        void SetOutStreamSize(UInt64? outSize);
    }
}
