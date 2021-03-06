using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetDecoderProperties2
        : Unknown, ICompressSetDecoderProperties2
    {
        protected CompressSetDecoderProperties2(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetDecoderProperties2 Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetDecoderProperties2(nativeInterfaceObject);
        }

        void ICompressSetDecoderProperties2.SetDecoderProperties2(ReadOnlySpan<Byte> data)
        {
            var result = UnmanagedEntryPoint.ICompressSetDecoderProperties2__SetDecoderProperties2(NativeInterfaceObject, data);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
