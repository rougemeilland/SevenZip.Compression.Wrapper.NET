using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using SevenZip.NativeWrapper.Managed.Platform;
using System;
using System.Collections.Generic;
using System.IO;

namespace SevenZip.NativeWrapper.Managed.Compression
{
    /// <summary>
    /// A class of set of codecs.
    /// </summary>
    public class CompressCodecsInfo
        : Unknown, ICompressCodecsInfo
    {
        private static string _locationPath;

        static CompressCodecsInfo()
        {
            _locationPath = Path.GetDirectoryName(typeof(CompressCodecsInfo).Assembly.Location) ?? ".";
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        public CompressCodecsInfo()
            : base(IntPtr.Zero)
        {
        }

        void ICompressCodecsInfo.Initialize()
        {
            IntPtr nativeResource;
            var result = UnmanagedEntryPoint.ICompressCodecsInfo_Create(_locationPath, out nativeResource);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            AttatchNativeInterfaceObject(nativeResource);
        }

        IEnumerable<ICompressCodecInfo> ICompressCodecsInfo.EnumerateCodecs()
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

        IUnknown IUnknown.QueryInterface(Type interfaceType)
        {
            if (interfaceType.GUID == typeof(IUnknown).GUID)
                return this;
            else if (interfaceType.GUID == typeof(ICompressCodecsInfo).GUID)
                return this;
            else
                throw new NotSupportedException();
        }

        private UInt32 GetNumMethods()
        {
            UInt32 count;
            var result = UnmanagedEntryPoint.ICompressCodecsInfo__GetNumMethods(NativeInterfaceObject, out count);
            if (result != HRESULT.S_OK)
                throw result.GetExceptionFromHRESULT();
            return count;
        }
    }
}
