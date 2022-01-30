using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetFinishMode
        : Unknown, ICompressSetFinishMode
    {
        protected CompressSetFinishMode(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetFinishMode Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetFinishMode(nativeInterfaceObject);
        }

        void ICompressSetFinishMode.SetFinishMode(bool finishMode)
        {
            var result = UnmanagedEntryPoint.ICompressSetFinishMode__SetFinishMode(NativeInterfaceObject, finishMode ? 1U : 0U);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
