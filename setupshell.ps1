$PrevPath = Get-Location;
if( $env:IS_PIPELINE -eq 'true' ){
&Import-Module "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\Microsoft.VisualStudio.DevShell.dll"; Enter-VsDevShell 11a2e51d  
}else{
&Import-Module "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\Microsoft.VisualStudio.DevShell.dll"; Enter-VsDevShell 11a2e51d  
}
Set-Location $PrevPath