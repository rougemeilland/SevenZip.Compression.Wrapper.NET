using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetCoderProperties
        : Unknown, ICompressSetCoderProperties
    {
        protected CompressSetCoderProperties(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetCoderProperties Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetCoderProperties(nativeInterfaceObject);
        }

        void ICompressSetCoderProperties.SetCoderProperties(ICoderProperties propertiesGetter)
        {
            var result = UnmanagedEntryPoint.ICompressSetCoderProperties__SetCoderProperties(NativeInterfaceObject, propertiesGetter.EnumerateProperties());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
