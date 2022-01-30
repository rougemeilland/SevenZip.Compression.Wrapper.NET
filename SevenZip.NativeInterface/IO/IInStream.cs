using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.IO
{
    /// <summary>
    /// An interface with no implementation.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000300030000")]
    [InterfaceImplementation(InterfaceImplementarionType.NotImplemented)]
    public interface IInStream
        : ISequentialInStream
    {
        // **** Not yet implemented. ****

        // void Seek(Int64 offset, UInt32 seekOrigin, UInt64 * newPosition);
    }
}
