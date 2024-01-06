using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressSetFinishMode
    {
        public static CompressSetFinishMode Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressSetFinishMode(nativeInterfaceObject);
        }

        public void SetFinishMode(Boolean finishMode)
        {
            var result = NativeInterOp.ICompressSetFinishMode__SetFinishMode(NativeInterfaceObject, finishMode ? 1U : 0U);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
