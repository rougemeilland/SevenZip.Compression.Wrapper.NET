using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Security
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-0004008C0000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICryptoResetInitVector
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void ResetInitVector();
    }
}
