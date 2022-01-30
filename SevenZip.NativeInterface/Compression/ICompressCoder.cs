using SevenZip.NativeInterface.IO;
using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that can be encoded or decoded.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400050000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressCoder
        : IUnknown
    {
        /// <summary>
        /// Coding data between streams.
        /// </summary>
        /// <param name="sequentialInStreamReader">
        /// A delegate to the callback function that will be called when it needs to read data from the input stream.
        /// See <see cref="SequentialInStreamReader"/> for more information.
        /// </param>
        /// <param name="sequentialOutStreamWriter">
        /// A delegate to the callback function that will be called when it needs to write data to the output stream.
        /// See <see cref="SequentialOutStreamWriter"/> for more information.
        /// </param>
        /// <param name="inSize">
        /// Gives the length of the input stream in bytes.
        /// If you omit the length specification, give null instead.
        /// </param>
        /// <param name="outSize">
        /// Gives the length of the output stream in bytes.
        /// If you omit the length specification, give null instead.
        /// </param>
        /// <param name="progressReporter">
        /// It is a delegate of the callback function to be notified of the processing progress of the coder.
        /// If you do not need to be notified of the progress of processing, give null.
        /// </param>
        void Code(SequentialInStreamReader sequentialInStreamReader, SequentialOutStreamWriter sequentialOutStreamWriter, UInt64? inSize, UInt64? outSize, CompressProgressInfoReporter? progressReporter);
    }
}
