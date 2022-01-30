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
        None = unchecked((UInt32)(-1)),

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
