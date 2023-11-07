using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface that allows you to see information about supported coders.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-000400600000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressCodecsInfo
        : IUnknown
    {
        /// <summary>
        /// Initialize the plugin.
        /// </summary>
        /// <param name="seevenZipNativeLibraryPath">
        /// The path to the 7-zip native library.
        /// </param>
        /// <returns>
        /// <para>
        /// If the initialization is successful, true is returned.
        /// </para>
        /// <para>
        /// If the 7-zip native library specified by seevenZipNativeLibraryPath does not exist, false will be returned.
        /// </para>
        /// <para>
        /// In cases other than the above, an exception will be notified.
        /// </para>
        /// </returns>
        bool Initialize(string seevenZipNativeLibraryPath);

        /// <summary>
        /// Enumerate the supported codec.
        /// </summary>
        /// <returns>
        /// An enumerator of supported codecs.
        /// </returns>
        IEnumerable<ICompressCodecInfo> EnumerateCodecs();
    }
}
