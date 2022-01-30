using System;
using System.Runtime.InteropServices;

namespace SevenZip.NativeWrapper.Managed
{

#if _PLATFORM_WINDOWS_X64
    [StructLayout(LayoutKind.Explicit, Pack = 8, Size = 24)]
    unsafe struct PROPVARIANT
    {
        [FieldOffset(0)]
        public PropertyValueType ValueType;

        [FieldOffset(8)]
        public PROPVARIANT_BOOLEAN_VALUE BooleanValue;

        [FieldOffset(8)]
        public UInt32 UInt32Value;

        [FieldOffset(8)]
        public UInt64 UInt64Value;

        [FieldOffset(8)]
        public Char* StringValue;

        [FieldOffset(8)]
        public NativeFILETIME FileTimeValue;

        public static unsafe void Clear(PROPVARIANT* propertyValue)
        {
            UInt64* ptr = (UInt64*)propertyValue;
            *ptr++ = 0;
            *ptr = 0;
        }

        public static void Clear(ref PROPVARIANT propertyValue)
        {
            unsafe
            {
                fixed (PROPVARIANT* propertyValuePtr = &propertyValue)
                {
                    Clear(propertyValuePtr);
                }
            }
        }
    }
#elif _PLATFORM_WINDOWS_X86
    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 16)]
    unsafe struct PROPVARIANT
    {
        [FieldOffset(0)]
        public PropertyValueType ValueType;

        [FieldOffset(8)]
        public PROPVARIANT_BOOLEAN_VALUE BooleanValue;

        [FieldOffset(8)]
        public UInt32 UInt32Value;

        [FieldOffset(8)]
        public UInt64 UInt64Value;

        [FieldOffset(8)]
        public Char* StringValue;

        [FieldOffset(8)]
        public NativeFILETIME FileTimeValue;

        public static unsafe void Clear(PROPVARIANT* propertyValue)
        {
            UInt32* ptr = (UInt32*)propertyValue;
            *ptr++ = 0;
            *ptr++ = 0;
            *ptr++ = 0;
            *ptr = 0;
        }

        public static void Clear(ref PROPVARIANT propertyValue)
        {
            unsafe
            {
                fixed (PROPVARIANT* propertyValuePtr = &propertyValue)
                {
                    Clear(propertyValuePtr);
                }
            }
        }
    }
#elif _PLATFORM_LINUX_X64
#error Define a PROPVARIANT structure. The layout of the PROPVARIANT structure should match the one on the 7-zip source code.
#elif _PLATFORM_LINUX_X86
#error Define a PROPVARIANT structure. The layout of the PROPVARIANT structure should match the one on the 7-zip source code.
#elif _PLATFORM_MACOS_X64
#error Define a PROPVARIANT structure. The layout of the PROPVARIANT structure should match the one on the 7-zip source code.
#elif _PLATFORM_MACOS_X86
#error Define a PROPVARIANT structure. The layout of the PROPVARIANT structure should match the one on the 7-zip source code.
#else
#error Not supported platform
#endif
}
