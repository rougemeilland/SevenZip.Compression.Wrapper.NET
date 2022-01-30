using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400040000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByExternalCode)]
    public interface ICompressProgressInfo
        : IUnknown
    {
        /// <summary>
        /// Notifies the progress of coder processing.
        /// </summary>
        /// <param name="inSize">
        /// If this value is not null, it is the number of bytes of processed data in the coder's input stream.
        /// </param>
        /// <param name="outSize">
        /// If this value is not null, it is the number of bytes of processed data in the coder's output stream.
        /// </param>
        void SetRatioInfo(UInt64? inSize, UInt64? outSize);
    }
}
