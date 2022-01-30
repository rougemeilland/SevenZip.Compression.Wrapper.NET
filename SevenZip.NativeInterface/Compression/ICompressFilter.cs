using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400400000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICompressFilter
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void Init();
        // UInt32 Filter(Byte *data, UInt32 size);;
    }
}
