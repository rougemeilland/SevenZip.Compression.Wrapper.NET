using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.IO
{
    /// <summary>
    /// A stream interface that can be read sequentially.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000300010000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByExternalCode | InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ISequentialInStream
        : IUnknown
    {
        /// <summary>
        /// Read the data.
        /// </summary>
        /// <param name="data">
        /// Set a buffer to store the data to be read.
        /// </param>
        /// <returns>
        /// Returns the length in bytes of the data actually read.
        /// </returns>
        UInt32 Read(Span<Byte> data);
    }
}
