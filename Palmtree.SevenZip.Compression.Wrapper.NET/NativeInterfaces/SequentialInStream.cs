using System;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class SequentialInStream
        : Unknown
    {
        public static SequentialInStream Create(IntPtr nativeInterfaceObject)
        {
            if (nativeInterfaceObject == IntPtr.Zero)
                throw new ArgumentNullException(nameof(nativeInterfaceObject));

            return new SequentialInStream(nativeInterfaceObject);
        }

        public UInt32 Read(Span<Byte> bytes)
        {
            var result = NativeInterOp.ISequentialInStream__Read(NativeInterfaceObject, bytes, out var processedSize);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return processedSize;
        }

        public override Unknown QueryInterface(Type interfaceType)
        {
            ArgumentNullException.ThrowIfNull(interfaceType);

            if (interfaceType.GUID == typeof(SequentialInStream).GUID)
                return this;
            else if (interfaceType.GUID == typeof(Unknown).GUID)
                return this;
            else
                throw new ArgumentException($"Type {GetType().FullName} does not implement interface {GetType().FullName}.", nameof(interfaceType));
        }
    }
}
