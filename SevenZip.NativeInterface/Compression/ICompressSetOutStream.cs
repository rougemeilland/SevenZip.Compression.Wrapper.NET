using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400320000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICompressSetOutStream
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void SetOutStream(ISequentialOutStream * outStream);
        // void ReleaseOutStream();
    }
}
