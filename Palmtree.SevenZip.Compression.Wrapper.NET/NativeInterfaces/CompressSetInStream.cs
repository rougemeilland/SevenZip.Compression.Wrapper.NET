using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetInStream
        : ICompressSetInStream
    {
        private NativeInStreamReader? _nativeReader = null;

        public static CompressSetInStream Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetInStream(nativeInterfaceObject);
        }

        public void ReleaseInStream()
        {
            if (_nativeReader is not null)
            {
                try
                {
                    var result = NativeInterOp.ICompressSetInStream__ReleaseInStream(NativeInterfaceObject);
                    if (result != HRESULT.S_OK)
                        throw result.GetExceptionFromHRESULT();
                }
                finally
                {
                    _nativeReader = null;
                }
            }
        }

        public void SetInStream(NativeInStreamReader inStreamReader)
        {
            if (inStreamReader is null)
                throw new ArgumentNullException(nameof(inStreamReader));

            var success = false;
            try
            {
                try
                {
                    ReleaseInStream();
                }
                catch (Exception)
                {
                }

                // To prevent the delegate from being released at an unintended timing by the garbage collector, associate the delegate with the class field.
                _nativeReader = inStreamReader;

                var result = NativeInterOp.ICompressSetInStream__SetInStream(NativeInterfaceObject, _nativeReader);
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
