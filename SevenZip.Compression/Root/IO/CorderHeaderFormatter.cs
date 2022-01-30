using System;

namespace SevenZip.IO
{
    /// <summary>
    /// A delegate for a function that writes a header to the coder's output stream.
    /// </summary>
    /// <param name="headerWriter">
    /// An interface object for accessing the output stream.
    /// </param>
    /// <returns>
    /// Returns the byte length of the written header.
    /// </returns>
    public delegate UInt64 CorderHeaderFormatter(ICoderHeaderWriter headerWriter);
}
