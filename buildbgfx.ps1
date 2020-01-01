.\setupshell.ps1
cd bgfx 
&devenv .build/projects/vs2019/bgfx.sln /Upgrade
&devenv .build/projects/vs2019/bgfx.sln /Build "Release|x64"
&devenv .build/projects/vs2019/bgfx.sln /Build "Release|Win32"
cd ..