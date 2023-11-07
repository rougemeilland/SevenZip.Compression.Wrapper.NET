using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that provides a way to filter data.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400400000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressFilter
        : IUnknown
    {
        /// <summary>
        /// Initialize the filter.
        /// </summary>
        void Init();

        /// <summary>
        /// Filters the data stored in a given buffer and stores it in the same buffer.
        /// </summary>
        /// <param name="data">
        /// Converts the data stored in a given buffer and stores it in the same buffer.
        /// </param>
        /// <returns>
        /// Returns the byte length of the converted data.
        /// </returns>
        UInt32 Filter(Span<Byte> data);
    }
}
