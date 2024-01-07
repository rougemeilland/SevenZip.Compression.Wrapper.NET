using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Palmtree.IO;

namespace SevenZip.Compression.NativeInterfaces
{
    internal partial class NativeInterOp
    {
        private class DllNameResolver
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
                        (RuntimeInformation.ProcessArchitecture switch
                        {
                            Architecture.X86 => EnumerablePath(assembly, "7z.win_x86.dll"),
                            Architecture.X64 => EnumerablePath(assembly, "7z.win_x64.dll"),
                            Architecture.Arm => EnumerablePath(assembly, "7z.win_arm32.dll"),
                            Architecture.Arm64 => EnumerablePath(assembly, "7z.win_arm64.dll"),
                            _ => throw new NotSupportedException($"Running on this architecture is not supported. : architecture={RuntimeInformation.ProcessArchitecture}"),
                        })
                        .Concat(EnumerablePath(assembly, "7z.dll"));
                }
                else if (OperatingSystem.IsLinux())
                {
                    return
                        (RuntimeInformation.ProcessArchitecture switch
                        {
                            Architecture.X86 => EnumerablePath(assembly, "lib7z.linux_x86.so"),
                            Architecture.X64 => EnumerablePath(assembly, "lib7z.linux_x64.so"),
                            Architecture.Arm => EnumerablePath(assembly, "lib7z.linux_arm32.so"),
                            Architecture.Arm64 => EnumerablePath(assembly, "lib7z.linux_arm64.so"),
                            _ => throw new NotSupportedException($"Running on this architecture is not supported. : architecture={RuntimeInformation.ProcessArchitecture}"),
                        })
                        .Concat(EnumerablePath(assembly, "lib7z.so"))
                        .Concat(EnumerablePath(assembly, "7z.so"));
                }
                else if (OperatingSystem.IsMacOS())
                {
                    return
                        (RuntimeInformation.ProcessArchitecture switch
                        {
                            Architecture.X86 => EnumerablePath(assembly, "lib7z.osx_x86.dylib"),
                            Architecture.X64 => EnumerablePath(assembly, "lib7z.osx_x64.dylib"),
                            Architecture.Arm => EnumerablePath(assembly, "lib7z.osx_arm32.dylib"),
                            Architecture.Arm64 => EnumerablePath(assembly, "lib7z.osx_arm64.dylib"),
                            _ => throw new NotSupportedException($"Running on this architecture is not supported. : architecture={RuntimeInformation.ProcessArchitecture}"),
                        })
                        .Concat(EnumerablePath(assembly, "lib7z.dylib"))
                        .Concat(EnumerablePath(assembly, "7z.dylib"))
                        .Concat(EnumerablePath(assembly, "lib7z.so"))
                        .Concat(EnumerablePath(assembly, "7z.so"));
                }
                else
                {
                    throw new NotSupportedException("Running on this operating system is not supported.");
                }
            }

            private static IEnumerable<String> EnumerateNativeMethodDllNames(Assembly assembly)
            {
                if (OperatingSystem.IsWindows())
                {
                    return
                        RuntimeInformation.ProcessArchitecture switch
                        {
                            Architecture.X86 => EnumerablePath(assembly, "Palmtree.SevenZip.Compression.Wrapper.NET.Native.win_x86.dll"),
                            Architecture.X64 => EnumerablePath(assembly, "Palmtree.SevenZip.Compression.Wrapper.NET.Native.win_x64.dll"),
                            Architecture.Arm => EnumerablePath(assembly, "Palmtree.SevenZip.Compression.Wrapper.NET.Native.win_arm32.dll"),
                            Architecture.Arm64 => EnumerablePath(assembly, "Palmtree.SevenZip.Compression.Wrapper.NET.Native.win_arm64.dll"),
                            _ => throw new NotSupportedException($"Running on this architecture is not supported. : architecture={RuntimeInformation.ProcessArchitecture}"),
                        };
                }
                else if (OperatingSystem.IsLinux())
                {
                    return
                        RuntimeInformation.ProcessArchitecture switch
                        {
                            Architecture.X86 => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.linux_x86.so"),
                            Architecture.X64 => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.linux_x64.so"),
                            Architecture.Arm => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.linux_arm32.so"),
                            Architecture.Arm64 => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.linux_arm64.so"),
                            _ => throw new NotSupportedException($"Running on this architecture is not supported. : architecture={RuntimeInformation.ProcessArchitecture}"),
                        };
                }
                else if (OperatingSystem.IsMacOS())
                {
                    return
                        RuntimeInformation.ProcessArchitecture switch
                        {
                            Architecture.X86 => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.osx_x86.dylib"),
                            Architecture.X64 => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.osx_x64.dylib"),
                            Architecture.Arm => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.osx_arm32.dylib"),
                            Architecture.Arm64 => EnumerablePath(assembly, "libPalmtree.SevenZip.Compression.Wrapper.NET.Native.osx_arm64.dylib"),
                            _ => throw new NotSupportedException($"Running on this architecture is not supported. : architecture={RuntimeInformation.ProcessArchitecture}"),
                        };
                }
                else
                {
                    throw new NotSupportedException("Running on this operating system is not supported.");
                }
            }

            private static IEnumerable<String> EnumerablePath(Assembly assembly, String libraryName)
            {
                // 先に、アセンブリと同じディレクトリの下にライブラリファイルが存在しているかどうかを確認する
                var file = assembly.GetBaseDirectory().GetFile(libraryName);
                if (file.Exists)
                {
                    // 存在していればそのフルパスを返す
                    yield return file.FullName;
                }

                // 次に、与えられたライブラリ名をそのまま返す。
                yield return libraryName;
            }
        }
    }
}
