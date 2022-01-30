using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Hash
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400C00000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface IHasher
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void Init();
        // void Update(const void *data, UInt32 size);
        // void Final(Byte *digest);
        // UInt32 GetDigestSize();
    }
}
