using SevenZip.IO;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeInterface.IO;
using System;
using System.IO;

namespace SevenZip.Compression
{
    /// <summary>
    /// This is the basic class of the coder class.
    /// </summary>
    public abstract class CompressCoder
        : IDisposable
    {
        private readonly ICompressCoder _coder;

        private bool _isDisposed;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="coder">
        /// Set the native coder object.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="coder"/> is null.
        /// </exception>
        protected CompressCoder(ICompressCoder coder)
        {
            _coder = coder ?? throw new ArgumentNullException(nameof(coder));
        }

        /// <summary>
        /// Reads data from the input stream, codes it, and writes it to the output stream.
        /// </summary>
        /// <param name="inStream">
        /// Set the input stream.
        /// </param>
        /// <param name="outStream">
        /// Set the output stream.
        /// </param>
        /// <param name="inStreamSize">
        /// Set the length of the data in the input stream in bytes.
        /// Set to null if the length of the data in the input stream is unknown.
        /// </param>
        /// <param name="outStreamSize">
        /// Set the length of the data in the output stream in bytes.
        /// Set to null if the length of the data in the output stream is unknown.
        /// </param>
        /// <param name="progress">
        /// <para>
        /// Set an object to receive notification of coding progress.
        /// This object must implement <see cref="IProgress{T}">IProgress&lt;(<see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> inStreamProcessedCount, <see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> outStreamProcessedCount)&gt;</see>.
        /// </para>
        /// <para>
        /// Set to null if you do not need to be notified of progress.
        /// </para>
        /// </param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>Values for <paramref name="inStreamSize"/>, <paramref name="outStreamSize"/>, or both may be ignored in the coding. Whether they are ignored depends on the coder's implementation.</item>
        /// <item>If <paramref name="inStreamSize"/> or <paramref name="outStreamSize"/> is not specified, the behavior of how far to continue coding depends on the coder. (For example, it may be coded until it reaches the end of the input stream, or it may detect the end of the data in the input data and stop coding there.)</item>
        /// <item>The object specified by <paramref name="progress"/> may not be notified of the progress of coding. Alternatively, even if notified, both "inStreamProcessedCount" and "outStreamProcessedCount" are not necessarily notified. Whether and how progress is notified depends on the coder's implementation.</item>
        /// </list>
        /// </remarks>
        public virtual void Code(Stream inStream, Stream outStream, UInt64? inStreamSize, UInt64? outStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));

            _coder.Code(
                inStream.GetStreamReader(),
                outStream.GetStreamWriter(),
                inStreamSize,
                outStreamSize,
                progress.GetProgressReporter());
        }

        /// <summary>
        /// Reads data from the input stream, codes it, and writes it to the output stream.
        /// </summary>
        /// <param name="inStream">
        /// Set the input stream.
        /// </param>
        /// <param name="outStream">
        /// Set the output stream.
        /// </param>
        /// <param name="inStreamSize">
        /// Set the length of the data in the input stream in bytes.
        /// Set to null if the length of the data in the input stream is unknown.
        /// </param>
        /// <param name="outStreamSize">
        /// Set the length of the data in the output stream in bytes.
        /// Set to null if the length of the data in the output stream is unknown.
        /// </param>
        /// <param name="progress">
        /// <para>
        /// Set an object to receive notification of coding progress.
        /// This object must implement <see cref="IProgress{T}">IProgress&lt;(<see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> inStreamProcessedCount, <see cref="Nullable{UInt64}">Nullable&lt;<see cref="UInt64"/>&gt;</see> outStreamProcessedCount)&gt;</see>.
        /// </para>
        /// <para>
        /// Set to null if you do not need to be notified of progress.
        /// </para>
        /// </param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// This override is provided in case you do not want to use <see cref="Stream"/> class for the I/O stream,
        /// and you must have an implementation of the <see cref="ISequentialInStream"/> and <see cref="ISequentialOutStream"/> interfaces in advance.
        /// </item>
        /// <item>Values for <paramref name="inStreamSize"/>, <paramref name="outStreamSize"/>, or both may be ignored in the coding. Whether they are ignored depends on the coder's implementation.</item>
        /// <item>If <paramref name="inStreamSize"/> or <paramref name="outStreamSize"/> is not specified, the behavior of how far to continue coding depends on the coder. (For example, it may be coded until it reaches the end of the input stream, or it may detect the end of the data in the input data and stop coding there.)</item>
        /// <item>The object specified by <paramref name="progress"/> may not be notified of the progress of coding. Alternatively, even if notified, both "inStreamProcessedCount" and "outStreamProcessedCount" are not necessarily notified. Whether and how progress is notified depends on the coder's implementation.</item>
        /// </list>
        /// </remarks>
        public virtual void Code(IO.ISequentialInStream inStream, IO.ISequentialOutStream outStream, UInt64? inStreamSize, UInt64? outStreamSize, IProgress<(UInt64? inStreamProcessedCount, UInt64? outStreamProcessedCount)>? progress)
        {
            if (inStream is null)
                throw new ArgumentNullException(nameof(inStream));
            if (outStream is null)
                throw new ArgumentNullException(nameof(outStream));

            _coder.Code(
                inStream.GetStreamReader(),
                outStream.GetStreamWriter(),
                inStreamSize,
                outStreamSize,
                progress.GetProgressReporter());
        }

        /// <summary>
        /// Explicitly release the resource associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the resources associated with the object.
        /// </summary>
        /// <param name="disposing">
        /// Set true when calling explicitly from <see cref="IDisposable.Dispose"/>.
        /// Set to false when calling implicitly from the garbage collector.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    (_coder as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
