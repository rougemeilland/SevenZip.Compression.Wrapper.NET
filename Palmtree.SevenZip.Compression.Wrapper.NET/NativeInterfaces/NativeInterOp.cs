using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Palmtree;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class NativeInterOp
    {
        private const String _SEVEN_ZIP_DLL_NAME = "7z";
        private const String _NATIVE_METHOD_DLL_NAME = "Palmtree.SevenZip.Compression.Wrapper.NET.Native";

        private static readonly DllNameResolver _dllNameResolver;

        static NativeInterOp()
        {
            _dllNameResolver = new DllNameResolver();

            // ネイティブコード DLL のパス名のリゾルバを登録します。
            NativeLibrary.SetDllImportResolver(
                typeof(NativeInterOp).Assembly,
                (libraryName, assembly, searchPath) => _dllNameResolver.ResolveDllName(libraryName, assembly, searchPath));
        }

        #region Global__GetSizeOfPROPVARIANT

        /// <summary>
        /// ネイティブコードにおける <see cref="PROPVARIANT"/> 構造体のサイズを取得します。
        /// </summary>
        /// <returns>
        /// ネイティブコードにおける <see cref="PROPVARIANT"/> 構造体のサイズ (バイト数) を示す <see cref="Int32"/> 値です。
        /// </returns>
        /// <remarks>
        /// <para>
        /// ネイティブコードにおける <see cref="PROPVARIANT"/> 構造体のサイズは処理系依存であるため、マルチプラットフォーム環境においては一定のサイズであると仮定してはなりません。
        /// </para>
        /// <para>
        /// このメソッドの復帰値を取得することにより、現在実行中の処理系での ネイティブコードにおける <see cref="PROPVARIANT"/> 構造体のサイズを知ることが出来ます。
        /// </para>
        /// <para>
        /// 既知の処理系での <see cref="PROPVARIANT"/> の構造体のサイズの例を以下に示します。
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <term>Windows (x86) の場合</term><description>16 バイト (4バイトバウンダリ)</description>
        /// </item>
        /// <item>
        /// <term>Windows (x64) の場合</term><description>24 バイト (8バイトバウンダリ)</description>
        /// </item>
        /// <item>
        /// <term>Linux (x86) の場合</term><description>16 バイト (4 バイトバウンダリ) (*1)</description>
        /// </item>
        /// <item>
        /// <term>Linux (x64) の場合</term><description>16 バイト (8 バイトバウンダリ) (*1)</description>
        /// </item>
        /// </list>
        /// <para>
        /// (*1): Linux においては <see cref="PROPVARIANT"/> 構造体は定義されておらず、7-zip が独自に定義しています。
        /// </para>
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// サポートされていないオペレーティングシステムまたは CPU アーキテクチャです。
        /// </exception>

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static Int32 Global__GetSizeOfPROPVARIANT()
        {
            if (OperatingSystem.IsWindows())
                return Global__GetSizeOfPROPVARIANT_win();
            else if (OperatingSystem.IsLinux())
                return Global__GetSizeOfPROPVARIANT_linux();
            else if (OperatingSystem.IsMacOS())
                return Global__GetSizeOfPROPVARIANT_osx();
            else
                throw new NotSupportedException("Running on this operating system is not supported.");
        }

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_Global__GetSizeOfPROPVARIANT")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static partial Int32 Global__GetSizeOfPROPVARIANT_win();

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_Global__GetSizeOfPROPVARIANT")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]

        private static partial Int32 Global__GetSizeOfPROPVARIANT_linux();
        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_Global__GetSizeOfPROPVARIANT")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static partial Int32 Global__GetSizeOfPROPVARIANT_osx();

        #endregion

        #region Global__GetSizeOfOleChar

        /// <summary>
        /// COM で使用される文字型の長さを取得します。
        /// </summary>
        /// <returns>
        /// COM で使用される文字型の長さを示す <see cref="Int32"/> 値です。
        /// </returns>
        /// <remarks>
        /// <para>
        /// COM で使用される文字型は <c>wchar_t</c> 型ですが、<c>wchar_t</c> 型のビット長は処理系依存であるため、マルチプラットフォーム環境においては一定のビット長であると仮定してはなりません。
        /// </para>
        /// <para>
        /// このメソッドの復帰値を取得することにより、現在実行中の処理系での COM で使用される文字型の長さ (バイト数) を知ることが出来ます。
        /// </para>
        /// <para>
        /// 既知の処理系での COM で使用される文字型の長さの例を以下に示します。
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <term>Windows (VC++) の場合</term><description>2 バイト</description>
        /// </item>
        /// <item>
        /// <term>Linux (GCC) の場合</term><description>4 バイト (*1)</description>
        /// </item>
        /// </list>
        /// <para>
        /// (*1): Linux においてはよく知られている COM の実装は存在しません。
        /// しかし、7-zip の外部インターフェースは COM に似た独自仕様になっており、文字型を <c>wchar_t</c> 型としています。
        /// </para>
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// サポートされていないオペレーティングシステムまたは CPU アーキテクチャです。
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public static Int32 Global__GetSizeOfOleChar()
        {
            if (OperatingSystem.IsWindows())
                return Global__GetSizeOfOleChar_win();
            else if (OperatingSystem.IsLinux())
                return Global__GetSizeOfOleChar_linux();
            else if (OperatingSystem.IsMacOS())
                return Global__GetSizeOfOleChar_osx();
            else
                throw new NotSupportedException("Running on this operating system is not supported.");
        }

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_Global__GetSizeOfOleChar")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private unsafe static partial Int32 Global__GetSizeOfOleChar_win();

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_Global__GetSizeOfOleChar")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        private unsafe static partial Int32 Global__GetSizeOfOleChar_linux();

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_Global__GetSizeOfOleChar")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        private unsafe static partial Int32 Global__GetSizeOfOleChar_osx();

        #endregion

        #region Global__GetSevenZipEntryPointsTable

        public static unsafe HRESULT Global__GetSevenZipEntryPointsTable(SevenZipEngineEntryPoints* entryPointsTable)
        {
            try
            {
                // 7-zip のライブラリのロードを引き起こすためのダミーの呼び出し
                _ = SevenZip__GetNumberOfMethods(out _);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException($"7-zip library DLL not found.", ex);
            }

            // この時点で 7-zip のロードが完了しているはず。

            // 7-zip ライブラリ DLL のハンドルを取得する
            var handle = _dllNameResolver.GetDllHandle(_SEVEN_ZIP_DLL_NAME);
            Validation.Assert(handle != IntPtr.Zero, "handle != IntPtr.Zero");

            // 7-zip ライブラリ DLL のエントリポイントを取得する
            if ((entryPointsTable->FpCreateDecoder = GetExport(handle, "CreateDecoder")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpCreateEncoder = GetExport(handle, "CreateEncoder")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpCreateObject = GetExport(handle, "CreateObject")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpGetHandlerProperty = GetExport(handle, "GetHandlerProperty")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpGetHandlerProperty2 = GetExport(handle, "GetHandlerProperty2")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpGetHashers = GetExport(handle, "GetHashers")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpGetMethodProperty = GetExport(handle, "GetMethodProperty")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpGetNumberOfFormats = GetExport(handle, "GetNumberOfFormats")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpGetNumberOfMethods = GetExport(handle, "GetNumberOfMethods")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            if ((entryPointsTable->FpGetModuleProp = GetExport(handle, "GetModuleProp")) == null)
                return HRESULT.E_NOT_SUPPORTED;
            return HRESULT.S_OK;

            static void* GetExport(IntPtr handle, String name)
                => NativeLibrary.TryGetExport(handle, name, out var address)
                    ? address.ToPointer()
                    : null;
        }

        #endregion

        #region SevenZip__GetNumberOfMethods

        private static HRESULT SevenZip__GetNumberOfMethods(out UInt32 numCodecs)
        {
            if (OperatingSystem.IsWindows())
                return SevenZip__GetNumberOfMethods_win(out numCodecs);
            else if (OperatingSystem.IsLinux())
                return SevenZip__GetNumberOfMethods_linux(out numCodecs);
            else if (OperatingSystem.IsMacOS())
                return SevenZip__GetNumberOfMethods_osx(out numCodecs);
            else
                throw new NotSupportedException("Running on this operating system is not supported.");
        }

        [LibraryImport(_SEVEN_ZIP_DLL_NAME, EntryPoint = "GetNumberOfMethods")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static partial HRESULT SevenZip__GetNumberOfMethods_win(out UInt32 numCodecs);

        [LibraryImport(_SEVEN_ZIP_DLL_NAME, EntryPoint = "GetNumberOfMethods")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static partial HRESULT SevenZip__GetNumberOfMethods_linux(out UInt32 numCodecs);

        [LibraryImport(_SEVEN_ZIP_DLL_NAME, EntryPoint = "GetNumberOfMethods")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        private static partial HRESULT SevenZip__GetNumberOfMethods_osx(out UInt32 numCodecs);

        #endregion

        #region ICompressCodecsInfo__Create

        /// <summary>
        /// Create an <c>ICompressCodecsInfo</c> interface object.
        /// </summary>
        /// <param name="entrypointsTable">
        /// A pointer to a table of entry points for the 7-zip library.
        /// </param>
        /// <param name="sizeOfEntryPontsTable">
        /// The size of <see cref="SevenZipEngineEntryPoints"/> structure.
        /// </param>
        /// <param name="obj">
        /// If the call to this function is successful, the ICompressCodecsInfo interface object will be output.
        /// </param>
        /// <returns>
        /// <para>
        /// If the return value is <see cref="HRESULT.S_OK"/>, it means that the call to this function was successful.
        /// </para>
        /// <para>
        /// If the return value is not <see cref="HRESULT.S_OK"/>, it means that the call to this function failed.
        /// At this time, the return value means the reason for the failure.
        /// </para>
        /// </returns>

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public unsafe static HRESULT ICompressCodecsInfo__Create(SevenZipEngineEntryPoints* entrypointsTable, UInt32 sizeOfEntryPontsTable, out IntPtr obj)
        {
            if (OperatingSystem.IsWindows())
                return ICompressCodecsInfo__Create_win(entrypointsTable, sizeOfEntryPontsTable, out obj);
            else if (OperatingSystem.IsLinux())
                return ICompressCodecsInfo__Create_linux(entrypointsTable, sizeOfEntryPontsTable, out obj);
            else if (OperatingSystem.IsMacOS())
                return ICompressCodecsInfo__Create_osx(entrypointsTable, sizeOfEntryPontsTable, out obj);
            else
                throw new NotSupportedException("Running on this operating system is not supported.");
        }

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_ICompressCodecsInfo__Create")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvStdcall) })]
        private unsafe static partial HRESULT ICompressCodecsInfo__Create_win(SevenZipEngineEntryPoints* entryPontsTable, UInt32 sizeOfEntryPontsTable, out IntPtr obj);

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_ICompressCodecsInfo__Create")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        private unsafe static partial HRESULT ICompressCodecsInfo__Create_linux(SevenZipEngineEntryPoints* entryPontsTable, UInt32 sizeOfEntryPontsTable, out IntPtr obj);

        [LibraryImport(_NATIVE_METHOD_DLL_NAME, EntryPoint = "EXPORTED_ICompressCodecsInfo__Create")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        private unsafe static partial HRESULT ICompressCodecsInfo__Create_osx(SevenZipEngineEntryPoints* entryPontsTable, UInt32 sizeOfEntryPontsTable, out IntPtr obj);

        #endregion

        #region ICompressCodecsInfo__GetProperty

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe HRESULT ICompressCodecsInfo__GetProperty(IntPtr ifp, UInt32 index, MethodPropID propID, PROPVARIANT_BUFFER* value)
            => ICompressCodecsInfo__GetProperty(ifp, index, propID, (PROPVARIANT*)value);

        #endregion

        #region ICompressCodecsInfo__GetModuleProp

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe HRESULT ICompressCodecsInfo__GetModuleProp(IntPtr ifp, ModulePropID propID, PROPVARIANT_BUFFER* value)
            => ICompressCodecsInfo__GetModuleProp(ifp, propID, (PROPVARIANT*)value);

        #endregion

        #region ICompressCoder__Code

        /// <summary>
        /// Reads data from the input stream, codes it, and writes it to the output stream.
        /// </summary>
        /// <param name="ifp">
        /// Set the coder's <c>ICompressCoder</c> interface object.
        /// </param>
        /// <param name="inStreamReader">
        /// Set the delegate for the function that reads the data from the input stream.
        /// </param>
        /// <param name="outStreamWriter">
        /// Set the delegate for the function that writes the data to the output stream.
        /// </param>
        /// <param name="inSize">
        /// If you specify the total size of the data to read from the input stream, set the number of bytes.
        /// If not, set null.
        /// </param>
        /// <param name="outSize">
        /// If you specify the total size of the data to write to the output stream, set the number of bytes.
        /// If not, set null.
        /// </param>
        /// <param name="progress">
        /// If you want to be notified of the progress of your coding, set the delegate of the function to be notified.
        /// If not, set null.
        /// </param>
        /// <returns>
        /// <para>
        /// If the return value is <see cref="HRESULT.S_OK"/>, it means that the call to this function was successful.
        /// </para>
        /// <para>
        /// If the return value is not <see cref="HRESULT.S_OK"/>, it means that the call to this function failed.
        /// At this time, the return value means the reason for the failure.
        /// </para>
        /// </returns>
        public static HRESULT ICompressCoder__Code(IntPtr ifp, NativeInStreamReader inStreamReader, NativeOutStreamWriter outStreamWriter, UInt64? inSize, UInt64? outSize, NativeProgressReporter? progress)
        {
            unsafe
            {
                UInt64 inSizeBuffer;
                UInt64 outSizeBuffer;
                return
                    ICompressCoder__Code(
                        ifp,
                        inStreamReader,
                        outStreamWriter,
                        inSize.ToPointer(&inSizeBuffer),
                        outSize.ToPointer(&outSizeBuffer),
                        progress);
            }
        }

        #endregion

        #region ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt

        /// <summary>
        /// Set the properties on the coder.
        /// </summary>
        /// <param name="ifp">
        /// Set the coder's <c>ICompressSetCoderPropertiesOpt</c> interface object.
        /// </param>
        /// <param name="properties">
        /// Set an enumerator of property ID / value pairs.
        /// </param>
        /// <returns>
        /// <para>
        /// If the return value is <see cref="HRESULT.S_OK"/>, it means that the call to this function was successful.
        /// </para>
        /// <para>
        /// If the return value is not <see cref="HRESULT.S_OK"/>, it means that the call to this function failed.
        /// At this time, the return value means the reason for the failure.
        /// </para>
        /// </returns>
        public static HRESULT ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt(IntPtr ifp, IEnumerable<(CoderPropertyId propertyId, Object propertyValue)> properties)
            => SetNativeCoderProperties(ifp, true, properties.Where(property => property.propertyId == CoderPropertyId.ExpectedDataSize).ToArray());

        #endregion

        #region ICompressSetCoderProperties__SetCoderProperties

        /// <summary>
        /// Set the properties on the coder.
        /// </summary>
        /// <param name="ifp">
        /// Set the coder's <c>ICompressSetCoderProperties</c> interface object.
        /// </param>
        /// <param name="properties">
        /// Set an enumerator of property ID / value pairs.
        /// </param>
        /// <returns>
        /// <para>
        /// If the return value is <see cref="HRESULT.S_OK"/>, it means that the call to this function was successful.
        /// </para>
        /// <para>
        /// If the return value is not <see cref="HRESULT.S_OK"/>, it means that the call to this function failed.
        /// At this time, the return value means the reason for the failure.
        /// </para>
        /// </returns>
        public static HRESULT ICompressSetCoderProperties__SetCoderProperties(IntPtr ifp, IEnumerable<(CoderPropertyId propertyId, Object propertyValue)> properties)
            => SetNativeCoderProperties(ifp, false, properties.Where(property => property.propertyId != CoderPropertyId.ExpectedDataSize).ToArray());

        #endregion

        #region ICompressSetDecoderProperties2__SetDecoderProperties2

        /// <summary>
        /// Set the content property in the decoder.
        /// </summary>
        /// <param name="ifp">
        /// Set the coder's <c>ICompressSetDecoderProperties2</c> interface object.
        /// </param>
        /// <param name="contentProperty">
        /// Set the byte array of content property.
        /// </param>
        /// <returns>
        /// <para>
        /// If the return value is <see cref="HRESULT.S_OK"/>, it means that the call to this function was successful.
        /// </para>
        /// <para>
        /// If the return value is not <see cref="HRESULT.S_OK"/>, it means that the call to this function failed.
        /// At this time, the return value means the reason for the failure.
        /// </para>
        /// </returns>
        public static HRESULT ICompressSetDecoderProperties2__SetDecoderProperties2(IntPtr ifp, ReadOnlySpan<Byte> contentProperty)
        {
            unsafe
            {
                fixed (Byte* contentPropertyPtr = contentProperty)
                {
                    return ICompressSetDecoderProperties2__SetDecoderProperties2(ifp, contentPropertyPtr, (UInt32)contentProperty.Length);
                }
            }
        }

        #endregion

        #region ICompressReadUnusedFromInBuf__ReadUnusedFromInBuf

        /// <summary>
        /// Reads uncoded data from the input stream.
        /// </summary>
        /// <param name="ifp">
        /// Set the coder's <c>ICompressReadUnusedFromInBuf</c> interface object.
        /// </param>
        /// <param name="buffer">
        /// Set a buffer to store the read data.
        /// </param>
        /// <param name="processedSize">
        /// If the call to this function is successful, the length in bytes of the data that could actually be read is output.
        /// </param>
        /// <returns>
        /// <para>
        /// If the return value is <see cref="HRESULT.S_OK"/>, it means that the call to this function was successful.
        /// </para>
        /// <para>
        /// If the return value is not <see cref="HRESULT.S_OK"/>, it means that the call to this function failed.
        /// At this time, the return value means the reason for the failure.
        /// </para>
        /// </returns>
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

        #endregion

        #region ICompressSetOutStreamSize__SetOutStreamSize

        /// <summary>
        /// Sets the length of the data to read from the output stream.
        /// </summary>
        /// <param name="ifp">
        /// Set the coder's <c>ICompressSetOutStreamSize</c> interface object.
        /// </param>
        /// <param name="outSize">
        /// Set the length of the data to read from the output stream. If you don't want to specify the length of the data, set null.
        /// </param>
        /// <returns>
        /// <para>
        /// If the return value is <see cref="HRESULT.S_OK"/>, it means that the call to this function was successful.
        /// </para>
        /// <para>
        /// If the return value is not <see cref="HRESULT.S_OK"/>, it means that the call to this function failed.
        /// At this time, the return value means the reason for the failure.
        /// </para>
        /// </returns>
        public static HRESULT ICompressSetOutStreamSize__SetOutStreamSize(IntPtr ifp, UInt64? outSize)
        {
            unsafe
            {
                UInt64 outSizeBuffer;
                return ICompressSetOutStreamSize__SetOutStreamSize(ifp, outSize.ToPointer(&outSizeBuffer));
            }
        }

        #endregion

        #region ISequentialInStream__Read

        public static HRESULT ISequentialInStream__Read(IntPtr ifp, Span<Byte> bytes, out UInt32 processedSize)
        {
            unsafe
            {
                fixed (Byte* p = bytes)
                {
                    return ISequentialInStream__Read(ifp, p, checked((UInt32)bytes.Length), out processedSize);
                }
            }
        }

        #endregion

        #region private methods

        private static HRESULT SetNativeCoderProperties(IntPtr ifp, Boolean isOpt, ReadOnlyMemory<(CoderPropertyId propertyId, Object propertyValue)> propertiesList)
        {
            unsafe
            {
                CoderPropertyId* propertyIds = stackalloc CoderPropertyId[propertiesList.Length];
                UInt64* propertyValues = stackalloc UInt64[(PROPVARIANT.SizeOfPropVariant * propertiesList.Length + (sizeof(UInt64) - 1)) >> 3];
                return SetNativeCoderProperties(ifp, isOpt, propertiesList, propertyIds, (PROPVARIANT*)propertyValues, 0);
            }
        }

        private static unsafe HRESULT SetNativeCoderProperties(IntPtr ifp, Boolean isOpt, ReadOnlyMemory<(CoderPropertyId propertyId, Object propertyValue)> properties, CoderPropertyId* nativeProvertyIds, PROPVARIANT* nativePropertyValues, Int32 currentIndex)
        {
            for (var index = currentIndex; index < properties.Length; ++index)
            {
                var (propertyId, propertyValue) = properties.Span[index];
                nativeProvertyIds[index] = propertyId;
                var nativePropertyValue = PROPVARIANT.GetElementOfArray(nativePropertyValues, index);

                nativePropertyValue->Clear();
                if (propertyValue is Boolean booleanPropertyValue)
                {
                    // If the property value is of type Boolean
                    nativePropertyValue->ValueType = PropertyValueType.VT_BOOL;
                    nativePropertyValue->ManagedBooleanValue = booleanPropertyValue;
                }
                else if (propertyValue is UInt32 uint32PropertyValue)
                {
                    // If the property value is of type UInt32
                    nativePropertyValue->ValueType = PropertyValueType.VT_UI4;
                    nativePropertyValue->UInt32Value = uint32PropertyValue;
                }
                else if (propertyValue is UInt64 uint64PropertyValue)
                {
                    // If the property value is of type UInt64
                    nativePropertyValue->ValueType = PropertyValueType.VT_UI8;
                    nativePropertyValue->UInt64Value = uint64PropertyValue;
                }
                else if (propertyValue is DateTime dateTimePropertyValue)
                {
                    // If the property value is of type DateTime
                    if (dateTimePropertyValue.Kind == DateTimeKind.Unspecified)
                        throw new NotSupportedException("DateTime objects whose Kind property value is 'DateTimeKind.Unspecified' cannot be used as property values.");
                    nativePropertyValue->ValueType = PropertyValueType.VT_UI8;
                    nativePropertyValue->FileTimeValue.DateTime = (UInt64)dateTimePropertyValue.ToFileTimeUtc();
                }
                else if (propertyValue is DateTimeOffset dateTimeOffsetPropertyValue)
                {
                    // If the property value is of type DateTimeOffset
                    nativePropertyValue->ValueType = PropertyValueType.VT_UI8;
                    nativePropertyValue->FileTimeValue.DateTime = (UInt64)dateTimeOffsetPropertyValue.ToFileTime();
                }
                else if (propertyValue is String stringPropertyValue)
                {
                    // If the property value is of type String

                    return
                        SetNativeCoderProperties(
                            ifp,
                            isOpt,
                            properties,
                            nativeProvertyIds,
                            nativePropertyValues,
                            index,
                            nativePropertyValue,
                            $"{stringPropertyValue}\x00");
                }
                else if (propertyValue is MatchFinderType matchFinderPropertyValue)
                {
                    // If the property value is of type MatchFinderType

                    return
                        SetNativeCoderProperties(
                            ifp,
                            isOpt,
                            properties,
                            nativeProvertyIds,
                            nativePropertyValues,
                            index,
                            nativePropertyValue,
                            $"{matchFinderPropertyValue}\x00");
                }
                else
                {
                    // If the property value is of an unsupported type
                    throw new ArgumentException($"{nameof(properties)} contains a property value of an unsupported type.:, propertyId={propertyId}, typeOfPropertyValue={propertyValue.GetType().FullName}", nameof(properties));
                }
            }

            // If the loop reaches the end
            if (isOpt)
                return ICompressSetCoderPropertiesOpt__SetCoderPropertiesOpt(ifp, nativeProvertyIds, nativePropertyValues, checked((UInt32)properties.Length));
            else
                return ICompressSetCoderProperties__SetCoderProperties(ifp, nativeProvertyIds, nativePropertyValues, checked((UInt32)properties.Length));
        }

        private static unsafe HRESULT SetNativeCoderProperties(
            IntPtr ifp,
            Boolean isOpt,
            ReadOnlyMemory<(CoderPropertyId propertyId, Object propertyValue)> properties,
            CoderPropertyId* nativeProvertyIds,
            PROPVARIANT* nativePropertyValues,
            Int32 index,
            PROPVARIANT* nativePropertyValue,
            String stringValue)
        {
            // Encode the string into bytes using the appropriate encoding,
            // and then call the SetNativeCoderProperties method recursively within the fixed interval.

            // Select encoding depending on the size of OLE characters (wchar_t) in native code.
            var encoding =
                PROPVARIANT.SizeOfChar switch
                {
                    2 => Encoding.Unicode,
                    4 => Encoding.UTF32,
                    _ => throw Validation.GetFailErrorException($"The value of sizeOfOleChar is unknown.: {nameof(PROPVARIANT)}.{nameof(PROPVARIANT.SizeOfChar)}SizeOfChar={PROPVARIANT.SizeOfChar}"),
                };
            var bufferSize = encoding.GetByteCount(stringValue);

            // Since the size of wchar_t can be up to 4, the buffer used to store the encoded byte sequence should have a 4-byte boundary (i.e., an array of UInt32).
            var stringValueBuffer = new UInt32[((bufferSize + 3) >> 2)];

            // Recursively execute the continuation within the fixed interval.
            fixed (UInt32* p = stringValueBuffer)
            {
                var length = encoding.GetBytes(stringValue, new Span<Byte>((Byte*)p, bufferSize));
                Validation.Assert(length <= bufferSize, "length <= bufferSize");
                nativePropertyValue->ValueType = PropertyValueType.VT_BSTR;
                nativePropertyValue->StringValue = (Char*)p;
                return SetNativeCoderProperties(ifp, isOpt, properties, nativeProvertyIds, nativePropertyValues, index + 1);
            }
        }

        #endregion
    }
}
