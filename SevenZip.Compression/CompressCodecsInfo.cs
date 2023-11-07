using SevenZip.NativeInterface;
using System.Runtime.InteropServices;
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

        private static readonly Models.SettingsModel _settings;
        private static readonly CompressCodecsInfo _instance;

        private readonly IDictionary<CodecsKey, ICompressCodecInfo> _codecs;

        static CompressCodecsInfo()
        {
            var baseDirectoryPath = Path.GetDirectoryName(typeof(CompressCodecsInfo).Assembly.Location) ?? ".";
            _settings = LoadSettingsFile(baseDirectoryPath);
            var pluginLocations = EnumerateAvailablePluginSettings(_settings).ToList();
            if (!pluginLocations.Any())
                throw new Exception($"The plugins available in the current execution environment are not registered in the configuration file.: framework={GetCurrentFramework()}, os=\"{GetCurrentOs()} ({GetCurrentArchitecture()})\"");
            var compressCodecsInfo = CreateInstances(baseDirectoryPath, pluginLocations).FirstOrDefault();
            if (compressCodecsInfo is null)
                throw new Exception($"No valid plugin was found, or no valid 7-zip native library was found. : framework={GetCurrentFramework()}, os=\"{GetCurrentOs()} ({GetCurrentArchitecture()})\"");
            _instance = compressCodecsInfo;
        }

        private CompressCodecsInfo(IEnumerable<ICompressCodecInfo> codecs)
        {
#if true
            foreach (var codec in codecs)
            {
                if (codec.IsSupportedICompressCoder || codec.IsSupportedICompressCoder2 || codec.IsSupportedICompressFilter)
                {
                    Console.WriteLine($"{{ \"index\":{codec.Index}, \"id\":{codec.ID}, \"name\":{codec.CodecName}, \"coderClassId\":{codec.CoderClassId}, \"packStreams\": {codec.PackStreams}, \"type\":{codec.CoderType}, \"isSupportedICompressCoder\":{codec.IsSupportedICompressCoder}, \"isSupportedICompressCoder2\":{codec.IsSupportedICompressCoder2}, \"isSupportedICompressFilter\":{codec.IsSupportedICompressFilter} }}");
                }
            }
#endif
            _codecs = codecs.ToDictionary(codec => new CodecsKey(codec.CodecName, codec.CoderType), codec => codec);
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

        private static IEnumerable<Models.PluginSettingModel> EnumerateAvailablePluginSettings(Models.SettingsModel settings)
        {
            var platformKey = $"{GetCurrentOs()}-{GetCurrentArchitecture()}";
            foreach (var pluginLocation in settings.Plugins)
            {
                if (string.Equals(pluginLocation.Platform, platformKey, StringComparison.OrdinalIgnoreCase))
                   yield return pluginLocation.Settings;
            }
        }

        private static IEnumerable<CompressCodecsInfo> CreateInstances(string baseDirectoryPath, IEnumerable<Models.PluginSettingModel> pluginLocations)
        {
            var pluginFileName = $"SevenZip.NativeWrapper.Managed.{GetCurrentOs()}.{GetCurrentArchitecture()}.dll";
            foreach (var pluginLocation in pluginLocations)
            {
                foreach (var pluginDir in pluginLocation.PluginDirs.Append("."))
                {
                    var pluginPath = BuildPathName(baseDirectoryPath, Path.Combine(pluginDir, pluginFileName));
                    if (pluginPath is not null)
                    {
                        var pluginAssembly = LoadAssembly(pluginPath);
                        if (pluginAssembly is not null)
                        {
                            foreach (var pluginObject in EnumeratePluginObject(pluginAssembly))
                            {
                                if (pluginObject is null)
                                    throw new Exception($"There is no plugin file available, or there is no plugin that can be loaded in the current runtime environment.: os={GetCurrentOs()} ({GetCurrentArchitecture()})");
                                foreach (var sevenZipLibraryFilePath in pluginLocation.SevenZipLibraryFilePaths)
                                {

                                    var expandedSevenZipLibraryFilePath = BuildPathName(baseDirectoryPath, sevenZipLibraryFilePath);
                                    if (expandedSevenZipLibraryFilePath != null)
                                    {
                                        if (InitializePlugin(pluginObject, pluginAssembly, expandedSevenZipLibraryFilePath))
                                            yield return new CompressCodecsInfo(pluginObject.EnumerateCodecs());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static string? BuildPathName(string baseDirectoryPath, string pathName)
        {
            try
            {
                if (pathName.EndsWith("/", StringComparison.Ordinal) || pathName.EndsWith("\\", StringComparison.Ordinal))
                    pathName = pathName[0..^1];
                if (!pathName.Contains('/', StringComparison.Ordinal) && !pathName.Contains('\\', StringComparison.Ordinal))
                    return pathName;
                else if (string.Equals(pathName, ".", StringComparison.Ordinal)
                    || string.Equals(pathName, "..", StringComparison.Ordinal)
                    || pathName.StartsWith("./", StringComparison.Ordinal)
                    || pathName.StartsWith(".\\", StringComparison.Ordinal)
                    || pathName.StartsWith("../", StringComparison.Ordinal)
                    || pathName.StartsWith("..\\", StringComparison.Ordinal))
                {
                    var pluginPath = baseDirectoryPath;
                    foreach (var dirName in pathName.Split('/', '\\'))
                    {
                        if (string.IsNullOrEmpty(dirName))
                            throw new ArgumentException("Illegal path name format.", nameof(pathName));
                        else if (string.Equals(dirName, ".", StringComparison.Ordinal))
                        {
                            // NOP
                        }
                        else if (string.Equals(dirName, "..", StringComparison.Ordinal))
                        {
                            var parentDir = Path.GetDirectoryName(pluginPath);
                            if (parentDir is null)
                                throw new Exception();
                            pluginPath = parentDir;
                        }
                        else
                        {
                            pluginPath =
                                Path.Combine(
                                    pluginPath,
                                    dirName
                                        .Replace("${framework}", GetCurrentFramework())
                                        .Replace("${os}", GetCurrentOs())
                                        .Replace("${architecture}", GetCurrentArchitecture()));
                        }
                    }
                    if (!File.Exists(pluginPath))
                        throw new Exception($"Plugin file is not exists. : {pluginPath}");
                    return pluginPath;
                }
                else
                {
                    if (!File.Exists(pathName))
                        throw new Exception($"Plugin file is not exists. : {pathName}");
                    return
                        pathName
                            .Replace("${framework}", GetCurrentFramework())
                            .Replace("${os}", GetCurrentOs())
                            .Replace("${architecture}", GetCurrentArchitecture());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Failed to build the path name.: message=\"{ex.Message}\", baseDirectoryPath=\"{baseDirectoryPath}\", pathName=\"{pathName}\"");
                return null;
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

        private static IEnumerable<ICompressCodecsInfo> EnumeratePluginObject(Assembly pluginAssembly)
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

        private static bool InitializePlugin(ICompressCodecsInfo pluginObject, Assembly pluginAssembly, string sevenZipLibraryFilePath)
        {
            try
            {
                if (!pluginObject.Initialize(sevenZipLibraryFilePath))
                {
                    System.Diagnostics.Trace.WriteLine($"Failed to initialize plugin: assemblyLocation=\"{pluginAssembly.Location}\", sevenZipLibraryFilePath=\"{sevenZipLibraryFilePath}\"");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Failed to initialize plugin: message=\"{ex.Message}\", location={pluginAssembly.Location}");
                return false;
            }
        }

        private static string GetCurrentFramework()
        {
#if NET5_0
            return "net50";
#elif NET6_0
            return "net60";
#else
#error Undefined macro 'NETx_x'
#endif
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

        private static string GetCurrentArchitecture()
        {
            var processArchitecture = RuntimeInformation.ProcessArchitecture;
            return
                processArchitecture switch
                {
                    Architecture.X86 => "x86",
                    Architecture.X64 => "x64",
                    Architecture.Arm64 => "arm64",
                    _ => throw new NotSupportedException($"Not supported on this architecture.: {processArchitecture}"),
                };
        }
    }
}
