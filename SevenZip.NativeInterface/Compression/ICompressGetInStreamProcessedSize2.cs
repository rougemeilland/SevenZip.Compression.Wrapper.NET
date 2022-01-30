using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400270000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICompressGetInStreamProcessedSize2
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void GetInStreamProcessedSize2(UInt32 streamIndex, UInt64 * value);
    }
}
