using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface.Compression
{
    /// <summary>
    /// An interface for setting various properties on the coder.
    /// </summary>
    [Guid("23170F69-40C1-278A-0000-0004001F0000")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface ICompressSetCoderPropertiesOpt
        : IUnknown
    {
        /// <summary>
        /// Set properties on the coder.
        /// </summary>
        /// <param name="propertiesGetter">
        /// An object that specifies how to determine the type and value of a property to give to a coder.
        /// This object must implement <see cref="ICoderProperties"/>.
        /// See <see cref="ICoderProperties"/> for more information.
        /// </param>
        void SetCoderPropertiesOpt(ICoderProperties propertiesGetter);
    }
}
