.\setupshell.ps1
cd BGFX.Net
msbuild BGFX.Net.sln /P:'Configuration=Release'
msbuild BGFX.Net.sln /P:'Configuration=Release;Platform=x64'
msbuild BGFX.Net.sln /P:'Configuration=Release;Platform=x86'
cd ..