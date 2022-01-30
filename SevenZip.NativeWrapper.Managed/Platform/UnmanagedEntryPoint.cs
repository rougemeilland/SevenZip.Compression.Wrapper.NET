using SevenZip.NativeInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SevenZip.NativeWrapper.Managed.Platform
{
    static partial class UnmanagedEntryPoint
    {
        private static DateTime _fileTimeOriginForDateTime;
        private static DateTimeOffset _fileTimeOriginForDateTimeOffset;

        static UnmanagedEntryPoint()
        {
            _fileTimeOriginForDateTime = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _fileTimeOriginForDateTimeOffset = new DateTimeOffset(1601, 1, 1, 0, 0, 0, TimeSpan.Zero);
        }

        [DllImport("SevenZip.NativeWrapper.Unmanaged", EntryPoint = "EXPORTED_ICompressCodecsInfo__Create")]
        public static extern HRESULT ICompressCodecsInfo_Create([MarshalAs(UnmanagedType.LPWStr)] string locationPath, out IntPtr obj);

        public static HRESULT ISequentialInStream__Read(IntPtr ifp, Span<Byte> buffer, out UInt32 processedSize)
        {
            unsafe
            {
                fixed (Byte* bufferPtr = buffer)
                {
                    return ISequentialInStream__Read(ifp, bufferPtr, (UInt32)buffer.Length, out processedSize);
                }
            }
        }

        public static HRESULT ISequentialOutStream__Write(IntPtr ifp, ReadOnlySpan<Byte> buffer, out UInt32 processedSize)
        {
            unsafe
            {
                fixed (Byte* bufferPtr = buffer)
                {
                    return ISequentialOutStream__Write(ifp, bufferPtr, (UInt32)buffer.Length, out processedSize);
                }
            }
        }

        public static HRESULT ICompressProgressInfo__SetRatioInfo(IntPtr ifp, UInt64? inSize, UInt64? outSize)
        {
            unsafe
            {
                UInt64 inSizeBuffer;
                UInt64 outSizeBuffer;
                return ICompressProgressInfo__SetRatioInfo(ifp, inSize.ToPointer(&inSizeBuffer), outSize.ToPointer(&outSizeBuffer));
            }
        }

        public static HRESULT ICompressCoder__Code(IntPtr ifp, NativeInterface.IO.SequentialInStreamReader inStreamReader, NativeInterface.IO.SequentialOutStreamWriter outStreamWriter, UInt64? inSize, UInt64? outSize, NativeInterface.Compression.CompressProgressInfoReporter? progressReporter)
        {
            unsafe
            {
                UInt64 inSizeBuffer;
                UInt64 outSizeBuffer;
                return
                    ICompressCoder__Code(
                        ifp,
                        inStreamReader.ToNativeDelegate(),
                        outStreamWriter.ToNativeDelegate(),
                        inSize.ToPointer(&inSizeBuffer),
                        outSize.ToPointer(&outSizeBuffer),
                        progressReporter is not null ? progressReporter.ToNativeDelegate() : null);
            }
        }

        public static HRESULT ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt(IntPtr ifp, IEnumerable<(CoderPropertyId propertyId, object propertyValue)> properties)
        {
            return SetNativeProperties(ifp, properties.Where(property => property.propertyId == CoderPropertyId.ExpectedDataSize).ToList());
        }

        public static HRESULT ICompressSetCoderProperties__SetCoderProperties(IntPtr ifp, IEnumerable<(CoderPropertyId propertyId, object propertyValue)> properties)
        {
            return SetNativeProperties(ifp, properties.Where(property => property.propertyId != CoderPropertyId.ExpectedDataSize).ToList());
        }

        public static HRESULT ICompressSetDecoderProperties2__SetDecoderProperties2(IntPtr ifp, ReadOnlySpan<Byte> data)
        {
            unsafe
            {
                fixed (Byte* dataPtr = data)
                {
                    return ICompressSetDecoderProperties2__SetDecoderProperties2(ifp, dataPtr, (UInt32)data.Length);
                }
            }
        }

        public static HRESULT ICompressWriteCoderProperties__WriteCoderProperties(IntPtr ifp, NativeInterface.IO.SequentialOutStreamWriter outStreamWriter)
        {
            return ICompressWriteCoderProperties__WriteCoderProperties(ifp, outStreamWriter.ToNativeDelegate());
        }

        public static HRESULT ICompressReadUnusedFromInBuf__ReadUnusedFromInBuf(IntPtr ifp, Span<Byte> buffer, out UInt32 processedSize)
        {
            unsafe
            {
                fixed (Byte* bufferPtr = buffer)
                {
                    return ICompressReadUnusedFromInBuf__ReadUnusedFromInBuf(ifp, bufferPtr, (UInt32)buffer.Length, out processedSize);
                }
            }
        }

        public static HRESULT ICompressSetOutStreamSize__SetOutStreamSize(IntPtr ifp, UInt64? outSize)
        {
            unsafe
            {
                UInt64 outSizeBuffer;
                return ICompressSetOutStreamSize__SetOutStreamSize(ifp, outSize.ToPointer(&outSizeBuffer));
            }
        }

        private static HRESULT SetNativeProperties(IntPtr ifp, ICollection<(CoderPropertyId propertyId, object propertyValue)> propertiesList)
        {
            unsafe
            {
                CoderPropID* propertyIds = stackalloc CoderPropID[propertiesList.Count];
                PROPVARIANT* propertyValues = stackalloc PROPVARIANT[propertiesList.Count];
                var result =
                    SetNativeProperties(
                        propertiesList.GetEnumerator(),
                        propertyIds,
                        propertyValues,
                        0);
                if (result != HRESULT.S_OK)
                    return result;
                return ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt(ifp, propertyIds, propertyValues, (UInt32)propertiesList.Count);
            }
        }

        private static unsafe HRESULT SetNativeProperties(IEnumerator<(CoderPropertyId propertyId, object propertyValue)> propertiesEnumerator, CoderPropID* nativeProvertyIds, PROPVARIANT* nativePropertyValues, UInt32 currentIndex)
        {
            try
            {
                while (propertiesEnumerator.MoveNext())
                {
                    var (propertyId, propertyValue) = propertiesEnumerator.Current;
                    nativeProvertyIds[currentIndex] = (CoderPropID)propertyId;
                    var nativePropertyValue = &nativePropertyValues[currentIndex];
                    PROPVARIANT.Clear(nativePropertyValue);
                    if (propertyValue is Boolean)
                    {
                        nativePropertyValue->ValueType = PropertyValueType.VT_BOOL;
                        nativePropertyValue->BooleanValue = (Boolean)propertyValue ? PROPVARIANT_BOOLEAN_VALUE.TRUE : PROPVARIANT_BOOLEAN_VALUE.FALSE;
                        ++currentIndex;
                    }
                    else if (propertyValue is UInt32)
                    {
                        nativePropertyValue->ValueType = PropertyValueType.VT_UI4;
                        nativePropertyValue->UInt32Value = (UInt32)propertyValue;
                        ++currentIndex;
                    }
                    else if (propertyValue is UInt64)
                    {
                        nativePropertyValue->ValueType = PropertyValueType.VT_UI4;
                        nativePropertyValue->UInt64Value = (UInt64)propertyValue;
                        ++currentIndex;
                    }
                    else if (propertyValue is DateTime)
                    {
                        var dateTime = (DateTime)propertyValue;
                        if (dateTime.Kind == DateTimeKind.Unspecified)
                            throw new NotSupportedException("DateTime objects whose Kind property value is 'DateTimeKind.Unspecified' cannot be used as property values.");
                        nativePropertyValue->ValueType = PropertyValueType.VT_UI4;
                        nativePropertyValue->FileTimeValue.DateTime = (UInt64)(dateTime.ToUniversalTime() - _fileTimeOriginForDateTime).Ticks;
                        ++currentIndex;
                    }
                    else if (propertyValue is DateTimeOffset)
                    {
                        nativePropertyValue->ValueType = PropertyValueType.VT_UI4;
                        nativePropertyValue->FileTimeValue.DateTime = (UInt64)(((DateTimeOffset)propertyValue).ToUniversalTime() - _fileTimeOriginForDateTimeOffset).Ticks;
                        ++currentIndex;
                    }
                    else if (propertyValue is string)
                    {
                        fixed (char* ptr = (string)propertyValue)
                        {
                            nativePropertyValue->ValueType = PropertyValueType.VT_BSTR;
                            nativePropertyValue->StringValue = ptr;
                            // Call this method recursively to continue processing the next property while keeping the string address fixed
                            return SetNativeProperties(propertiesEnumerator, nativeProvertyIds, nativePropertyValue, currentIndex + 1);
                        }
                    }
                    else if (propertyValue is NativeInterface.Compression.MatchFinderType)
                    {
                        var matchFinderSymbol = propertyValue.ToString();
#if DEBUG
                        if (!string.Equals(matchFinderSymbol, "BT2", StringComparison.OrdinalIgnoreCase)
                            && !string.Equals(matchFinderSymbol, "BT3", StringComparison.OrdinalIgnoreCase)
                            && !string.Equals(matchFinderSymbol, "BT4", StringComparison.OrdinalIgnoreCase)
                            && !string.Equals(matchFinderSymbol, "BT5", StringComparison.OrdinalIgnoreCase)
                            && !string.Equals(matchFinderSymbol, "HC4", StringComparison.OrdinalIgnoreCase)
                            && !string.Equals(matchFinderSymbol, "HC5", StringComparison.OrdinalIgnoreCase))
                            throw new Exception();
#endif
                        fixed (char* ptr = matchFinderSymbol)
                        {
                            nativePropertyValue->ValueType = PropertyValueType.VT_BSTR;
                            nativePropertyValue->StringValue = ptr;
                            // Call this method recursively to continue processing the next property while keeping the string address fixed
                            return SetNativeProperties(propertiesEnumerator, nativeProvertyIds, nativePropertyValue, currentIndex + 1);
                        }
                    }
                    else
                        throw new Exception($"The type of the value is unknown.: {propertyValue.GetType().FullName}");
                }
                return HRESULT.S_OK;
            }
            catch (Exception ex)
            {
                return (HRESULT)ex.HResult;
            }
        }
    }
}
