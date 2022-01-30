using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetBufSize
        : Unknown, ICompressSetBufSize
    {
        protected CompressSetBufSize(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetBufSize Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetBufSize(nativeInterfaceObject);
        }

        void ICompressSetBufSize.SetInBufSize(UInt32 streamIndex, UInt32 size)
        {
            var result = UnmanagedEntryPoint.ICompressSetBufSize__SetInBufSize(NativeInterfaceObject, streamIndex, size);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }

        void ICompressSetBufSize.SetOutBufSize(UInt32 streamIndex, UInt32 size)
        {
            var result = UnmanagedEntryPoint.ICompressSetBufSize__SetOutBufSize(NativeInterfaceObject, streamIndex, size);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
