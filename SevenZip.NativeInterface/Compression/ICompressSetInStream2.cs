using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400370000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICompressSetInStream2
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void SetInStream2(UInt32 streamIndex, ISequentialInStream * inStream);
        // void ReleaseInStream2(UInt32 streamIndex);
    }
}
