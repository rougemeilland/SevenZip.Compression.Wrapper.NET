using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palmtree;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal class CompressCodecsCollection
    {
        private class CodecsKey
            : IEquatable<CodecsKey>
        {
            public CodecsKey(String codecsName, CoderType coderType)
            {
                CodecsName = codecsName;
                CoderType = coderType;
            }

            public String CodecsName { get; }
            public CoderType CoderType { get; }

            public Boolean Equals(CodecsKey? other)
                => other is not null
                    && String.Equals(CodecsName, other.CodecsName, StringComparison.OrdinalIgnoreCase)
                    && CoderType.Equals(other.CoderType);

            public override Boolean Equals(Object? other)
                => other is not null
                    && GetType() == other.GetType()
                    && Equals((CodecsKey)other);

            public override Int32 GetHashCode()
                => HashCode.Combine(CodecsName, CoderType);

            public override String ToString() => $"{CodecsName}:{CoderType}";
        }

        private static readonly Object _lockObject;

        private static CompressCodecsCollection? _instance;

        private readonly CompressCodecsInfo _codecsInfo;
        private readonly IDictionary<CodecsKey, CompressCodecInfo> _codecs;

        static CompressCodecsCollection()
        {
            _lockObject = new Object();
        }

        private CompressCodecsCollection(CompressCodecsInfo codecsInfo, IDictionary<CodecsKey, CompressCodecInfo> codecs)
        {
            _codecsInfo = codecsInfo;
            _codecs = codecs;
        }

        public static CompressCodecsCollection Instance
        {
            get
            {
                lock (_lockObject)
                {
                    _instance ??= CreateInstance();
                    Validation.Assert(_instance is not null, "_instance is not null");
                    return _instance;
                }
            }
        }

        public UInt32 Version => _codecsInfo.Version;
        public UInt32 InterfaceType => _codecsInfo.InterfaceType;

        public CompressCoder CreateCompressCoder(String coderName, CoderType coderType)
        {
            if (String.IsNullOrEmpty(coderName))
                throw new ArgumentException($"'{nameof(coderName)}' cannot be null or an empty string.", nameof(coderName));

            var codecKey = new CodecsKey(coderName, coderType);
            if (!_codecs.TryGetValue(codecKey, out var codec))
                throw new NotSupportedException();
            if (!codec.IsSupportedICompressCoder)
                throw new NotSupportedException();
            return codec.CreateCompressCoder();
        }

        private static CompressCodecsCollection CreateInstance()
        {
            var baseDirectory = typeof(CompressCodecsCollection).Assembly.GetBaseDirectory();
            var codecsInfo =
                CompressCodecsInfo.Create()
                ?? throw new FileNotFoundException($"The native library package (Palmtree.SevenZip.Compression.Wrapper.NET.Native) of this library (Palmtree.SevenZip.Compression.Wrapper.NET) is not installed.");
            return
                new CompressCodecsCollection(
                    codecsInfo,
                    codecsInfo.EnumerateCodecs()
                    .ToDictionary(
                        codec => new CodecsKey(codec.CodecName, codec.CoderType),
                        codec => codec));
        }
    }
}
