using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using Palmtree;
using Palmtree.IO;
using SevenZip.Compression.Models;
using SevenZip.Compression.NativeInterfaces;

namespace SevenZip.Compression
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

        private const String _settingsFileName = "SevenZip.Compression.settings.json";

        private static readonly Object _lockObject;

        private static CompressCodecsCollection? _instance;

        private readonly IDictionary<CodecsKey, CompressCodecInfo> _codecs;

        static CompressCodecsCollection()
        {
            _lockObject = new Object();
        }

        private CompressCodecsCollection(IDictionary<CodecsKey, CompressCodecInfo> codecs)
        {
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

        private static SettingsModel LoadSettingsFile(DirectoryPath baseDirectory)
        {
            var settingsFile = baseDirectory.GetFile(_settingsFileName);
            if (!settingsFile.Exists)
                throw new FileNotFoundException($"The configuration file cannot be found.: \"{settingsFile.FullName}\"", settingsFile.FullName);

            return
                JsonSerializer.Deserialize(
                    File.ReadAllText(settingsFile.FullName),
                    SettingsModelSourceGenerationContext.Default.SettingsModel)
                ?? throw new Exception($"Failed to deserialize the configuration file.: \"{settingsFile.FullName}\"");
        }

        private static CompressCodecsCollection CreateInstance()
        {
            var baseDirectory = typeof(CompressCodecsCollection).Assembly.GetBaseDirectory();
            var settings = LoadSettingsFile(baseDirectory);
            var platformKey = $"{GetCurrentOs()}-{GetCurrentArchitecture()}";
            var pluginLocations =
                settings.Entries
                .Where(entry => String.Equals(entry.Platform, platformKey, StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (pluginLocations.Count <= 0)
                throw new Exception($"The plugins available in the current execution environment are not registered in the configuration file.: os=\"{GetCurrentOs()} ({GetCurrentArchitecture()})\"");
            var codecsInfo =
                CreateInstances(baseDirectory, pluginLocations).FirstOrDefault()
                ?? throw new Exception($"No valid plugin was found, or no valid 7-zip native library was found. : os=\"{GetCurrentOs()} ({GetCurrentArchitecture()})\"");

            return
                new CompressCodecsCollection(
                    codecsInfo.EnumerateCodecs()
                    .ToDictionary(
                        codec => new CodecsKey(codec.CodecName, codec.CoderType),
                        codec => codec));
        }

        private static IEnumerable<CompressCodecsInfo> CreateInstances(DirectoryPath baseDirectory, IEnumerable<PlatformSettingsModel> pluginLocations)
        {
            foreach (var pluginLocation in pluginLocations)
            {
                foreach (var sevenZipLibraryFilePath in pluginLocation.SevenZipLibraryFilePaths)
                {
                    if (sevenZipLibraryFilePath.StartsWith("./", StringComparison.Ordinal)
                        || sevenZipLibraryFilePath.StartsWith("../", StringComparison.Ordinal))
                    {
                        // パス名が "./" または "../" で始まっている場合、アセンブリがある場所からの相対パスと見做す
                        var sevenZipFile = baseDirectory.GetFile(sevenZipLibraryFilePath);
                        if (sevenZipFile.Exists)
                        {
                            var instance = CompressCodecsInfo.Create(sevenZipFile.FullName);
                            if (instance is not null)
                                yield return instance;
                        }
                    }
                    else
                    {
                        // パス名が "./" または "../" の何れでも始まっていない場合、システムの既定のディレクトリから 7-zip の DLL を検索する。
                        var instance = CompressCodecsInfo.Create(sevenZipLibraryFilePath);
                        if (instance is not null)
                            yield return instance;
                    }
                }
            }
        }

        private static String GetCurrentOs()
        {
            if (OperatingSystem.IsWindows())
                return "win";
            else if (OperatingSystem.IsLinux())
                return "linux";
            else if (OperatingSystem.IsMacOS())
                return "osx";
            else
                throw new NotSupportedException("Running on an unexpected operating system.");
        }

        private static String GetCurrentArchitecture()
        {
            var processArchitecture = RuntimeInformation.ProcessArchitecture;
            return
                processArchitecture switch
                {
                    Architecture.X86 => "x86",
                    Architecture.X64 => "x64",
                    Architecture.Arm => "arm32",
                    Architecture.Arm64 => "arm64",
                    _ => throw new NotSupportedException($"Not supported on this architecture.: {processArchitecture}"),
                };
        }
    }
}
