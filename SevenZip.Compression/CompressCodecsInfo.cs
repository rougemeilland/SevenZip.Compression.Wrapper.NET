using SevenZip.NativeInterface;
using SevenZip.NativeInterface.Compression;
using System;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SevenZip.Compression
{
    class CompressCodecsInfo
    {
        private class CodecsKey
            : IEquatable<CodecsKey>
        {
            public CodecsKey(string codecsName, CoderType coderType)
            {
                CodecsName = codecsName;
                CoderType = coderType;
            }

            public string CodecsName { get; }
            public CoderType CoderType { get; }

            public bool Equals(CodecsKey? other) =>
                other is not null
                    && string.Equals(CodecsName, other.CodecsName, StringComparison.OrdinalIgnoreCase)
                    && CoderType.Equals(other.CoderType);

            public override bool Equals(object? obj) =>
                obj is not null
                    && GetType() == obj.GetType()
                    && Equals((CodecsKey)obj);

            public override Int32 GetHashCode() =>
                CodecsName.GetHashCode() ^ CoderType.GetHashCode();
        }

#if true
        private const string _implemetModuleFileName = "SevenZip.NativeWrapper.Managed";
#else
        private const string _implemetModuleFileName = "SevenZip.NativeWrapper";
#endif

        private static Models.SettingsModel _settings;
        private static readonly CompressCodecsInfo _instance;

        private readonly IDictionary<CodecsKey, ICompressCodecInfo> _codecs;

        static CompressCodecsInfo()
        {
            var baseDirectoryPath = Path.GetDirectoryName(typeof(CompressCodecsInfo).Assembly.Location) ?? ".";
            _settings = LoadSettingsFile(baseDirectoryPath);
            var pluginLocations = EnumerateAvailablePluginSettings(_settings).ToList();
            if (!pluginLocations.Any())
                throw new Exception($"The plugins available in the current execution environment are not registered in the configuration file.: os=\"{GetCurrentOs()} ({GetCurrentArchitectureBitsCount()}bits)\"");
            CompressCodecsInfo compressCodecsInfo = CreateInstance(baseDirectoryPath, pluginLocations);
            _instance = compressCodecsInfo;
        }

        private CompressCodecsInfo(IEnumerable<ICompressCodecInfo> codecs)
        {
            _codecs = codecs.ToDictionary(codec => new CodecsKey(codec.CodecName, codec.CoderType), codec => codec);
            var x = _codecs.Keys.Where(key => key.CodecsName.Contains("ppmd", StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public static ICompressCoder CreateCompressCoder(string coderName, CoderType coderType)
        {
            if (string.IsNullOrEmpty(coderName))
                throw new ArgumentException($"'{nameof(coderName)}' cannot be null or an empty string.", nameof(coderName));

            var codecKey = new CodecsKey(coderName, coderType);
            if (!_instance._codecs.TryGetValue(codecKey, out ICompressCodecInfo? codec))
                throw new NotSupportedException();
            if (!codec.IsSupportedICompressCoder)
                throw new NotSupportedException();
            return codec.CreateCompressCoder();
        }

        private static Models.SettingsModel LoadSettingsFile(string baseDirectoryPath)
        {
            var settingsFilePath =
                Path.Combine(baseDirectoryPath, "SevenZip.Compression.settings.json");
            if (!File.Exists(settingsFilePath))
                throw new FileNotFoundException("The configuration file cannot be found.", settingsFilePath);
            var settings =
                JsonSerializer.Deserialize<Models.SettingsModel>(
                    File.ReadAllText(settingsFilePath),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (settings is null)
                throw new Exception("Failed to deserialize the configuration file.");
            return settings;
        }

        private static IEnumerable<Models.PluginModel> EnumerateAvailablePluginSettings(Models.SettingsModel settings)
        {
            var currentOs = GetCurrentOs();
            var currentArchitectureBitCount = GetCurrentArchitectureBitsCount();
            foreach (var pluginLocation in settings.Plugins)
            {
                if (string.Equals(pluginLocation.Os, currentOs, StringComparison.OrdinalIgnoreCase)
                    && pluginLocation.Bits == currentArchitectureBitCount)
                {
                    yield return pluginLocation;
                }
            }
        }

        private static CompressCodecsInfo CreateInstance(string baseDirectoryPath, IEnumerable<Models.PluginModel> pluginLocations)
        {
            CompressCodecsInfo compressCodecsInfo;
            try
            {
                var pluginObject = EnumeratePluginEntryPoints(baseDirectoryPath, pluginLocations).FirstOrDefault();
                if (pluginObject is null)
                    throw new Exception($"There is no plugin file available, or there is no plugin that can be loaded in the current runtime environment.: os={GetCurrentOs()} ({GetCurrentArchitectureBitsCount()}bit)");
                try
                {
                    pluginObject.Initialize();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to initialize the plugin.: {pluginObject.GetType().FullName}", ex);
                }

                compressCodecsInfo = new CompressCodecsInfo(pluginObject.EnumerateCodecs());
            }
            catch (Exception ex)
            {
                throw new Exception("The plug-in corresponding to the current operating environment is not installed or registered in the configuration file.", ex);
            }

            return compressCodecsInfo;
        }

        private static IEnumerable<ICompressCodecsInfo> EnumeratePluginEntryPoints(string baseDirectoryPath, IEnumerable<Models.PluginModel> pluginLocations)
        {
            foreach (var pluginFilePath in EnumeratePluginFilePath(baseDirectoryPath, pluginLocations))
            {
                var pluginAssembly = LoadAssembly(pluginFilePath);
                if (pluginAssembly is not null)
                {
                    foreach (var entrypoint in FindPluginEntryPoint(pluginAssembly))
                        yield return entrypoint;
                }

            }
        }

        private static IEnumerable<string> EnumeratePluginFilePath(string baseDirectoryPath, IEnumerable<Models.PluginModel> pluginLocations)
        {
            foreach (var pluginLocation in pluginLocations)
            {
                var pluginFileNamePattern = new Regex(string.Format(pluginLocation.FileNamePattern, "SevenZip.NativeWrapper.Managed"), RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                var pluginDirectory =
                    pluginLocation.SubDir is null
                    ? baseDirectoryPath
                    : Path.Combine(baseDirectoryPath, pluginLocation.SubDir);
                foreach (var filePath in Directory.EnumerateFiles(pluginDirectory))
                {
                    if (filePath is not null)
                    {
                        var fileName = Path.GetFileName(filePath);
                        if (fileName is not null)
                        {
                            if (pluginFileNamePattern.IsMatch(fileName))
                                yield return filePath;
                        }
                    }
                }
            }
        }

        private static Assembly? LoadAssembly(string pluginFilePath)
        {
            try
            {
                // Check if the file is correct as an assembly
                AssemblyName.GetAssemblyName(pluginFilePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
            try
            {
                return Assembly.LoadFile(pluginFilePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private static IEnumerable<ICompressCodecsInfo> FindPluginEntryPoint(Assembly pluginAssembly)
        {
            var interfaceName = typeof(ICompressCodecsInfo).FullName;
            if (interfaceName is not null)
            {
                foreach (var type in pluginAssembly.GetTypes())
                {
                    if (type.FullName is not null && type.IsClass && !type.IsAbstract && type.GetInterface(interfaceName) is not null)
                    {
                        var interfaceObject = TryCreatePluginEntryPoint(pluginAssembly, type.FullName);
                        if (interfaceObject is not null)
                            yield return interfaceObject;
                    }
                }
            }
        }

        private static ICompressCodecsInfo? TryCreatePluginEntryPoint(Assembly pluginAssembly, string pluginClassTypeFull)
        {
            try
            {
                var interfaceObjectThatMayBeNull = pluginAssembly.CreateInstance(pluginClassTypeFull);
                if (interfaceObjectThatMayBeNull is null)
                    throw new Exception("interfaceObjectThatMayBeNull is null");
                if (interfaceObjectThatMayBeNull is not ICompressCodecsInfo)
                    throw new Exception("interfaceObjectThatMayBeNull is not ICompressCodecsInfo");
                return (ICompressCodecsInfo)interfaceObjectThatMayBeNull;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        private static string GetCurrentOs()
        {
            if (OperatingSystem.IsWindows())
                return "win";
            else if (OperatingSystem.IsLinux())
                return "linux";
            else if (OperatingSystem.IsMacOS())
                return "macos";
            else
                throw new NotSupportedException("Running on an unexpected operating system.");
        }

        private static Int32 GetCurrentArchitectureBitsCount()
        {
            if (Environment.Is64BitProcess)
                return 64;
            else
                return 32;
        }
    }
}
