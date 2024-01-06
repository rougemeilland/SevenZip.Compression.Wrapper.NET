using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class SequentialOutStream
        : Unknown
    {
        public static SequentialOutStream Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new SequentialOutStream(nativeInterfaceObject);
        }

        public UInt32 Write(ReadOnlySpan<Byte> bytes)
        {
            var result = NativeInterOp.ISequentialOutStream__Write(NativeInterfaceObject, bytes, out var processedSize);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();

            return processedSize;
        }

        public override Unknown QueryInterface(Type interfaceType)
        {
            if (interfaceType is null)
                throw new ArgumentNullException(nameof(interfaceType));

            if (interfaceType.GUID == typeof(SequentialOutStream).GUID)
                return this;
            else if (interfaceType.GUID == typeof(Unknown).GUID)
                return this;
            else
                throw new ArgumentException($"Type {GetType().FullName} does not implement interface {GetType().FullName}.", nameof(interfaceType));
        }
    }
}
