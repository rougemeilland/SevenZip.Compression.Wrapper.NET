using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Palmtree;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class NativeInterOp
    {
        private sealed class DllNameResolver
        {
            private readonly Dictionary<String, IntPtr> _loadedDllHandles;

            public DllNameResolver()
            {
                _loadedDllHandles = new Dictionary<String, IntPtr>();
            }

            public IntPtr ResolveDllName(String libraryName, Assembly assembly, DllImportSearchPath? searchPath)
            {
                if (_loadedDllHandles.TryGetValue(libraryName, out var loadedLibraryHandle))
                {
                    return loadedLibraryHandle;
                }
                else if (libraryName == _SEVEN_ZIP_DLL_NAME)
                {
                    foreach (var libraryPath in EnumerateSevenZipDllName(assembly))
                    {
                        if (NativeLibrary.TryLoad(libraryPath, assembly, searchPath, out var handle))
                        {
                            _loadedDllHandles.Add(libraryName, handle);
                            return handle;
                        }
                    }

                    return IntPtr.Zero;
                }
                else if (libraryName == _NATIVE_METHOD_DLL_NAME)
                {
                    foreach (var libraryPath in EnumerateNativeMethodDllNames(assembly))
                    {
                        if (NativeLibrary.TryLoad(libraryPath, assembly, searchPath, out var handle))
                        {
                            _loadedDllHandles.Add(libraryName, handle);
                            return handle;
                        }
                    }

                    return IntPtr.Zero;
                }
                else
                {
                    if (NativeLibrary.TryLoad(libraryName, assembly, searchPath, out var handle))
                    {
                        _loadedDllHandles.Add(libraryName, handle);
                        return handle;
                    }

                    return IntPtr.Zero;
                }
            }

            public IntPtr GetDllHandle(String dllName) => _loadedDllHandles.TryGetValue(dllName, out var handle) ? handle : IntPtr.Zero;

            private static IEnumerable<String> EnumerateSevenZipDllName(Assembly assembly)
            {
                if (OperatingSystem.IsWindows())
                {
                    return
                        EnumerablePath(assembly, $"7z.{Platform.NativeCodeId}.dll", Platform.NugetResourceId)
                        .Concat(EnumerablePath(assembly, "7z.dll", Platform.NugetResourceId));
                }
                else if (OperatingSystem.IsLinux())
                {
                    return
                        EnumerablePath(assembly, $"lib7z.{Platform.NativeCodeId}.so", Platform.NugetResourceId)
                        .Concat(EnumerablePath(assembly, "lib7z.so", Platform.NugetResourceId))
                        .Concat(EnumerablePath(assembly, "7z.so", Platform.NugetResourceId));
                }
                else if (OperatingSystem.IsMacOS())
                {
                    return
                        EnumerablePath(assembly, $"lib7z.{Platform.NativeCodeId}.dylib", Platform.NugetResourceId)
                        .Concat(EnumerablePath(assembly, "lib7z.dylib", Platform.NugetResourceId))
                        .Concat(EnumerablePath(assembly, "7z.dylib", Platform.NugetResourceId))
                        .Concat(EnumerablePath(assembly, "lib7z.so", Platform.NugetResourceId))
                        .Concat(EnumerablePath(assembly, "7z.so", Platform.NugetResourceId));
                }
                else
                {
                    throw new NotSupportedException("Running on this operating system is not supported.");
                }
            }

            private static IEnumerable<String> EnumerateNativeMethodDllNames(Assembly assembly)
            {
                if (OperatingSystem.IsWindows())
                    return EnumerablePath(assembly, $"Palmtree.SevenZip.Compression.Wrapper.NET.Native.{Platform.NativeCodeId}.dll", Platform.NugetResourceId);
                else if (OperatingSystem.IsLinux())
                    return EnumerablePath(assembly, $"libPalmtree.SevenZip.Compression.Wrapper.NET.Native.{Platform.NativeCodeId}.so", Platform.NugetResourceId);
                else if (OperatingSystem.IsMacOS())
                    return EnumerablePath(assembly, $"libPalmtree.SevenZip.Compression.Wrapper.NET.Native.{Platform.NativeCodeId}.dylib", Platform.NugetResourceId);
                else
                    throw new NotSupportedException("Running on this operating system is not supported.");
            }

            private static IEnumerable<String> EnumerablePath(Assembly assembly, String libraryFileName, String nugetResourceId)
            {
                var baseDirectory = assembly.GetBaseDirectory();

                // アセンブリと同じディレクトリの下にライブラリファイルが存在しているかどうかを確認する
                var dllFile1 = baseDirectory.GetFile(libraryFileName);
                if (dllFile1.Exists)
                {
                    // 存在していればそのフルパスを返す
                    yield return dllFile1.FullName;
                }

                // アセンブリと同じディレクトリの "./runtimes/<platform-id>/native" の下にライブラリファイルが存在しているかどうかを確認する
                var dllFile2 = baseDirectory.GetSubDirectory("runtimes", nugetResourceId, "native").GetFile(libraryFileName);
                if (dllFile2.Exists)
                {
                    // 存在していればそのフルパスを返す
                    yield return dllFile2.FullName;
                }

                // 与えられたライブラリ名をそのまま返す。
                yield return libraryFileName;
            }
        }
    }
}
