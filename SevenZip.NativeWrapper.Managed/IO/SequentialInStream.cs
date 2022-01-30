using SevenZip.NativeInterface.IO;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.IO
{
    class SequentialInStream
        : Unknown, ISequentialInStream
    {
        protected SequentialInStream(IntPtr nativeInterfaceObject)
            : base(nativeInterfaceObject)
        {
        }

        public static ISequentialInStream Create(IntPtr nativeInterfaceObject)
        {
            return new SequentialInStream(nativeInterfaceObject);
        }

        UInt32 ISequentialInStream.Read(Span<Byte> buffer)
        {
            UInt32 processedSize;
            var result = UnmanagedEntryPoint.ISequentialInStream__Read(NativeInterfaceObject, buffer, out processedSize);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return processedSize;
        }
    }
}
