using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.win.x64.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.win.x64.Compression
{
    class CompressSetOutStreamSize
        : Unknown, ICompressSetOutStreamSize
    {
        protected CompressSetOutStreamSize(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetOutStreamSize Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetOutStreamSize(nativeInterfaceObject);
        }

        void ICompressSetOutStreamSize.SetOutStreamSize(UInt64? outSize)
        {
            var result = UnmanagedEntryPoint.ICompressSetOutStreamSize__SetOutStreamSize(NativeInterfaceObject, outSize);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
