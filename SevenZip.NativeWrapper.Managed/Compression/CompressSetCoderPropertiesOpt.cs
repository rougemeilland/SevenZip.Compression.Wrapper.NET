using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.win.x64.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.win.x64.Compression
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
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetCoderPropertiesOpt(nativeInterfaceObject);
        }

        void ICompressSetCoderPropertiesOpt.SetCoderPropertiesOpt(ICoderProperties propertiesGetter)
        {
            if (propertiesGetter is null)
                throw new ArgumentNullException(nameof(propertiesGetter));

            var result = UnmanagedEntryPoint.ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt(NativeInterfaceObject, propertiesGetter.EnumerateProperties());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
