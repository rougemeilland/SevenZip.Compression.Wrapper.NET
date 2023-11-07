using SevenZip.NativeInterface;
using SevenZip.NativeWrapper.Managed.win.x64.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.win.x64
{
    /// <summary>
    /// The base class for all interface implementations.
    /// </summary>
    public partial class Unknown
        : IUnknown, IDisposable
    {
        private bool _isDisposed;
        private IntPtr _nativeInterfaceObject;

        static Unknown()
        {
#if DEBUG
            ValidateInterfaces();
#endif
        }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="nativeInterfaceObject">
        /// Sets a pointer to a native object that points to the entity of the specified interface.
        /// </param>
        protected Unknown(IntPtr nativeInterfaceObject)
        {
            _isDisposed = false;
            _nativeInterfaceObject = nativeInterfaceObject;
        }

        /// <summary>
        /// It's a destructor.
        /// </summary>
        ~Unknown()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the object to access the specified interface.
        /// </summary>
        /// <param name="interfaceType">
        /// Set the interface type.
        /// </param>
        /// <returns>
        /// Returns an object to access the interface of the type specified by <paramref name="interfaceType"/>.
        /// This object can be cast to the type specified by <paramref name="interfaceType"/>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The interface object has already been disposed.</exception>
        /// <exception cref="NotSupportedException">This object does not support the interface of the type specified by <paramref name="interfaceType"/>.</exception>
        public IUnknown QueryInterface(Type interfaceType)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (_nativeInterfaceObject == IntPtr.Zero)
                throw new InvalidOperationException();

            var success = false;
            var newNativeInterfaceObject = QueryInterface(_nativeInterfaceObject, interfaceType.GUID);
            try
            {
                var managedInterfaceObject = GetInterfaceObjectCreator(interfaceType.GUID, newNativeInterfaceObject);
                if (managedInterfaceObject is null)
                    throw new NotSupportedException();
                success = true;
                return managedInterfaceObject;
            }
            finally
            {
                if (!success)
                {
                    if (newNativeInterfaceObject != IntPtr.Zero)
                        _ = UnmanagedEntryPoint.IUnknown__Release(newNativeInterfaceObject);
                }
            }
        }

        /// <summary>
        /// Explicitly release the resource associated with the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// An object for accessing the native interface.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The interface object has already been disposed.</exception>
        /// <exception cref="InvalidOperationException">No object has been assigned to access the native interface.</exception>
        protected IntPtr NativeInterfaceObject
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
                if (_nativeInterfaceObject == IntPtr.Zero)
                    throw new InvalidOperationException();

                return _nativeInterfaceObject;
            }
        }

        /// <summary>
        /// Assign an object to access the native interface.
        /// </summary>
        /// <param name="nativeInterfaceObject"></param>
        /// <exception cref="ObjectDisposedException">The interface object has already been disposed.</exception>
        /// <exception cref="ArgumentException">An attempt was made to assign a null pointer.</exception>
        /// <remarks>
        /// If an object has already been assigned to access the native interface, the assignment will be made after releasing the assigned object.
        /// </remarks>
        protected void AttatchNativeInterfaceObject(IntPtr nativeInterfaceObject)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentException("An attempt was made to assign a null pointer.", nameof(nativeInterfaceObject));

            if (_nativeInterfaceObject != IntPtr.Zero)
                _ = UnmanagedEntryPoint.IUnknown__Release(_nativeInterfaceObject);
            _nativeInterfaceObject = nativeInterfaceObject;

        }

        /// <summary>
        /// Releases the resources associated with the object.
        /// </summary>
        /// <param name="disposing">
        /// Set true when calling explicitly from <see cref="IDisposable.Dispose"/>.
        /// Set to false when calling implicitly from the garbage collector.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                }
                if (_nativeInterfaceObject != IntPtr.Zero)
                {
                    _ = UnmanagedEntryPoint.IUnknown__Release(_nativeInterfaceObject);
                    _nativeInterfaceObject = IntPtr.Zero;
                }
                _isDisposed = true;
            }
        }

        private static IntPtr QueryInterface(IntPtr nativeInterfaceObject, Guid interfaceId)
        {
            var interfaceIdBuffer = NativeGUID.FromManagedGuidToNativeGuid(interfaceId);
            var result = UnmanagedEntryPoint.IUnknown__QueryInterface(nativeInterfaceObject, ref interfaceIdBuffer, out IntPtr newNativeInterfaceObject);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return newNativeInterfaceObject;
        }
    }
}
