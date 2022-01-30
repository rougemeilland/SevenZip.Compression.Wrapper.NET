using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Security
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400900000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICryptoSetPassword
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void CryptoSetPassword(const Byte * data, UInt32 size);
    }
}
