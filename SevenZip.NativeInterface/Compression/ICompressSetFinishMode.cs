using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to specify decoding in multiple streams.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400260000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetFinishMode
        : IUnknown
    {
        /// <summary>
        /// Specifies whether to perform multi-stream decoding.
        /// </summary>
        /// <param name="finishMode">
        /// Set to true if you want to decode only from a single stream.
        /// Set false to decode from multiple streams.
        /// </param>
        /// <remarks>
        /// <para>
        /// The initial value of <paramref name="finishMode"/> before calling <see cref="SetFinishMode(bool)"/> is unknown.
        /// The initial value of <paramref name="finishMode"/> may vary depending on the coder,
        /// so be sure to call <see cref="SetFinishMode(bool)"/>  to explicitly specify <paramref name="finishMode"/> on a coder that implements the <see cref="ICompressSetFinishMode"/> interface.
        /// </para>
        /// <para>
        /// Set <paramref name="finishMode"/> to true if you do not expect multi-streaming, such as content contained in a ZIP file.
        /// </para>
        /// </remarks>
        void SetFinishMode(bool finishMode);
    }
}
