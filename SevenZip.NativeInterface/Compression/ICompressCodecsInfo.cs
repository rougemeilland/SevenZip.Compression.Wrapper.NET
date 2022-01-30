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
        /// Initialize the instance.
        /// This method must always be called first.
        /// </summary>
        void Initialize();

#if true
        /// <summary>
        /// Enumerate the supported codec.
        /// </summary>
        /// <returns>
        /// An enumerator of supported codecs.
        /// </returns>
        IEnumerable<ICompressCodecInfo> EnumerateCodecs();
#else
        /// <summary>
        /// Enumerates information on supported codecs.
        /// </summary>
        /// <param name="compressCodecInfoGetter">
        /// A delegate for a callback function that returns codec information.
        /// This callback function will be called for each codec until <see cref="EnumerateCodecs(CompressCodecInfoGetter)"/> returns.
        /// See <see cref="CompressCodecInfoGetter"/> for more information on callback functions.
        /// </param>
        void EnumerateCodecs(CompressCodecInfoGetter compressCodecInfoGetter);
#endif
    }
}
