using System;

namespace SevenZip.NativeInterface.Compression
{
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
}
