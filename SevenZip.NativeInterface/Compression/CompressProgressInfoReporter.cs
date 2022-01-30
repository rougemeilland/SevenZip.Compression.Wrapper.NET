using System;

namespace SevenZip.NativeInterface.Compression
{
#if true
    /// <summary>
    /// Delegate of callback function called when the process of encoding/decoding changes
    /// </summary>
    /// <param name="inSize">
    /// Set the total size of the data read by the coder from the input stream in bytes.
    /// If the total size of the read data is unknown, set it to null.
    /// </param>
    /// <param name="outSize">
    /// Set the total size of the data written by the coder to the output stream in bytes.
    /// If the total size of the written data is unknown, set it to null.
    /// </param>
    public delegate void CompressProgressInfoReporter(UInt64? inSize, UInt64? outSize);
#else
    /// <summary>
    /// Delegate of callback function called when the process of encoding/decoding changes
    /// </summary>
    /// <param name="inSize">
    /// inSize is of type UInt64*.
    /// If inSize is not null, it indicates the total number of bytes of data processed in the input stream.
    /// </param>
    /// <param name="outSize">
    /// outSize is of type UInt64*.
    /// If outSize is not null, it indicates the total number of bytes of data processed in the output stream.
    /// </param>
    /// <remarks>
    /// <para>
    /// Keep in mind that some coders always give inSize or outSize null (or both).
    /// </para>
    /// <para>
    /// When the delegate is called, do not notify the caller of the exception.
    /// </para>
    /// </remarks>
    public delegate void CompressProgressInfoReporter(IntPtr inSize, IntPtr outSize);
#endif
}
