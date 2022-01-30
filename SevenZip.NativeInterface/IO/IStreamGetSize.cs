using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.IO
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000300060000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface IStreamGetSize
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void GetSize(UInt64 * size);
    }
}
