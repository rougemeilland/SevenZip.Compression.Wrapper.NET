#if IS_SUPPORTED_SEVENZIP_PPMD8
using System;

namespace SevenZip.Compression.Ppmd8
{
    /// <summary>
    /// &lt;Unknown&gt;
    /// </summary>
    public enum PPMd8RestoreMethod
        : UInt32
    {
        /// <summary>
        /// Not specified.
        /// </summary>
        None = UInt32.MaxValue,

        /// <summary>
        /// &lt;Unknown&gt;
        /// </summary>
        Restart = 0,

        /// <summary>
        /// &lt;Unknown&gt;
        /// </summary>
        CutOff,
    }
}
#endif
