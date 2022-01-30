using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.IO
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000300080000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface IStreamGetProps
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void GetProps(UInt64 * size, FILETIME * cTime, FILETIME * aTime, FILETIME * mTime, UInt32 * attrib);
    }
}
