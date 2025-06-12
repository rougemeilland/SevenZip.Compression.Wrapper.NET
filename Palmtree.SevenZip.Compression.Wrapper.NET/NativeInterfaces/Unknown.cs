using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal abstract partial class Unknown
        : IDisposable
    {
        private Boolean _isDisposed;
        private IntPtr _nativeInterfaceObject;

        protected Unknown(IntPtr nativeInterfaceObject)
        {
            _isDisposed = false;
            _nativeInterfaceObject = nativeInterfaceObject;
        }

        ~Unknown()
        {
            Dispose(false);
        }

        public virtual Unknown QueryInterface(Type interfaceType)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            if (_nativeInterfaceObject == IntPtr.Zero)
                throw new InvalidOperationException();

            var success = false;
            var newNativeInterfaceObject = QueryInterface(_nativeInterfaceObject, interfaceType.GUID);
            try
            {
                var managedInterfaceObject =
                    CreateInterfaceObject(interfaceType.GUID, newNativeInterfaceObject)
                    ?? throw new NotSupportedException($"The interface identified by ID \"{interfaceType.GUID}\" is not supported.");
                success = true;
                return managedInterfaceObject;
            }
            finally
            {
                if (!success)
                {
                    if (newNativeInterfaceObject != IntPtr.Zero)
                        _ = NativeInterOp.IUnknown__Release(newNativeInterfaceObject);
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected IntPtr NativeInterfaceObject
        {
            get
            {
                ObjectDisposedException.ThrowIf(_isDisposed, this);

                if (_nativeInterfaceObject == IntPtr.Zero)
                    throw new InvalidOperationException();

                return _nativeInterfaceObject;
            }
        }

        protected void AttatchNativeInterfaceObject(IntPtr nativeInterfaceObject)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentException("An attempt was made to assign a null pointer.", nameof(nativeInterfaceObject));

            if (_nativeInterfaceObject != IntPtr.Zero)
                _ = NativeInterOp.IUnknown__Release(_nativeInterfaceObject);
            _nativeInterfaceObject = nativeInterfaceObject;

        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                }

                if (_nativeInterfaceObject != IntPtr.Zero)
                {
                    _ = NativeInterOp.IUnknown__Release(_nativeInterfaceObject);
                    _nativeInterfaceObject = IntPtr.Zero;
                }

                _isDisposed = true;
            }
        }

        private static IntPtr QueryInterface(IntPtr nativeInterfaceObject, Guid interfaceId)
        {
            var interfaceIdBuffer = NativeGUID.FromManagedGuidToNativeGuid(interfaceId);
            var result = NativeInterOp.IUnknown__QueryInterface(nativeInterfaceObject, ref interfaceIdBuffer, out var newNativeInterfaceObject);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return newNativeInterfaceObject;
        }

        private static Unknown? CreateInterfaceObject(Guid iid, IntPtr nativeInterfaceObject)
        {
            var lowerBoundary = 0;
            var upperBoundary = _instanceCreators.Length;
            while (upperBoundary - lowerBoundary > 1)
            {
                var middleIndex = lowerBoundary + (upperBoundary - lowerBoundary) / 2;
                var middle = _instanceCreators[middleIndex];
                if (iid.CompareTo(middle.iid) < 0)
                    upperBoundary = middleIndex;
                else
                    lowerBoundary = middleIndex;
            }

            if (lowerBoundary >= upperBoundary)
                return null;
            var foundItem = _instanceCreators[lowerBoundary];
            if (foundItem.iid != iid)
                return null;
            return foundItem.instanceCreator(nativeInterfaceObject);
        }
    }
}
