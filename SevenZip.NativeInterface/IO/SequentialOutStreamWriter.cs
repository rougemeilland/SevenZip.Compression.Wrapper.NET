using System;

namespace SevenZip.NativeInterface.IO
{
#if true
    /// <summary>
    /// A delegate for a function that writes data to a stream.
    /// </summary>
    /// <param name="data">
    /// Set the buffer that stores the data to be written.
    /// </param>
    /// <returns>
    /// Returns the byte length of the data that could actually be written.
    /// </returns>
    public delegate Int32 SequentialOutStreamWriter(ReadOnlySpan<Byte> data);
#else
    /// <summary>
    /// A delegate to the callback function that will be called when a virtual stream write occurs.
    /// </summary>
    /// <param name="buffer">
    /// A pointer to the beginning of memory where the data to be written is stored. This is a const void* type.
    /// </param>
    /// <param name="size">
    /// The length in bytes of the data to write.
    /// </param>
    /// <param name="processedSize">
    /// A pointer to memory for storing the byte length of the data that could actually be written. This is an Int32* type.
    /// </param>
    /// <returns>
    /// </returns>
    /// <remarks>
    /// <para>
    /// If size is 0, no write is done and the memory pointed to by processedSize is set to 0.
    /// If size is greater than 0, at least 1 byte or more is written.
    /// </para>
    /// <para>
    /// Do not notify the caller of this delegate with an exception. Please notify the error content with the return value.
    /// The following is a sample error handling:
    /// <code>
    /// try
    /// {
    ///
    ///     // Write data...
    ///
    ///     return 0;
    /// }
    /// catch (Exception ex)
    /// {
    ///    return ex.HResult;
    /// }
    /// </code>
    /// </para>
    /// </remarks>
    public delegate Int32 SequentialOutStreamWriter(IntPtr buffer, Int32 size, IntPtr processedSize);
#endif
}
