using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Palmtree;

namespace SevenZip.Compression.NativeInterfaces
{
    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = 24)]
    internal unsafe struct PROPVARIANT
    {
        private enum PROPVARIANT_BOOLEAN_VALUE
            : UInt16
        {
            FALSE = 0x0000,
            TRUE = 0xffff,
        }

        public static readonly Int32 SizeOfPropVariant;
        public static readonly Int32 SizeOfChar;

        static PROPVARIANT()
        {
            SizeOfPropVariant = NativeInterOp.Global__GetSizeOfPROPVARIANT();
            SizeOfChar = NativeInterOp.Global__GetSizeOfOleChar();
        }

        [FieldOffset(0)]
        public PropertyValueType ValueType;

        [FieldOffset(8)]
        private PROPVARIANT_BOOLEAN_VALUE _booleanValue;

        [FieldOffset(8)]
        public UInt32 UInt32Value;

        [FieldOffset(8)]
        public UInt64 UInt64Value;

        [FieldOffset(8)]
        public void* StringValue;

        [FieldOffset(8)]
        public NativeFILETIME FileTimeValue;

        public Boolean ManagedBooleanValue
        {
            readonly get
            {
                if (ValueType != PropertyValueType.VT_BOOL)
                    throw new ApplicationException("Unexpected value type.");

                return _booleanValue != PROPVARIANT_BOOLEAN_VALUE.FALSE;
            }

            set
            {
                ValueType = PropertyValueType.VT_BOOL;
                _booleanValue = value ? PROPVARIANT_BOOLEAN_VALUE.TRUE : PROPVARIANT_BOOLEAN_VALUE.FALSE;
            }
        }

        public readonly String ManagedStringValue
        {
            get
            {
                if (ValueType != PropertyValueType.VT_BSTR)
                    throw new ApplicationException("Unexpected value type.");

                Encoding encoding;
                Int32 count;
                var sizeOfChar = NativeInterOp.Global__GetSizeOfOleChar();
                if (sizeOfChar == sizeof(UInt16))
                {
                    encoding = Encoding.Unicode;
                    count = 0;
                    for (var p = (UInt16*)StringValue; *p != 0; ++p)
                        count += sizeof(UInt16);
                }
                else if (sizeOfChar == sizeof(UInt32))
                {
                    encoding = Encoding.UTF32;
                    count = 0;
                    for (var p = (UInt32*)StringValue; *p != 0; ++p)
                        count += sizeof(UInt32);
                }
                else
                {
                    throw Validation.GetFailErrorException();
                }

                return encoding.GetString(new Span<Byte>((Byte*)StringValue, count));
            }
        }

        public readonly Guid ManagedGuidValue
        {
            get
            {
                if (ValueType != PropertyValueType.VT_BSTR)
                    throw new ApplicationException("Unexpected value type.");

                return new Guid(new ReadOnlySpan<Byte>((Byte*)StringValue, sizeof(NativeGUID)));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe PROPVARIANT* GetElementOfArray(PROPVARIANT* array, Int32 index)
            => (PROPVARIANT*)((Byte*)array + index * SizeOfPropVariant);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            fixed (PROPVARIANT* thiPropertyValuePtr = &this)
            {
                new Span<Byte>((Byte*)thiPropertyValuePtr, SizeOfPropVariant).Clear();
            }
        }
    }
}
