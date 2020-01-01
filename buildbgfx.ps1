.\setupshell.ps1
cd bgfx 
cd .build/projects/vs2019/
$configFiles = Get-ChildItem . *.vcxproj -rec
foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "<WindowsTargetPlatformVersion>8.1</WindowsTargetPlatformVersion>", "<WindowsTargetPlatformVersion>10.0</WindowsTargetPlatformVersion>" } |
    Set-Content $file.PSPath
}
cd ../../../
&devenv .build/projects/vs2019/bgfx.sln /Build "Release|x64"
&devenv .build/projects/vs2019/bgfx.sln /Build "Release|Win32"
cd ..