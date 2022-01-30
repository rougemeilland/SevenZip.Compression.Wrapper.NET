using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.IO
{
    /// <summary>
    /// An interface for streams that can be written sequentially.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000300020000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByExternalCode)]
    public interface ISequentialOutStream
        : IUnknown
    {
        /// <summary>
        /// Write the data.
        /// </summary>
        /// <param name="data">
        /// Set a pointer to the beginning of the data to be written.
        /// </param>
        /// <param name="size">
        /// Sets the length of the data to be written in bytes.
        /// </param>
        /// <returns>
        /// Returns the length in bytes of the data actually written.
        /// </returns>
        UInt32 Write(IntPtr data, UInt32 size);
    }
}
