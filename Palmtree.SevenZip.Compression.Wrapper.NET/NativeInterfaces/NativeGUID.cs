﻿using System;
using System.Runtime.InteropServices;

namespace SevenZip.Compression.NativeInterfaces
{
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 16)]
    unsafe struct NativeGUID
    {
        public UInt32 Data1;
        public UInt16 Data2;
        public UInt16 Data3;
        public fixed Byte Data4[8];

        public static NativeGUID FromManagedGuidToNativeGuid(Guid managedGuid)
        {
            unsafe
            {
                NativeGUID nativeGuid;
                CopyFromManagedGuidToNativeGuid(managedGuid, &nativeGuid);
                return nativeGuid;
            }
        }

        public static void CopyFromManagedGuidToNativeGuid(Guid managedGuid, ref NativeGUID nativeGuid)
        {
            unsafe
            {
                fixed (NativeGUID* nativeGuidPtr = &nativeGuid)
                {
                    CopyFromManagedGuidToNativeGuid(managedGuid, nativeGuidPtr);
                }
            }
        }
        public static unsafe void CopyFromManagedGuidToNativeGuid(Guid managedGuid, NativeGUID* nativeGuid)
        {
            if (!managedGuid.TryWriteBytes(new Span<Byte>(nativeGuid, sizeof(NativeGUID))))
                throw new Exception("Failed to copy the GUID.");
        }
    }
}
