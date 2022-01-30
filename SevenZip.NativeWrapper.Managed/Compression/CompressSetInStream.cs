using SevenZip.NativeInterface.Compression;
using SevenZip.NativeInterface.IO;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetInStream
        : Unknown, ICompressSetInStream
    {
        private NativeInStreamReader? _nativeReader;

        protected CompressSetInStream(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
            _nativeReader = null;
        }

        public static ICompressSetInStream Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetInStream(nativeInterfaceObject);
        }

        void ICompressSetInStream.ReleaseInStream()
        {
            try
            {
                var result = UnmanagedEntryPoint.ICompressSetInStream__ReleaseInStream(NativeInterfaceObject);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
            }
            finally
            {
                _nativeReader = null;
            }
        }

        void ICompressSetInStream.SetInStream(SequentialInStreamReader sequentialInStreamReader)
        {
            var success = false;
            try
            {
                // To prevent the delegate from being released at an unintended timing by the garbage collector, associate the delegate with the class field.
                _nativeReader = sequentialInStreamReader.ToNativeDelegate();
                var result = UnmanagedEntryPoint.ICompressSetInStream__SetInStream(NativeInterfaceObject, _nativeReader);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                success = true;
            }
            finally
            {
                if (!success)
                    _nativeReader = null;
            }
        }
    }
}
