using System;
using System.IO;

namespace SevenZip.IO
{
    /// <summary>
    /// The base class of the decoder stream that inherits <see cref="Stream"/>.
    /// </summary>
    public abstract class DecoderStream
        : Stream, ISequentialInStream
    {
        private readonly NativeInterface.IO.ISequentialInStream _sequentialInStream;

        private bool _isDisposed;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="sequentialInStream">
        /// Set the coder's <see cref="NativeInterface.IO.ISequentialInStream"/> interface object.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="sequentialInStream"/> is null.</exception>
        protected DecoderStream(NativeInterface.IO.ISequentialInStream sequentialInStream)
        {
            _isDisposed = false;
            _sequentialInStream = sequentialInStream ?? throw new ArgumentNullException(nameof(sequentialInStream));
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value>
        /// true if the stream supports reading; otherwise, false.
        /// </value>
        public override bool CanRead => true;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value>true if the stream supports seeking; otherwise, false.</value>
        public override bool CanSeek => false;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value>
        /// true if the stream supports writing; otherwise, false.
        /// </value>
        public override bool CanWrite => false;

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value>
        /// A long value representing the length of the stream in bytes.
        /// </value>
        /// <exception cref="NotSupportedException">A class derived from Stream does not support seeking.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override long Length => throw new NotImplementedException();

        /// <summary>
        ///  When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value>
        /// The current position within the stream.
        /// </value>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">A class derived from Stream does not support seeking.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        public override void Flush() {}

        /// <summary>
        ///  When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. When this method returns, the buffer contains the specified byte array with the values between offset and (offset + count - 1) replaced by the bytes read from the current source.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in buffer at which to begin storing the data read from the current stream.
        /// </param>
        /// <param name="count">
        /// The maximum number of bytes to be read from the current stream.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer.
        /// This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="ArgumentNullException">buffer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override Int32 Read(byte[] buffer, Int32 offset, Int32 count)
        {
            return checked((Int32)_sequentialInStream.Read(new Span<Byte>(buffer, offset, count)));
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">
        /// A byte offset relative to the origin parameter.
        /// </param>
        /// <param name="origin">
        /// A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position.
        /// </param>
        /// <returns>The new position within the current stream.
        /// </returns>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">
        /// The desired length of the current stream in bytes.
        /// </param>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.</exception>
        /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public override void SetLength(long value) => throw new NotImplementedException();

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes. This method copies count bytes from buffer to the current stream.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in buffer at which to begin copying bytes to the current stream.
        /// </param>
        /// <param name="count">The number of bytes to be written to the current stream.
        /// </param>
        /// <exception cref="ArgumentException">The sum of offset and count is greater than the buffer length.</exception>
        /// <exception cref="ArgumentNullException">buffer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">offset or count is negative.</exception>
        /// <exception cref="IOException">An I/O error occurred, such as the specified file cannot be found.</exception>
        /// <exception cref="NotSupportedException">The stream does not support writing.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Write(Byte[], Int32, Int32)"/> was called after the stream was closed.</exception>
        public override void Write(byte[] buffer, Int32 offset, Int32 count) => throw new NotImplementedException();

        Int32 ISequentialInStream.Read(Span<Byte> data)
        {
            return checked((Int32)_sequentialInStream.Read(data));
        }

        /// <summary>
        /// Releases the resources associated with the object.
        /// </summary>
        /// <param name="disposing">
        /// Set true when calling explicitly from <see cref="IDisposable.Dispose"/>.
        /// Set to false when calling implicitly from the garbage collector.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    (_sequentialInStream as IDisposable)?.Dispose();
                }
                _isDisposed = true;
            }
            base.Dispose(disposing);
        }
    }
}
