using System;

namespace SevenZip.IO
{
    /// <summary>
    /// A delegate for a function that parses the header of the data read from the coder's input stream.
    /// </summary>
    /// <param name="headerReader">
    /// An interface object for accessing the coder's input stream.
    /// </param>
    /// <returns>
    /// Returns the byte length of the data read from the input stream when parsing the header.
    /// </returns>
    public delegate UInt64 CoderHeaderParser(ICoderHeaderReader headerReader);
}
