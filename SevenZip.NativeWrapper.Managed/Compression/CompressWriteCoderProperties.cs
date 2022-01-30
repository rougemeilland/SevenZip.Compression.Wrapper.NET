using SevenZip.NativeInterface.Compression;
using SevenZip.NativeInterface.IO;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressWriteCoderProperties
        : Unknown, ICompressWriteCoderProperties
    {
        protected CompressWriteCoderProperties(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressWriteCoderProperties Create(IntPtr nativeInterfaceObject)
        {
            return new CompressWriteCoderProperties(nativeInterfaceObject);
        }

        void ICompressWriteCoderProperties.WriteCoderProperties(SequentialOutStreamWriter outStreamWriter)
        {
            var result = UnmanagedEntryPoint.ICompressWriteCoderProperties__WriteCoderProperties(NativeInterfaceObject, outStreamWriter);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
