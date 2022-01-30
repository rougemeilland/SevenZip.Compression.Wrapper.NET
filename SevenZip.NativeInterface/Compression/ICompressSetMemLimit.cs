using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to set an upper limit on the size of memory used by the coder.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400280000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetMemLimit
        : IUnknown
    {
        /// <summary>
        /// Sets an upper limit on the size of memory used by the coder.
        /// </summary>
        /// <param name="memUsage">
        /// Sets an upper limit on the size of memory used by the coder.
        /// </param>
        void SetMemLimit(UInt64 memUsage);
    }
}
