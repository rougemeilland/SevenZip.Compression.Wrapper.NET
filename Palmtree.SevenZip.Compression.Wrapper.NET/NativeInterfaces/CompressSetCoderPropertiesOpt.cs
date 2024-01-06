using System;

namespace SevenZip.Compression.NativeInterfaces
{
    /// <summary>
    /// An interface for setting various properties on the coder.
    /// </summary>
    internal partial class CompressSetCoderPropertiesOpt
    {
        public static CompressSetCoderPropertiesOpt Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetCoderPropertiesOpt(nativeInterfaceObject);
        }

        public void SetCoderPropertiesOpt(ICoderProperties propertiesGetter)
        {
            if (propertiesGetter is null)
                throw new ArgumentNullException(nameof(propertiesGetter));

            var result = NativeInterOp.ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt(NativeInterfaceObject, propertiesGetter.EnumerateProperties());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
