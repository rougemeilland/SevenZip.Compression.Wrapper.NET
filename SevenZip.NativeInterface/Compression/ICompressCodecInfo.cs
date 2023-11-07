using System;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to refer to codec information.
    /// </summary>
    public interface ICompressCodecInfo
    {
        /// <summary>
        /// An index number that identifies the codec.
        /// </summary>
        Int32 Index { get; }

        /// <summary>
        /// The codec ID.
        /// </summary>
        UInt64 ID { get; }

        /// <summary>
        /// The name of the codec.
        /// </summary>
        string CodecName { get; }

        /// <summary>
        /// A UUID that identifies the codec.
        /// </summary>
        Guid CoderClassId { get; }

        /// <summary>
        /// The number of compressed streams coded by the codec.
        /// </summary>
        UInt32 PackStreams { get; }

        /// <summary>
        /// The type of coder (encoder or decoder) that can be created.
        /// </summary>

        CoderType CoderType { get; }

        /// <summary>
        /// True is returned if a coder that implements the <see cref="ICompressCoder"/> interface can be created. If not, false is returned.
        /// </summary>
        bool IsSupportedICompressCoder { get; }

        /// <summary>
        /// True is returned if a coder that implements the <see cref="ICompressCoder2"/> interface can be created. If not, false is returned.
        /// </summary>
        bool IsSupportedICompressCoder2 { get; }

        /// <summary>
        /// True is returned if a coder that implements the <see cref="ICompressFilter"/> interface can be created. If not, false is returned.
        /// </summary>
        bool IsSupportedICompressFilter { get; }

        /// <summary>
        /// Create a coder that implements <see cref="ICompressCoder"/>.
        /// </summary>
        /// <returns>
        /// The created object that implements the <see cref="ICompressCoder"/> interface.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="ICompressCoder"/> interface does not inherit from the <see cref="IDisposable"/> interface, but the created coder object implements the <see cref="IDisposable"/> interface.
        /// If you no longer need the created coder object, release it by calling <see cref="IDisposable.Dispose"/>.
        /// </para>
        /// <para>
        /// The sample code is as follows.:
        /// <code>
        /// ICompressCoder coderObject = codec.CreateCompressCoder();
        /// 
        /// ...
        /// 
        /// (coderObject as IDisposable)?.Dispose();
        /// </code>
        /// </para>
        /// </remarks>
        ICompressCoder CreateCompressCoder();

        /// <summary>
        /// Create a coder that implements <see cref="ICompressCoder2"/>.
        /// </summary>
        /// <returns>
        /// The created object that implements the <see cref="ICompressCoder2"/> interface.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="ICompressCoder2"/> interface does not inherit from the <see cref="IDisposable"/> interface, but the created coder object implements the <see cref="IDisposable"/> interface.
        /// If you no longer need the created coder object, release it by calling <see cref="IDisposable.Dispose"/>.
        /// </para>
        /// <para>
        /// The sample code is as follows.:
        /// <code>
        /// ICompressCoder2 coderObject = codec.CreateCompressCoder2();
        /// 
        /// ...
        /// 
        /// (coderObject as IDisposable)?.Dispose();
        /// </code>
        /// </para>
        /// </remarks>
        ICompressCoder2 CreateCompressCoder2();

        /// <summary>
        /// Create a coder that implements <see cref="ICompressFilter"/>.
        /// </summary>
        /// <returns>
        /// The created object that implements the <see cref="ICompressFilter"/> interface.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="ICompressFilter"/> interface does not inherit from the <see cref="IDisposable"/> interface, but the created coder object implements the <see cref="IDisposable"/> interface.
        /// If you no longer need the created coder object, release it by calling <see cref="IDisposable.Dispose"/>.
        /// </para>
        /// <para>
        /// The sample code is as follows.:
        /// <code>
        /// ICompressFilter filterObject = codec.CreateCompressFilter();
        /// 
        /// ...
        /// 
        /// (filterObject as IDisposable)?.Dispose();
        /// </code>
        /// </para>
        /// </remarks>
        ICompressFilter CreateCompressFilter();
    }
}
