$package_version = "0.0.1"
$base_dir = resolve-path .\
$sln_file = "$base_dir\src\Elmah.Masking.sln"
$package_file = "$base_dir\Elmah.Masking.nuspec"
$nuget_exe = "C:\Tools\NuGet.exe"
$msbuild_exe = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
$config = "Release"

& "$msbuild_exe" /v:q /t:Clean $sln_file /p:Configuration=$config /nologo
 
& "$msbuild_exe" /v:m /t:Rebuild $sln_file /p:Configuration=$config /nologo

& "$nuget_exe" pack $package_file -Version $package_version -OutputDirectory C:\Projects\NugetPackages\
