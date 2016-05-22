param($scriptRoot)

$ErrorActionPreference = "Stop"

$msBuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"
#$msBuild = "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
$nuGet = "$scriptRoot..\tools\NuGet.exe"
$solution = "$scriptRoot\..\Sitecore.Datalift.sln"

& $nuGet restore $solution
& $msBuild $solution /p:Configuration=Release /t:Rebuild /m

$SitecoreDataliftAssembly = Get-Item "$scriptRoot\..\src\Sitecore.Datalift\bin\Sitecore.Datalift.dll" | Select-Object -ExpandProperty VersionInfo
$targetAssemblyVersion = $SitecoreDataliftAssembly.ProductVersion

& $nuGet pack "$scriptRoot\..\src\Sitecore.Datalift\Sitecore.Datalift.csproj" -Symbols -Prop "Configuration=Release"

COPY *.nupkg "D:\NuGet Repository"