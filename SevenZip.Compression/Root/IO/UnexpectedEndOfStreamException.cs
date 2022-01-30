using System;
using System.IO;
using System.Runtime.Serialization;

namespace SevenZip.IO
{
    /// <summary>
    /// An exception that is notified when an unexpected end of a stream is detected.
    /// </summary>
    [Serializable]
    public class UnexpectedEndOfStreamException
        : IOException
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UnexpectedEndOfStreamException()
            : base("An unexpected end of the input stream has been detected.")
        {
        }

        /// <summary>
        /// A constructor that initializes an instance by specifying a message.
        /// </summary>
        /// <param name="message">
        /// This is an exception message.
        /// </param>
        public UnexpectedEndOfStreamException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// A constructor initialized by the specified message and an internal exception.
        /// </summary>
        /// <param name="message">
        /// This is an exception message.
        /// </param>
        /// <param name="inner">
        /// The internal exception that caused this exception to be thrown.
        /// </param>
        public UnexpectedEndOfStreamException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// A constructor initialized by the specified serialization and context information
        /// </summary>
        /// <param name="info">
        /// The data for serializing or deserializing the object.
        /// </param>
        /// <param name="context">
        /// The source and destination for the object.
        /// </param>
        protected UnexpectedEndOfStreamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
