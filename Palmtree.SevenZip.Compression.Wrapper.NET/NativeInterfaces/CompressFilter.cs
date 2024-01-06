using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressFilter
    {
        public static CompressFilter Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressFilter(nativeInterfaceObject);
        }

        public void Init()
        {
            var result = NativeInterOp.ICompressFilter__Init(NativeInterfaceObject);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }

        public UInt32 Filter(Span<Byte> data)
            => NativeInterOp.ICompressFilter__Filter(NativeInterfaceObject, data);
    }
}
