$package_version = "0.1.0.1"
$base_dir = resolve-path .\
$src_dir = "$base_dir\src"
$sln_file = "$src_dir\Elmah.Masking.sln"
$package_file = "$base_dir\Elmah.Masking.nuspec"
$version_exe = "$base_dir\build\UpdateVersion.exe"
$nuget_exe = "C:\Tools\NuGet.exe"
$msbuild_exe = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
$config = "Release"

& "$msbuild_exe" /v:q /t:Clean $sln_file /p:Configuration=$config /nologo

& "$version_exe" -v Assembly -p "$package_version" -i "$src_dir\Common\AssemblyInfo.cs" -o "$src_dir\Common\AssemblyInfo.cs"

& "$msbuild_exe" /v:m /t:Rebuild $sln_file /p:Configuration=$config /nologo

& "$nuget_exe" pack $package_file -verbosity detailed -Version $package_version -OutputDirectory C:\Projects\NugetPackages\
