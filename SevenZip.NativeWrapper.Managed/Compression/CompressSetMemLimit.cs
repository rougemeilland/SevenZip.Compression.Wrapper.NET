using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressSetMemLimit
        : Unknown, ICompressSetMemLimit
    {
        protected CompressSetMemLimit(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ICompressSetMemLimit Create(IntPtr nativeInterfaceObject)
        {
            return new CompressSetMemLimit(nativeInterfaceObject);
        }

        void ICompressSetMemLimit.SetMemLimit(UInt64 memUsage)
        {
            var result = UnmanagedEntryPoint.ICompressSetMemLimit__SetMemLimit(NativeInterfaceObject, memUsage);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
