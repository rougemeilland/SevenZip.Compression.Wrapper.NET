using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetCoderProperties
    {
        public static CompressSetCoderProperties Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetCoderProperties(nativeInterfaceObject);
        }

        public void SetCoderProperties(ICoderProperties propertiesGetter)
        {
            if (propertiesGetter is null)
                throw new ArgumentNullException(nameof(propertiesGetter));

            var result = NativeInterOp.ICompressSetCoderProperties__SetCoderProperties(NativeInterfaceObject, propertiesGetter.EnumerateProperties());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
