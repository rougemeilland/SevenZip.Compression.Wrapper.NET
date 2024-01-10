using System;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression
{
    /// <summary>
    /// This class retrieves information about installed 7-Zip module.
    /// </summary>
    public static class SevenZipModule
    {
        /// <summary>
        /// Get the version number of 7-zip.
        /// </summary>
        /// <value>
        /// A <see cref="UInt32"/> value that indicates the 7-zip version number.
        /// The upper 16 bits are the major version, and the lower 16 bits are the minor version.
        /// </value>
        public static UInt32 Versio => CompressCodecsCollection.Instance.Version;

        /// <summary>
        /// Get the interface type.
        /// </summary>
        /// <value>
        /// One of the following values is returned.
        /// <list type="bullet">
        /// <item><term>If the IUnknown interface supports virtual destructor</term><description>1</description></item>
        /// <item><term>If the IUnknown interface does not support virtual destructor</term><description>0</description></item>
        /// </list>
        /// </value>
        public static UInt32 InterfaceType => CompressCodecsCollection.Instance.InterfaceType;
    }
}
