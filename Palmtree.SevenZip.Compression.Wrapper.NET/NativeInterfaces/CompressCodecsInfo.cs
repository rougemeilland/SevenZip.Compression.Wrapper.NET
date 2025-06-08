using System;
using System.Collections.Generic;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressCodecsInfo
    {
        /// <summary>
        /// インストールされている 7-zip のバージョン番号を取得します。
        /// </summary>
        /// <value>
        /// インストールされている 7-zip のバージョン番号を示す <see cref="UInt32"/> 値です。
        /// 上位 16 ビットがメジャーバージョン、下位 16 ビットがマイナーバージョンです。
        /// </value>
        public UInt32 Version
        {
            get
            {
                unsafe
                {
                    PROPVARIANT_BUFFER propValueBuffer;
                    var result = NativeInterOp.ICompressCodecsInfo__GetModuleProp(NativeInterfaceObject, ModulePropID.Version, &propValueBuffer);
                    if (result != HRESULT.S_OK)
                        throw result.GetExceptionFromHRESULT();
                    var propValuePtr = (PROPVARIANT*)&propValueBuffer;
                    if (propValuePtr->ValueType != PropertyValueType.VT_UI4)
                        throw new Exception("Unexpected value type.");
                    return propValuePtr->UInt32Value;
                }
            }
        }

        /// <summary>
        /// インストールされている 7-zip のインターフェースタイプを取得します。
        /// </summary>
        /// <value>
        /// 以下の何れかの値が返ります。
        /// <list type="bullet">
        /// <item><term>IUnknown インターフェースで仮想デストラクタがサポートされている場合</term><description>1</description></item>
        /// <item><term>IUnknown インターフェースで仮想デストラクタがサポートされていない場合</term><description>0</description></item>
        /// </list>
        /// </value>
        public UInt32 InterfaceType
        {
            get
            {
                unsafe
                {
                    PROPVARIANT_BUFFER propValueBuffer;
                    var result = NativeInterOp.ICompressCodecsInfo__GetModuleProp(NativeInterfaceObject, ModulePropID.InterfaceType, &propValueBuffer);
                    if (result != HRESULT.S_OK)
                        throw result.GetExceptionFromHRESULT();
                    var propValuePtr = (PROPVARIANT*)&propValueBuffer;
                    if (propValuePtr->ValueType != PropertyValueType.VT_UI4)
                        throw new Exception("Unexpected value type.");
                    return propValuePtr->UInt32Value;
                }
            }
        }

        /// <summary>
        /// Enumerate the supported codec.
        /// </summary>
        /// <returns>
        /// An enumerator of supported codecs.
        /// </returns>
        public IEnumerable<CompressCodecInfo> EnumerateCodecs()
        {
            var codecsCount = GetNumMethods();
            for (var index = 0; index < codecsCount; ++index)
            {
                var decoder = CompressCodecInfo.Create(NativeInterfaceObject, index, CoderType.Decoder);
                if (decoder is not null)
                    yield return decoder;
                var encoder = CompressCodecInfo.Create(NativeInterfaceObject, index, CoderType.Encoder);
                if (encoder is not null)
                    yield return encoder;
            }
        }

        public static CompressCodecsInfo? Create()
        {
            var success = false;
            var instance = (CompressCodecsInfo?)null;
            try
            {
                instance = new CompressCodecsInfo(IntPtr.Zero);
                if (!instance.Initialize())
                    return null;
                success = true;
                return instance;
            }
            finally
            {
                if (!success)
                    instance?.Dispose();
            }
        }

        private Boolean Initialize()
        {
            SevenZipEngineEntryPoints entryPontsTable;
            unsafe
            {
                if (NativeInterOp.Global__GetSevenZipEntryPointsTable(&entryPontsTable) != HRESULT.S_OK)
                    return false;
                if (NativeInterOp.ICompressCodecsInfo__Create(&entryPontsTable, checked((UInt32)sizeof(SevenZipEngineEntryPoints)), out var nativeResource) != HRESULT.S_OK)
                    return false;
                AttatchNativeInterfaceObject(nativeResource);
                return true;
            }
        }

        private UInt32 GetNumMethods()
        {
            var result = NativeInterOp.ICompressCodecsInfo__GetNumMethods(NativeInterfaceObject, out var count);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return count;
        }
    }
}
