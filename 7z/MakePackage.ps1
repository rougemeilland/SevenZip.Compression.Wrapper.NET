$baseDirectory = $PSScriptRoot
$currentDirectory = Get-Location

try {
    $packagePath = (Join-Path $baseDirectory.ToString() "package")
    $outputPath = (Join-Path $baseDirectory.ToString() "bin/Release")
    if ( -not ( Test-Path -Path $packagePath ) ) {
        New-Item -Path $packagePath -Type Directory -Force
    }
    if ( -not ( Test-Path -Path $outputPath ) ) {
        New-Item -Path $outputPath -Type Directory -Force
    }
    Copy-Item (Join-Path $baseDirectory "Palmtree.SevenZip.Compression.Wrapper.NET.Engine.nuspec") -Destination $packagePath
    Copy-Item (Join-Path $baseDirectory "Engine.target") -Destination $packagePath
    Set-Location $packagePath
    $process = (Start-Process -FilePath "nuget" -ArgumentList "pack Palmtree.SevenZip.Compression.Wrapper.NET.Engine.nuspec -OutputDirectory ../bin/Release" -NoNewWindow -PassThru)
    $handle = $process.Handle # おまじない https://stackoverflow.com/questions/10262231/obtaining-exitcode-using-start-process-and-waitforexit-instead-of-wait
    $process.WaitForExit()
    if ($process.ExitCode -gt 1) {
        Write-Host ("nuget の実行に失敗しました。: 終了コード: " + $process.ExitCode)
        Exit 1
    }
}
finally {
    Set-Location $currentDirectory
}
