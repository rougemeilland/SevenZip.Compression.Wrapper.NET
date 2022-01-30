using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
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
