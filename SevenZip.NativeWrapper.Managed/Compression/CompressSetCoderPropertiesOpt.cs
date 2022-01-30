using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetCoderPropertiesOpt
        : Unknown, ICompressSetCoderPropertiesOpt
    {
        protected CompressSetCoderPropertiesOpt(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetCoderPropertiesOpt Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetCoderPropertiesOpt(nativeInterfaceObject);
        }

        void ICompressSetCoderPropertiesOpt.SetCoderPropertiesOpt(ICoderProperties propertiesGetter)
        {
            var result = UnmanagedEntryPoint.ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt(NativeInterfaceObject, propertiesGetter.EnumerateProperties());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
