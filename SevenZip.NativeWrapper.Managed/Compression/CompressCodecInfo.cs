using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    class CompressCodecInfo
        : ICompressCodecInfo, IDisposable
    {
        private static object _lockObject;

        // [Note]
        //  When calling ICompressCodecsInfo__GetProperty, if the PROPVARIANT structure returns the VT_BSTR type,
        // the PROPVARIANT structure is set to a pointer to the memory acquired inside 7-zip.
        //  There is no way to free this memory from the caller.
        //  However, by calling ICompressCodecsInfo__GetProperty again using the same PROPVARIANT structure,
        // 7-zip will release the memory pointed to by the pointer already set before setting the new value in the PROPVARIANT structure.
        //  Due to this property, the PROPVARIANT structure used in ICompressCodecsInfo__GetProperty should be shared as much as possible.
        //  Also, for the same reason, the PROPVARIANT structure must first be initialized to zero.
        //  And if possible, you should first call CompressCodecsInfo__GetProperty, which returns the VT_BSTR type.
        private static PROPVARIANT _propertyValueBuffer;
        private static NativeGUID _compressCoderInterfaceId;

        private IntPtr _compressCodecsInfo;
        private bool _isDisposed;

        static CompressCodecInfo()
        {
            _lockObject = new object();
            PROPVARIANT.Clear(ref _propertyValueBuffer);
            NativeGUID.CopyFromManagedGuidToNativeGuid(typeof(ICompressCoder).GUID, ref _compressCoderInterfaceId);
        }

        private CompressCodecInfo(IntPtr compressCodecsInfo, int index, UInt64 id, string codecName, Guid coderClassId, CoderType coderType, bool isSupportedICompressCoder)
        {
            _isDisposed = false;
            _compressCodecsInfo = compressCodecsInfo;
            Index = index;
            ID = id;
            CodecName = codecName;
            CoderClassId = coderClassId;
            CoderType = coderType;
            IsSupportedICompressCoder = isSupportedICompressCoder;
        }

        ~CompressCodecInfo()
        {
            Dispose(disposing: false);
        }

        public Int32 Index { get; }

        public UInt64 ID { get; }

        public string CodecName { get; }

        public Guid CoderClassId { get; }

        public CoderType CoderType { get; }

        public bool IsSupportedICompressCoder { get; }

        public static ICompressCodecInfo? Create(IntPtr compressCodecsInfo, Int32 index, CoderType coderType)
        {
            lock (_lockObject)
            {
                var ifp = compressCodecsInfo;
                var codecName = GetCodecName(ifp, index, ref _propertyValueBuffer);
                Console.WriteLine($"index={index}, name={codecName}");
                switch (coderType)
                {
                    case CoderType.Decoder:
                        if (IsDecoderAssigned(ifp, index, ref _propertyValueBuffer))
                        {
                            var coderClassId = GetDecoderClassId(ifp, index, ref _propertyValueBuffer);
                            var codecId = GetCodecId(ifp, index, ref _propertyValueBuffer);
                            IntPtr coder = IntPtr.Zero;
                            try
                            {
                                coder = CreateCompressDecoder(ifp, index, ref _compressCoderInterfaceId);
                                UnmanagedEntryPoint.IUnknown__AddRef(ifp);
                                return new CompressCodecInfo(ifp, index, codecId, codecName, coderClassId, coderType, coder != IntPtr.Zero);
                            }
                            finally
                            {
                                if (coder != IntPtr.Zero)
                                    UnmanagedEntryPoint.IUnknown__Release(coder);
                            }
                        }
                        else
                            return null;
                    case CoderType.Encoder:
                        if (IsEncoderAssigned(ifp, index, ref _propertyValueBuffer))
                        {
                            var coderClassId = GetEncoderClassId(ifp, index, ref _propertyValueBuffer);
                            var codecId = GetCodecId(ifp, index, ref _propertyValueBuffer);
                            IntPtr coder = IntPtr.Zero;
                            try
                            {
                                coder = CreateCompressEncoder(ifp, index, ref _compressCoderInterfaceId);
                                UnmanagedEntryPoint.IUnknown__AddRef(ifp);
                                return new CompressCodecInfo(ifp, index, codecId, codecName, coderClassId, coderType, coder != IntPtr.Zero);
                            }
                            finally
                            {
                                if (coder != IntPtr.Zero)
                                    UnmanagedEntryPoint.IUnknown__Release(coder);
                            }
                        }
                        else
                            return null;
                    default:
                        return null;
                }
            }
        }

        public ICompressCoder CreateCompressCoder()
        {
            return CompressCoder.Create(CreateCompressEncoder(_compressCodecsInfo, Index, ref _compressCoderInterfaceId));
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                }
                if (_compressCodecsInfo != IntPtr.Zero)
                {
                    UnmanagedEntryPoint.ICompressSetInStream__ReleaseInStream(_compressCodecsInfo);
                    _compressCodecsInfo = IntPtr.Zero;
                }
                _isDisposed = true;
            }
        }

        private static UInt64 GetCodecId(IntPtr ifp, Int32 index, ref PROPVARIANT valueBuffer)
        {
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__GetProperty(ifp, (UInt32)index, MethodPropID.ID, ref valueBuffer);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            if (valueBuffer.ValueType != PropertyValueType.VT_UI8)
                throw new Exception("Unexpected value type.");
            return valueBuffer.UInt64Value;
        }

        private static string GetCodecName(IntPtr ifp, Int32 index, ref PROPVARIANT valueBuffer)
        {
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__GetProperty(ifp, (UInt32)index, MethodPropID.Name, ref valueBuffer);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            if (valueBuffer.ValueType != PropertyValueType.VT_BSTR)
                throw new Exception("Unexpected value type.");
            unsafe
            {
                return new string(valueBuffer.StringValue);
            }
        }

        private static bool IsDecoderAssigned(IntPtr ifp, Int32 index, ref PROPVARIANT valueBuffer)
        {
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__GetProperty(ifp, (UInt32)index, MethodPropID.DecoderIsAssigned, ref valueBuffer);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            if (valueBuffer.ValueType != PropertyValueType.VT_BOOL)
                throw new Exception("Unexpected value type.");
            return valueBuffer.BooleanValue != PROPVARIANT_BOOLEAN_VALUE.FALSE;
        }

        private static bool IsEncoderAssigned(IntPtr ifp, Int32 index, ref PROPVARIANT valueBuffer)
        {
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__GetProperty(ifp, (UInt32)index, MethodPropID.EncoderIsAssigned, ref valueBuffer);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            if (valueBuffer.ValueType != PropertyValueType.VT_BOOL)
                throw new Exception("Unexpected value type.");
            return valueBuffer.BooleanValue != PROPVARIANT_BOOLEAN_VALUE.FALSE;
        }

        private static Guid GetDecoderClassId(IntPtr ifp, Int32 index, ref PROPVARIANT valueBuffer)
        {
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__GetProperty(ifp, (UInt32)index, MethodPropID.Decoder, ref valueBuffer);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            if (valueBuffer.ValueType != PropertyValueType.VT_BSTR)
                throw new Exception("Unexpected value type.");
            unsafe
            {
                return new Guid(new ReadOnlySpan<Byte>(valueBuffer.StringValue, sizeof(NativeGUID)));
            }
        }

        private static Guid GetEncoderClassId(IntPtr ifp, Int32 index, ref PROPVARIANT valueBuffer)
        {
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__GetProperty(ifp, (UInt32)index, MethodPropID.Encoder, ref valueBuffer);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            if (valueBuffer.ValueType != PropertyValueType.VT_BSTR)
                throw new Exception("Unexpected value type.");
            unsafe
            {
                return new Guid(new ReadOnlySpan<Byte>(valueBuffer.StringValue, sizeof(NativeGUID)));
            }
        }

        private static IntPtr CreateCompressDecoder(IntPtr ifp, Int32 index, ref NativeGUID interfaceId)
        {
            IntPtr coder;
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__CreateDecoder(ifp, (UInt32)index, ref interfaceId, out coder);
            if (result == HRESULT.S_OK)
                return coder;
            else if (result == HRESULT.E_NOINTERFACE)
                return IntPtr.Zero;
            else
                throw result.GetExceptionFromHRESULT();
        }

        private static IntPtr CreateCompressEncoder(IntPtr ifp, Int32 index, ref NativeGUID interfaceId)
        {
            IntPtr coder;
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__CreateEncoder(ifp, (UInt32)index, ref interfaceId, out coder);
            if (result == HRESULT.S_OK)
                return coder;
            else if (result == HRESULT.E_NOINTERFACE)
                return IntPtr.Zero;
            else
                throw result.GetExceptionFromHRESULT();
        }
    }
}
