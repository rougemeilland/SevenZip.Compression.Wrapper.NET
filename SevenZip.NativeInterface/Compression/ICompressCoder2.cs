using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400180000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface ICompressCoder2
        : IUnknown
    {
        // **** Not yet implemented. ****

        // void Code(ISequentialInStream* const* inStreams, const UInt64* const* inSizes, UInt32 numInStreams, ISequentialOutStream* const* outStreams, const UInt64* const* outSizes, UInt32 numOutStreams, ICompressProgressInfo * progress);
    }
}
