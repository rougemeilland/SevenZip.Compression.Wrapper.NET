using System;

namespace SevenZip.NativeInterface.IO
{
#if true
    /// <summary>
    /// A delegate for a function that reads data from a stream.
    /// </summary>
    /// <param name="buffer">
    /// Set a buffer to read the data.
    /// </param>
    /// <returns>
    /// The length in bytes of the data actually read is returned.
    /// If the stream has reached the end and can no longer be read, 0 is returned. ..
    /// </returns>
    public delegate Int32 SequentialInStreamReader(Span<Byte> buffer);
#else
    /// <summary>
    /// Delegate of the callback function that is called when a virtual stream is read.
    /// </summary>
    /// <param name="buffer">
    /// A pointer to the beginning of memory for storing the data to be read. This is a void* type.
    /// </param>
    /// <param name="size">
    /// The size of the memory in bytes for storing the data to be read.
    /// </param>
    /// <param name="processedSize">
    /// A pointer to memory for storing the byte length of the read data. This is an Int32* type.
    /// </param>
    /// <returns>
    /// Returns 0 if the read is successful.
    /// If it fails to read, it is the error code. This has the same meaning as the value of <see cref="Exception.HResult"/>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// If size is 0, no reading is done and the memory pointed to by processedSize is set to 0.
    /// If size is greater than 0 and the end of the virtual stream is reached, the memory pointed to by processedSize is set to 0.
    /// </para>
    /// <para>
    /// Do not notify the caller of this delegate with an exception. Please notify the error content with the return value.
    /// The following is a sample error handling:
    /// <code>
    /// try
    /// {
    ///
    ///     // Read data...
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
    public delegate Int32 SequentialInStreamReader(IntPtr buffer, Int32 size, IntPtr processedSize);
#endif
}
