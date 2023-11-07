Set-Location -Path ./7z
./pack.ps1
Set-Location -Path ../

#compress-archive SevenZip.NativeWrapper.Managed.linux.arm64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.linux.arm64.dll plugins/plugin.platform.net50.linux.arm64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.linux.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.arm64.so plugins/plugin.platform.net50.linux.arm64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.linux.x64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.linux.x64.dll plugins/plugin.platform.net50.linux.x64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.linux.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x64.so plugins/plugin.platform.net50.linux.x64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.linux.x86/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.linux.x86.dll plugins/plugin.platform.net50.linux.x86.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.linux.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x86.so plugins/plugin.platform.net50.linux.x86.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.macOs.arm64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.macOs.arm64.dll plugins/plugin.platform.net50.macOs.arm64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.macOs.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.arm64.dylib plugins/plugin.platform.net50.macOs.arm64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.macOs.x64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.macOs.x64.dll plugins/plugin.platform.net50.macOs.x64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.macOs.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x64.dylib plugins/plugin.platform.net50.macOs.x64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.macOs.x86/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.macOs.x86.dll plugins/plugin.platform.net50.macOs.x86.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.macOs.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x86.dylib plugins/plugin.platform.net50.macOs.x86.zip -Update

compress-archive SevenZip.NativeWrapper.Managed.win.arm64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.win.arm64.dll plugins/plugin.platform.net50.win.arm64.zip -Force
compress-archive SevenZip.NativeWrapper.Unmanaged.win.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.arm64.dll plugins/plugin.platform.net50.win.arm64.zip -Update

compress-archive SevenZip.NativeWrapper.Managed.win.x64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.win.x64.dll plugins/plugin.platform.net50.win.x64.zip -Force
compress-archive SevenZip.NativeWrapper.Unmanaged.win.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x64.dll plugins/plugin.platform.net50.win.x64.zip -Update

compress-archive SevenZip.NativeWrapper.Managed.win.x86/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.win.x86.dll plugins/plugin.platform.net50.win.x86.zip -Force
compress-archive SevenZip.NativeWrapper.Unmanaged.win.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x86.dll plugins/plugin.platform.net50.win.x86.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.linux.arm64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.linux.arm64.dll plugins/plugin.platform.net60.linux.arm64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.linux.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.arm64.so plugins/plugin.platform.net60.linux.arm64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.linux.x64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.linux.x64.dll plugins/plugin.platform.net60.linux.x64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.linux.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x64.so plugins/plugin.platform.net60.linux.x64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.linux.x86/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.linux.x86.dll plugins/plugin.platform.net60.linux.x86.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.linux.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x86.so plugins/plugin.platform.net60.linux.x86.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.macOs.arm64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.macOs.arm64.dll plugins/plugin.platform.net60.macOs.arm64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.macOs.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.arm64.dylib plugins/plugin.platform.net60.macOs.arm64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.macOs.x64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.macOs.x64.dll plugins/plugin.platform.net60.macOs.x64.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.macOs.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x64.dylib plugins/plugin.platform.net60.macOs.x64.zip -Update

#compress-archive SevenZip.NativeWrapper.Managed.macOs.x86/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.macOs.x86.dll plugins/plugin.platform.net60.macOs.x86.zip -Force
#compress-archive SevenZip.NativeWrapper.Unmanaged.macOs.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x86.dylib plugins/plugin.platform.net60.macOs.x86.zip -Update

compress-archive SevenZip.NativeWrapper.Managed.win.arm64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.win.arm64.dll plugins/plugin.platform.net60.win.arm64.zip -Force
compress-archive SevenZip.NativeWrapper.Unmanaged.win.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.arm64.dll plugins/plugin.platform.net60.win.arm64.zip -Update

compress-archive SevenZip.NativeWrapper.Managed.win.x64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.win.x64.dll plugins/plugin.platform.net60.win.x64.zip -Force
compress-archive SevenZip.NativeWrapper.Unmanaged.win.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x64.dll plugins/plugin.platform.net60.win.x64.zip -Update

compress-archive SevenZip.NativeWrapper.Managed.win.x86/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.win.x86.dll plugins/plugin.platform.net60.win.x86.zip -Force
compress-archive SevenZip.NativeWrapper.Unmanaged.win.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x86.dll plugins/plugin.platform.net60.win.x86.zip -Update


