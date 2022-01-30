using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeInterface
{
    /// <summary>
    /// The most basic interface for 7-zip coders or interfaces.
    /// </summary>
    [Guid("00000000-0000-0000-C000-000000000046")]
    [InterfaceImplementation(InterfaceImplementarionType.ImplementedByInternalCode)]
    public interface IUnknown
    {
        /// <summary>
        /// Creates an object to access the specified interface.
        /// </summary>
        /// <param name="interfaceType">
        /// Sets the interface type.
        /// </param>
        /// <returns>
        /// Returns an object to access the interface specified by <paramref name="interfaceType"/>.
        /// This object can be cast to the type specified by <paramref name="interfaceType"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="IUnknown"/> interface does not inherit from the <see cref="IDisposable"/> interface, but the created interface object implements the <see cref="IDisposable"/> interface.
        /// If you no longer need the created interface object, release it by calling <see cref="IDisposable.Dispose"/>.
        /// </para>
        /// <para>
        /// The sample code is as follows.:
        /// <code>
        /// ICompressSetFinishMode interfaceObject = (ICompressSetFinishMode)coderObject.QueryInterface(typeof(ICompressSetFinishMode));
        /// 
        /// ...
        /// 
        /// (interfaceObject as IDisposable)?.Dispose();
        /// </code>
        /// </para>
        /// </remarks>
        IUnknown QueryInterface(Type interfaceType);
    }
}
