using System;
using System.Collections.Generic;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class CompressCodecsInfo
    {
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
                if (NativeInterOp.ICompressCodecsInfo__Create(&entryPontsTable, out IntPtr nativeResource) != HRESULT.S_OK)
                    return false;
                AttatchNativeInterfaceObject(nativeResource);
                return true;
            }
        }

        private UInt32 GetNumMethods()
        {
            var result = NativeInterOp.ICompressCodecsInfo__GetNumMethods(NativeInterfaceObject, out UInt32 count);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return count;
        }
    }
}
