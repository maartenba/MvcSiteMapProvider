function Generate-Assembly-Info
{
param(
	[string]$copyright, 
	[string]$version,
	[string]$packageVersion,
	[string]$company,
	[string]$file = $(throw "file is a required parameter.")
)
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: CLSCompliantAttribute(false)]
[assembly: ComVisibleAttribute(false)]
[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyInformationalVersionAttribute(""$packageVersion"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: AssemblyDelaySignAttribute(false)]
"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	out-file -filePath $file -encoding UTF8 -inputObject $asmInfo
}

function Generate-Nuspec-File 
{
param( 
	[string]$version,
	[string]$id,
	[string]$file = $(throw "file is a required parameter."),
	[string[]]$dependencies
	
)
  $contents = "<?xml version=""1.0""?>
<package xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <metadata xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
    <id>$id</id>
	<title>MvcSiteMapProvider.Web</title>
    <version>$version</version>
    <authors>Maarten Balliauw</authors>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>This project contains extra configuration that is required by MvcSiteMapProvider during installation into a Web project as well as a starting point for XML configuration of the provider.</description>
    <summary>MvcSiteMapProvider is a SiteMapProvider implementation for the ASP.NET MVC framework. The project is hosted on http://github.com/maartenba/MvcSiteMapProvider.</summary>
    <language>en-US</language>
    <tags>mvc sitemap menu breadcrumb navigation</tags>
    <projectUrl>http://github.com/maartenba/MvcSiteMapProvider</projectUrl>
    <iconUrl>http://download.codeplex.com/Project/Download/FileDownload.aspx?ProjectName=mvcsitemap&amp;DownloadId=196029</iconUrl>"
	
	if ($dependencies.Length -gt 0) {
		$contents = "$contents
	<dependencies>";

	foreach ($dependency in $dependencies) {
		$contents = "$contents
		<dependency id=`"$dependency`" />";
	}

		$contents = "$contents
	</dependencies>";
	}
	
	$contents = "$contents
  </metadata>
</package>
"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating nuspec file: $file"
	out-file -filePath $file -encoding UTF8 -inputObject $contents
}

function Ensure-Directory-Exists($file)
{
	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
}