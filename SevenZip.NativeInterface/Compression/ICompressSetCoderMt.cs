using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to set the number of threads used by the coder.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400250000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetCoderMt
        : IUnknown
    {
        /// <summary>
        /// Sets the number of threads used by the coder.
        /// </summary>
        /// <param name="numThreads">
        /// Sets the number of threads used by the coder.
        /// </param>
        void SetNumberOfThreads(UInt32 numThreads);
    }
}
