using SevenZip.NativeInterface.IO;
using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to set the input stream for the coder.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400310000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetInStream
        : IUnknown
    {
        /// <summary>
        /// Set the input stream for the coder.
        /// </summary>
        /// <param name="sequentialInStreamReader">
        /// A delegate to the callback function that will be called when the coder makes a read from the input stream.
        /// </param>
        void SetInStream(SequentialInStreamReader sequentialInStreamReader);

        /// <summary>
        /// Releases the input stream set in the coder.
        /// </summary>
        void ReleaseInStream();
    }
}
