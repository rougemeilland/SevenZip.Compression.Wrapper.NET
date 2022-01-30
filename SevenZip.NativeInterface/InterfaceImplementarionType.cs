using System;

namespace SevenZip.NativeInterface
{
    /// <summary>
    /// An enumeration that indicates the implementation status or usage of the interface.
    /// </summary>
    [Flags]
    public enum InterfaceImplementarionType
    {
        /// <summary>
        /// Indicates that there is no information.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates an interface that is defined by higher-level code and exposed to lower-level code.
        /// </summary>
        ImplementedByExternalCode = 1 << 0,

        /// <summary>
        /// Indicates an interface that is defined by lower-level code and exposed to higherr-level code.
        /// </summary>
        ImplementedByInternalCode = 1 << 1,

        /// <summary>
        /// Indicates that the interface implementation does not exist or is not supported.
        /// </summary>
        NotImplemented = 1 << 2,
    }
}
