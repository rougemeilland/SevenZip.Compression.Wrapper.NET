using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Hash
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400C10000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface IHashers
        : IUnknown
    {
        // **** Not yet implemented. ****

        // UInt32 GetNumHashers();
        // void GetHasherProp(UInt32 index, PROPID propID, PROPVARIANT * value);
        // void CreateHasher(UInt32 index, IHasher * *hasher);
    }
}
