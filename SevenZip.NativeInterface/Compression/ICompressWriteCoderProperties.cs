using SevenZip.NativeInterface.IO;
using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to set values for coder properties.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400230000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressWriteCoderProperties
        : IUnknown
    {
        /// <summary>
        /// Writes the coder parameters to the output destination virtual stream.
        /// </summary>
        /// <param name="outStreamWriter">
        /// A delegate to the callback function that is called when data is written to the virtual stream.
        /// See <see cref="SequentialOutStreamWriter"/> for more information.
        /// </param>
        void WriteCoderProperties(SequentialOutStreamWriter outStreamWriter);
    }
}
