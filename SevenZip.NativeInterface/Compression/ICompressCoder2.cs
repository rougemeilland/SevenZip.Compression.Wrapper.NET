using SevenZip.NativeInterface.IO;
using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that provides a way to code between multiple streams.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400180000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressCoder2
        : IUnknown
    {
        /// <summary>
        /// Coding from multiple streams to different streams.
        /// </summary>
        /// <param name="inStreams">
        /// <para>
        /// It is an array of pairs of <c>sequentialInStreamReader</c> which is a delegate that reads data from the input stream and <c>inStreamSize</c> which is the length in bytes of the data that can be read from the input stream.
        /// </para>
        /// <para>
        /// If <c>inStreamSize</c> is null, it means that the corresponding stream data length is not specified.
        /// </para>
        /// </param>
        /// <param name="outStreams">
        /// <para>
        /// It is an array of a pair of <c>sequentialOutStreamWriter</c> which is a delegate that writes data to the output stream and <c>outStreamSize</c> which is the length in bytes of the data that can be written to the output stream.
        /// </para>
        /// <para>
        /// If <c>outStreamSize</c> is null, it means that the data length of the corresponding stream is not specified.
        /// </para>
        /// </param>
        /// <param name="progressReporter">
        /// It is a delegate of the callback function to be notified of the processing progress of the coder.
        /// If you do not need to be notified of the progress of processing, give null.
        /// </param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// The number of streams (<paramref name="inStreams"/> and <paramref name="outStreams"/>) given to this method is not arbitrary by the caller.
        /// Note that your coder implementation may limit the number of streams you can specify.
        /// </item>
        /// </list>
        /// </remarks>
        void Code(ReadOnlySpan<(SequentialInStreamReader sequentialInStreamReader, UInt64? inStreamSize)> inStreams, ReadOnlySpan<(SequentialOutStreamWriter sequentialOutStreamWriter, UInt64? outStreamSize)> outStreams, CompressProgressInfoReporter? progressReporter);
    }
}
