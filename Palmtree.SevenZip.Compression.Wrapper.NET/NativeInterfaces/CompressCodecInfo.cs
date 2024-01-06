using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SevenZip.Compression.NativeInterfaces
{
    /// <summary>
    /// An interface that allows you to refer to codec information.
    /// </summary>
    internal class CompressCodecInfo
        : Unknown
    {
        private static readonly Object _lockObject;

        // [Note]
        //  When calling ICompressCodecsInfo__GetProperty, if the PROPVARIANT structure returns the VT_BSTR type,
        // the PROPVARIANT structure is set to a pointer to the memory acquired inside 7-zip.
        //  There is no way to free this memory from the caller.
        //  However, by calling ICompressCodecsInfo__GetProperty again using the same PROPVARIANT structure,
        // 7-zip will release the memory pointed to by the pointer already set before setting the new value in the PROPVARIANT structure.
        //  Due to this property, the PROPVARIANT structure used in ICompressCodecsInfo__GetProperty should be shared as much as possible.
        //  Also, for the same reason, the PROPVARIANT structure must first be initialized to zero.
        //  And if possible, you should first call CompressCodecsInfo__GetProperty, which returns the VT_BSTR type.
        private static NativeGUID _compressCoderInterfaceId;
        private static NativeGUID _compressCoder2InterfaceId;
        private static NativeGUID _compressFilterInterfaceId;

        static CompressCodecInfo()
        {
            _lockObject = new Object();
            NativeGUID.CopyFromManagedGuidToNativeGuid(typeof(CompressCoder).GUID, ref _compressCoderInterfaceId);
            NativeGUID.CopyFromManagedGuidToNativeGuid(typeof(CompressCoder2).GUID, ref _compressCoder2InterfaceId);
            NativeGUID.CopyFromManagedGuidToNativeGuid(typeof(CompressFilter).GUID, ref _compressFilterInterfaceId);
        }

        private CompressCodecInfo(
            IntPtr compressCodecsInfo,
            Int32 index,
            UInt64 id,
            String codecName,
            Guid coderClassId,
            UInt32 packStreams,
            Boolean isFilter,
            CoderType coderType,
            Boolean isSupportedICompressCoder,
            Boolean isSupportedICompressCoder2,
            Boolean isSupportedICompressFilter)
            : base(compressCodecsInfo)
        {
            Index = index;
            ID = id;
            CodecName = codecName;
            CoderClassId = coderClassId;
            PackStreams = packStreams;
            IsFilter = isFilter;
            CoderType = coderType;
            IsSupportedICompressCoder = isSupportedICompressCoder;
            IsSupportedICompressCoder2 = isSupportedICompressCoder2;
            IsSupportedICompressFilter = isSupportedICompressFilter;
        }

        /// <summary>
        /// An index number that identifies the codec.
        /// </summary>
        public Int32 Index { get; }

        /// <summary>
        /// The codec ID.
        /// </summary>
        public UInt64 ID { get; }

        /// <summary>
        /// The name of the codec.
        /// </summary>
        public String CodecName { get; }

        /// <summary>
        /// A UUID that identifies the codec.
        /// </summary>
        public Guid CoderClassId { get; }

        /// <summary>
        /// The number of compressed streams coded by the codec.
        /// </summary>
        public UInt32 PackStreams { get; }

        public Boolean IsFilter { get; }

        /// <summary>
        /// The type of coder (encoder or decoder) that can be created.
        /// </summary>
        public CoderType CoderType { get; }

        /// <summary>
        /// True is returned if a coder that implements the <see cref="CompressCoder"/> interface can be created. If not, false is returned.
        /// </summary>
        public Boolean IsSupportedICompressCoder { get; }

        /// <summary>
        /// True is returned if a coder that implements the <see cref="CompressCoder2"/> interface can be created. If not, false is returned.
        /// </summary>
        public Boolean IsSupportedICompressCoder2 { get; }

        /// <summary>
        /// True is returned if a coder that implements the <see cref="CompressFilter"/> interface can be created. If not, false is returned.
        /// </summary>
        public Boolean IsSupportedICompressFilter { get; }

        /// <summary>
        /// Create a coder that implements <see cref="CompressCoder"/>.
        /// </summary>
        /// <returns>
        /// The created object that implements the <see cref="CompressCoder"/> interface.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="CompressCoder"/> interface does not inherit from the <see cref="IDisposable"/> interface, but the created coder object implements the <see cref="IDisposable"/> interface.
        /// If you no longer need the created coder object, release it by calling <see cref="IDisposable.Dispose"/>.
        /// </para>
        /// <para>
        /// The sample code is as follows.:
        /// <code>
        /// ICompressCoder coderObject = codec.CreateCompressCoder();
        /// 
        /// ...
        /// 
        /// (coderObject as IDisposable)?.Dispose();
        /// </code>
        /// </para>
        /// </remarks>
        public CompressCoder CreateCompressCoder()
        {
            if (!IsSupportedICompressCoder)
                throw new NotSupportedException();

            return
                CompressCoder.Create(
                    CoderType switch
                    {
                        CoderType.Decoder => CreateCompressDecoder(NativeInterfaceObject, Index, ref _compressCoderInterfaceId),
                        CoderType.Encoder => CreateCompressEncoder(NativeInterfaceObject, Index, ref _compressCoderInterfaceId),
                        _ => throw new Exception($"Unknown coder type: {CoderType}"),
                    });
        }

        /// <summary>
        /// Create a coder that implements <see cref="CompressCoder2"/>.
        /// </summary>
        /// <returns>
        /// The created object that implements the <see cref="CompressCoder2"/> interface.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="CompressCoder2"/> interface does not inherit from the <see cref="IDisposable"/> interface, but the created coder object implements the <see cref="IDisposable"/> interface.
        /// If you no longer need the created coder object, release it by calling <see cref="IDisposable.Dispose"/>.
        /// </para>
        /// <para>
        /// The sample code is as follows.:
        /// <code>
        /// ICompressCoder2 coderObject = codec.CreateCompressCoder2();
        /// 
        /// ...
        /// 
        /// (coderObject as IDisposable)?.Dispose();
        /// </code>
        /// </para>
        /// </remarks>
        public CompressCoder2 CreateCompressCoder2()
        {
            if (!IsSupportedICompressCoder2)
                throw new NotSupportedException();

            return
                CompressCoder2.Create(
                    CoderType switch
                    {
                        CoderType.Decoder => CreateCompressDecoder(NativeInterfaceObject, Index, ref _compressCoder2InterfaceId),
                        CoderType.Encoder => CreateCompressEncoder(NativeInterfaceObject, Index, ref _compressCoder2InterfaceId),
                        _ => throw new Exception($"Unknown coder type: {CoderType}"),
                    });
        }

        /// <summary>
        /// Create a coder that implements <see cref="CompressFilter"/>.
        /// </summary>
        /// <returns>
        /// The created object that implements the <see cref="CompressFilter"/> interface.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="CompressFilter"/> interface does not inherit from the <see cref="IDisposable"/> interface, but the created coder object implements the <see cref="IDisposable"/> interface.
        /// If you no longer need the created coder object, release it by calling <see cref="IDisposable.Dispose"/>.
        /// </para>
        /// <para>
        /// The sample code is as follows.:
        /// <code>
        /// ICompressFilter filterObject = codec.CreateCompressFilter();
        /// 
        /// ...
        /// 
        /// (filterObject as IDisposable)?.Dispose();
        /// </code>
        /// </para>
        /// </remarks>
        public CompressFilter CreateCompressFilter()
        {
            if (!IsSupportedICompressFilter)
                throw new NotSupportedException();

            return
                CompressFilter.Create(
                    CoderType switch
                    {
                        CoderType.Decoder => CreateCompressDecoder(NativeInterfaceObject, Index, ref _compressFilterInterfaceId),
                        CoderType.Encoder => CreateCompressEncoder(NativeInterfaceObject, Index, ref _compressFilterInterfaceId),
                        _ => throw new Exception($"Unknown coder type: {CoderType}"),
                    });
        }

        public static CompressCodecInfo? Create(IntPtr compressCodecsInfo, Int32 index, CoderType coderType)
        {
            if (compressCodecsInfo == IntPtr.Zero)
                throw new ArgumentNullException(nameof(compressCodecsInfo));

            lock (_lockObject)
            {
                var ifp = compressCodecsInfo;
                var codecName = GetCodecName(ifp, index);
                var packStreams = GetPackStreams(ifp, index);
                var isFilter = GetIsFilter(ifp, index);
                switch (coderType)
                {
                    case CoderType.Decoder:
                        if (GetIsDecoderAssigned(ifp, index))
                        {
                            var coderClassId = GetDecoderClassId(ifp, index);
                            var codecId = GetCodecId(ifp, index);
                            IntPtr compressCoder = IntPtr.Zero;
                            IntPtr compressCoder2 = IntPtr.Zero;
                            IntPtr compressFilter = IntPtr.Zero;
                            try
                            {
                                compressCoder = CreateCompressDecoder(ifp, index, ref _compressCoderInterfaceId);
                                compressCoder2 = CreateCompressDecoder(ifp, index, ref _compressCoder2InterfaceId);
                                compressFilter = CreateCompressDecoder(ifp, index, ref _compressFilterInterfaceId);
                                _ = NativeInterOp.IUnknown__AddRef(ifp);
                                return new CompressCodecInfo(
                                    ifp,
                                    index,
                                    codecId,
                                    codecName,
                                    coderClassId,
                                    packStreams,
                                    isFilter,
                                    coderType,
                                    compressCoder != IntPtr.Zero,
                                    compressCoder2 != IntPtr.Zero,
                                    compressFilter != IntPtr.Zero);
                            }
                            finally
                            {
                                if (compressFilter != IntPtr.Zero)
                                    _ = NativeInterOp.IUnknown__Release(compressFilter);
                                if (compressCoder2 != IntPtr.Zero)
                                    _ = NativeInterOp.IUnknown__Release(compressCoder2);
                                if (compressCoder != IntPtr.Zero)
                                    _ = NativeInterOp.IUnknown__Release(compressCoder);
                            }
                        }
                        else
                        {
                            return null;
                        }

                    case CoderType.Encoder:
                        if (GetIsEncoderAssigned(ifp, index))
                        {
                            var coderClassId = GetEncoderClassId(ifp, index);
                            var codecId = GetCodecId(ifp, index);
                            IntPtr compressCoder = IntPtr.Zero;
                            IntPtr compressCoder2 = IntPtr.Zero;
                            IntPtr compressFilter = IntPtr.Zero;
                            try
                            {
                                compressCoder = CreateCompressEncoder(ifp, index, ref _compressCoderInterfaceId);
                                compressCoder2 = CreateCompressEncoder(ifp, index, ref _compressCoder2InterfaceId);
                                compressFilter = CreateCompressEncoder(ifp, index, ref _compressFilterInterfaceId);
                                _ = NativeInterOp.IUnknown__AddRef(ifp);
                                return new CompressCodecInfo(
                                    ifp,
                                    index,
                                    codecId,
                                    codecName,
                                    coderClassId,
                                    packStreams,
                                    isFilter,
                                    coderType,
                                    compressCoder != IntPtr.Zero,
                                    compressCoder2 != IntPtr.Zero,
                                    compressFilter != IntPtr.Zero);
                            }
                            finally
                            {
                                if (compressFilter != IntPtr.Zero)
                                    _ = NativeInterOp.IUnknown__Release(compressFilter);
                                if (compressCoder2 != IntPtr.Zero)
                                    _ = NativeInterOp.IUnknown__Release(compressCoder2);
                                if (compressCoder != IntPtr.Zero)
                                    _ = NativeInterOp.IUnknown__Release(compressCoder);
                            }
                        }
                        else
                        {
                            return null;
                        }

                    default:
                        return null;
                }
            }
        }

        private static UInt64 GetCodecId(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.ID, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                var propValuePtr = (PROPVARIANT*)&propValueBuffer;
                if (propValuePtr->ValueType != PropertyValueType.VT_UI8)
                    throw new Exception("Unexpected value type.");
                return propValuePtr->UInt64Value;
            }
        }

        private static String GetCodecName(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.Name, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return ((PROPVARIANT*)&propValueBuffer)->ManagedStringValue;
            }
        }

        private static Boolean GetIsDecoderAssigned(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.DecoderIsAssigned, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return ((PROPVARIANT*)&propValueBuffer)->ManagedBooleanValue;
            }
        }

        private static Boolean GetIsEncoderAssigned(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.EncoderIsAssigned, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return ((PROPVARIANT*)&propValueBuffer)->ManagedBooleanValue;
            }
        }

        private static Guid GetDecoderClassId(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.Decoder, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return ((PROPVARIANT*)&propValueBuffer)->ManagedGuidValue;
            }
        }

        private static Guid GetEncoderClassId(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.Encoder, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return ((PROPVARIANT*)&propValueBuffer)->ManagedGuidValue;
            }
        }

        private static IntPtr CreateCompressDecoder(IntPtr ifp, Int32 index, ref NativeGUID interfaceId)
        {
            var result = NativeInterOp.ICompressCodecsInfo__CreateDecoder(ifp, (UInt32)index, ref interfaceId, out IntPtr coder);
            if (result == HRESULT.S_OK)
                return coder;
            else if (result == HRESULT.E_NOINTERFACE)
                return IntPtr.Zero;
            else
                throw result.GetExceptionFromHRESULT();
        }

        private static IntPtr CreateCompressEncoder(IntPtr ifp, Int32 index, ref NativeGUID interfaceId)
        {
            var result = NativeInterOp.ICompressCodecsInfo__CreateEncoder(ifp, (UInt32)index, ref interfaceId, out IntPtr coder);
            if (result == HRESULT.S_OK)
                return coder;
            else if (result == HRESULT.E_NOINTERFACE)
                return IntPtr.Zero;
            else
                throw result.GetExceptionFromHRESULT();
        }

        private static UInt32 GetPackStreams(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.PackStreams, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                var propValuePtr = (PROPVARIANT*)&propValueBuffer;
                return
                    propValuePtr->ValueType switch
                    {
                        PropertyValueType.VT_EMPTY => 1,
                        PropertyValueType.VT_UI4 => propValuePtr->UInt32Value,
                        _ => throw new Exception("Unexpected value type."),
                    };
            }
        }

        private static Boolean GetIsFilter(IntPtr ifp, Int32 index)
        {
            unsafe
            {
                PROPVARIANT_BUFFER propValueBuffer;
                var result = GetPropertyValue(ifp, index, MethodPropID.IsFilter, &propValueBuffer);
                if (result != HRESULT.S_OK)
                    throw result.GetExceptionFromHRESULT();
                return ((PROPVARIANT*)&propValueBuffer)->ManagedBooleanValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        private static unsafe HRESULT GetPropertyValue(IntPtr ifp, Int32 index, MethodPropID propId, PROPVARIANT_BUFFER* propValueBuffer)
            => NativeInterOp.ICompressCodecsInfo__GetProperty(ifp, checked((UInt32)index), propId, propValueBuffer);
    }
}
