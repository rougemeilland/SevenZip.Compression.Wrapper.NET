using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that sets properties in the format of byte data on the coder.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400220000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetDecoderProperties2
        : IUnknown
    {
        /// <summary>
        /// This method gives the parameters of the coder as a byte array.
        /// </summary>
        /// <param name="data">
        /// Set the buffer that contains the properties to be set in the coder.
        /// </param>
        void SetDecoderProperties2(ReadOnlySpan<Byte> data);
    }
}
