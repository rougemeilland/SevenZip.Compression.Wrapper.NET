using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Security
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400800000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICryptoProperties
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void SetKey(const Byte * data, UInt32 size);
        // void SetInitVector(const Byte * data, UInt32 size);
    }
}
