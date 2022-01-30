using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400300000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICompressGetSubStreamSize
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void GetSubStreamSize(UInt64 subStream, UInt64 * value);
    }
}
