using SevenZip.NativeInterface.Compression;
using System;
using System.IO;

namespace SevenZip.Compression
{
    /// <summary>
    /// A class of virtual stream format coders that can be read sequentially.
    /// </summary>
    public abstract class CompressCoderInStream
        : IO.ISequentialInStream, IDisposable
    {
        private readonly NativeInterface.IO.ISequentialInStream _sequentialInStream;

        private bool _isDisposed;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="sequentialInStream">
        /// Set the coder's <see cref="NativeInterface.IO.ISequentialInStream"/> interface object.
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        protected CompressCoderInStream(NativeInterface.IO.ISequentialInStream sequentialInStream)
        {
            _sequentialInStream = sequentialInStream ?? throw new ArgumentNullException(nameof(sequentialInStream));
        }

        /// <summary>
        /// Read the data from the coder.
        /// </summary>
        /// <param name="data">
        /// Set a buffer to store the data to be read.
        /// </param>
        /// <returns>
        /// Returns the byte length of the read data.
        /// If the end of the stream is reached and no more data can be read, 0 is returned.
        /// </returns>
        public virtual Int32 Read(Span<Byte> data)
        {
            return checked((Int32)_sequentialInStream.Read(data));
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
                    (_sequentialInStream as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
        }
    }
}
