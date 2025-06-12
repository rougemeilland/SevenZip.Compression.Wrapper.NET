using System;
using System.IO;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressWriteCoderProperties
    {
        public static CompressWriteCoderProperties Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new CompressWriteCoderProperties(nativeInterfaceObject);
        }

        public void WriteCoderProperties(ISequentialOutputByteStream outStream)
        {
            ArgumentNullException.ThrowIfNull(outStream);

            var result = NativeInterOp.ICompressWriteCoderProperties__WriteCoderProperties(NativeInterfaceObject, outStream.FromOutputStreamToNativeDelegate());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }

        public void WriteCoderProperties(Stream outStream)
        {
            ArgumentNullException.ThrowIfNull(outStream);
            if (!outStream.CanWrite)
                throw new ArgumentException("The specified stream does not support writing.", nameof(outStream));

            var result = NativeInterOp.ICompressWriteCoderProperties__WriteCoderProperties(NativeInterfaceObject, outStream.FromOutputStreamToNativeDelegate());
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
        }
    }
}