#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.arm64/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.linux.arm64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.arm64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.linux.arm64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x64/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.linux.x64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.linux.x64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x86/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.linux.x86.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x86/bin/Debug/SevenZip.NativeWrapper.Unmanaged.linux.x86.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.arm64/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.macOs.arm64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.arm64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.macOs.arm64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x64/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.macOs.x64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.macOs.x64.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x86/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.macOs.x86.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x86/bin/Debug/SevenZip.NativeWrapper.Unmanaged.macOs.x86.* -Destination Experiment/bin/Debug/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.arm64/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.win.arm64.* -Destination Experiment/bin/Debug/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.arm64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.win.arm64.* -Destination Experiment/bin/Debug/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x64/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.win.x64.* -Destination Experiment/bin/Debug/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.win.x64.* -Destination Experiment/bin/Debug/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x86/bin/Debug/net5.0/SevenZip.NativeWrapper.Managed.win.x86.* -Destination Experiment/bin/Debug/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x86/bin/Debug/SevenZip.NativeWrapper.Unmanaged.win.x86.* -Destination Experiment/bin/Debug/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.arm64/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.linux.arm64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.arm64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.linux.arm64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x64/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.linux.x64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.linux.x64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x86/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.linux.x86.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x86/bin/Debug/SevenZip.NativeWrapper.Unmanaged.linux.x86.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.arm64/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.macOs.arm64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.arm64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.macOs.arm64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x64/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.macOs.x64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.macOs.x64.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x86/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.macOs.x86.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x86/bin/Debug/SevenZip.NativeWrapper.Unmanaged.macOs.x86.* -Destination Experiment/bin/Debug/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.arm64/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.win.arm64.* -Destination Experiment/bin/Debug/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.arm64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.win.arm64.* -Destination Experiment/bin/Debug/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x64/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.win.x64.* -Destination Experiment/bin/Debug/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x64/bin/Debug/SevenZip.NativeWrapper.Unmanaged.win.x64.* -Destination Experiment/bin/Debug/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x86/bin/Debug/net6.0/SevenZip.NativeWrapper.Managed.win.x86.* -Destination Experiment/bin/Debug/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x86/bin/Debug/SevenZip.NativeWrapper.Unmanaged.win.x86.* -Destination Experiment/bin/Debug/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.arm64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.linux.arm64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.arm64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.linux.x64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x86/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.linux.x86.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x86.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.arm64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.macOs.arm64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.arm64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.macOs.x64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x64.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x86/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.macOs.x86.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x86.* -Destination Experiment/bin/Release/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.arm64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.win.arm64.* -Destination Experiment/bin/Release/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.arm64.* -Destination Experiment/bin/Release/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x64/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.win.x64.* -Destination Experiment/bin/Release/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x64.* -Destination Experiment/bin/Release/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x86/bin/Release/net5.0/SevenZip.NativeWrapper.Managed.win.x86.* -Destination Experiment/bin/Release/net5.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x86.* -Destination Experiment/bin/Release/net5.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.arm64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.linux.arm64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.arm64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.linux.x64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.linux.x86/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.linux.x86.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.linux.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.linux.x86.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.arm64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.macOs.arm64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.arm64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.macOs.x64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x64.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Managed.macOs.x86/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.macOs.x86.* -Destination Experiment/bin/Release/net6.0 -Force
#Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.macOs.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.macOs.x86.* -Destination Experiment/bin/Release/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.arm64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.win.arm64.* -Destination Experiment/bin/Release/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.arm64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.arm64.* -Destination Experiment/bin/Release/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x64/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.win.x64.* -Destination Experiment/bin/Release/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x64/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x64.* -Destination Experiment/bin/Release/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Managed.win.x86/bin/Release/net6.0/SevenZip.NativeWrapper.Managed.win.x86.* -Destination Experiment/bin/Release/net6.0 -Force
Copy-Item -Path SevenZip.NativeWrapper.Unmanaged.win.x86/bin/Release/SevenZip.NativeWrapper.Unmanaged.win.x86.* -Destination Experiment/bin/Release/net6.0 -Force
