properties {
	$base_directory   = resolve-path "..\."
	$build_directory  = "$base_directory\release"
	$source_directory = "$base_directory\src\MvcSiteMapProvider"
	$nuget_directory  = "$base_directory\nuget"
	$tools_directory  = "$base_directory\tools"
	$output_directory = "$build_directory\packagesource"
	$version          = "4.0.0"
	$packageVersion   = "$version-pre"
	$configuration    = "Release"
}

include .\psake_ext.ps1

task default -depends Finalize

task Clean -description "This task cleans up the build directory" {
	Remove-Item $build_directory -Force -Recurse -ErrorAction SilentlyContinue
}

task Init -description "This tasks makes sure the build environment is correctly setup" {  
	if ($env:BuildRunner -ne $null -and $env:BuildRunner -eq "MyGet") {		
		$version = $packageVersion
		if ($version.Contains("-") -eq $true) {
			$version = $version.SubString(0, $version.IndexOf("-"))
		}
		echo "Updated version to: $version"
	}
	
	#Backup the original CommonAssemblyInfo.cs file
	Ensure-Directory-Exists "$build_directory\test.temp"
	Copy-Item "$source_directory\Shared\CommonAssemblyInfo.cs" "$build_directory\CommonAssemblyInfo.cs"

	#Get the current year from the system
	$year = [DateTime]::Today.Year

	Generate-Assembly-Info `
		-file "$source_directory\Shared\CommonAssemblyInfo.cs" `
		-company "MvcSiteMapProvider" `
		-version $version `
		-packageVersion $packageVersion `
		-copyright "Copyright © MvcSiteMapProvider 2009 - $year"
}

task Restore -depends Clean -description "This task runs NuGet package restore" {
	exec { 
		&"$tools_directory\nuget\NuGet.exe" restore "$source_directory\MvcSiteMapProvider.sln"
	}
}

task Test -depends Clean, Init, Restore -description "This tasks runs the unit tests" {
	Write-Host "Running Unit Tests..." -ForegroundColor Magenta

	$target_project = "$source_directory\MvcSiteMapProvider.Tests\MvcSiteMapProvider.Tests.csproj"

	exec { 
		msbuild $target_project `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:DefineConstants=`" MVC4`;NET40`" `
			/property:TargetFrameworkVersion=v4.0 `"
	}

	exec {
		&"$tools_directory\nunit\nunit-console.exe" $target_project /config:$configuration /noshadow /noresult /framework:net-4.0
	}
}

task Compile -depends Test -description "This task compiles the solution" {

	Write-Host "Compiling..." -ForegroundColor Green

	Build-MvcSiteMapProvider-Versions ("net35", "net40", "net45") -mvc_version "2"
	Build-MvcSiteMapProvider-Versions ("net40", "net45") -mvc_version "3"
	Build-MvcSiteMapProvider-Versions ("net40", "net45") -mvc_version "4"
	Build-MvcSiteMapProvider-Versions ("net45") -mvc_version "5"
}

task NuGet -depends Compile -description "This tasks makes creates the NuGet packages" {
	
	#create the nuget package output directory
	Ensure-Directory-Exists "$output_directory\test.temp"

	#link the legacy package to the highest MVC version
	Create-MvcSiteMapProvider-Legacy-Package -mvc_version "4"

	Create-MvcSiteMapProvider-Package -mvc_version "2"
	Create-MvcSiteMapProvider-Package -mvc_version "3"
	Create-MvcSiteMapProvider-Package -mvc_version "4"
	Create-MvcSiteMapProvider-Package -mvc_version "5"
	
	Create-MvcSiteMapProvider-Core-Package -mvc_version "2"
	Create-MvcSiteMapProvider-Core-Package -mvc_version "3"
	Create-MvcSiteMapProvider-Core-Package -mvc_version "4"
	Create-MvcSiteMapProvider-Core-Package -mvc_version "5"

	Create-MvcSiteMapProvider-Web-Package
	
	Create-DIContainer-Packages ("Autofac", "Grace", "Ninject", "SimpleInjector", "StructureMap", "Unity", "Windsor")
}

task Finalize -depends NuGet -description "This tasks finalizes the build" {  
	#Restore the original CommonAssemblyInfo.cs file from backup
	Move-Item "$build_directory\CommonAssemblyInfo.cs" "$source_directory\Shared\CommonAssemblyInfo.cs" -Force
}

function Transform-Nuspec ([string] $source, [string] $transform, [string] $destination) {
	$transform_xml = "$tools_directory\TransformXml.proj"
	Write-Host "Creating nuspec for $destination" -ForegroundColor Green
	Exec { msbuild $transform_xml /p:Source=$source /p:Transform=$transform /p:Destination=$destination /v:minimal /nologo }
	$nuspec
}

function Preprocess-Code-File ([string] $source, [string] $net_version, [string] $mvc_version) {
	$net_version_upper = $net_version.toUpper()
	Write-Host "Preprocessing code for $source, $net_version_upper, MVC$mvc_version" -ForegroundColor Green
	Copy-Item $source "$source.temp"
	Exec { &"$tools_directory\mcpp-2.7.2\mcpp" -C -P "-D$net_version_upper" "-DMVC$mvc_version" "$source.temp" $source }
	Remove-Item "$source.temp" -Force -ErrorAction SilentlyContinue
}

function Preprocess-Code-Files ([string] $path, [string] $net_version, [string] $mvc_version) {
	$net_version_upper = $net_version.toUpper()
	Get-Childitem -path "$path\*" -recurse -include *.cs | % {
		$file = $_
		Begin-Preserve-Symbols "$file"
		Preprocess-Code-File "$file" $net_version $mvc_version
		End-Preserve-Symbols "$file"
		Tokenize-Namespaces "$file"
		Rename-Item "$file" "$file.pp" #must be done last
	}
}

function Begin-Preserve-Symbols ([string] $source) {
	(Get-Content $source) | % {
		$_-replace "((?:#if\s+\w+|#else|#endif)\s*?//\s*?[Pp]reserve|#region|#endregion)", '//$1'
	} | Set-Content $source -Force
}

function End-Preserve-Symbols ([string] $source) {
	(Get-Content $source) | % {
		$_-replace "//(?:(#if\s+\w+|#else|#endif)\s*?//\s*?[Pp]reserve)|//(#region|#endregion)", '$1$2'
	} | Set-Content $source -Force
}

function Tokenize-Namespaces ([string] $source) {
	(Get-Content $source) | % {
		$_-replace "(namespace|using)\s+(DI)", '$1 $rootnamespace$.$2'
	} | Set-Content $source -Force
}

function Build-MvcSiteMapProvider-Versions ([string[]] $net_versions, [string] $mvc_version) {
	#create the build for each version of the framework
	foreach ($net_version in $net_versions) {
		Build-MvcSiteMapProvider-Version $net_version $mvc_version
		Build-MvcSiteMapProvider-WebActivator-Version $net_version $mvc_version
	}
}

function Build-MvcSiteMapProvider-Version ([string] $net_version, [string] $mvc_version) {
	$net_version_upper = $net_version.toUpper()
	Write-Host "Compiling MvcSiteMapProvider for $net_version_upper, MVC$mvc_version" -ForegroundColor Blue
	$outdir = "$build_directory\mvcsitemapprovider.mvc$mvc_version.core\lib\$net_version\"
	$documentation_file = $outdir + "MvcSiteMapProvider.xml"
	$targetFramework = Get-TargetFramework-Version $net_version
	Write-Host "Targeting framework $targetFramework" -ForegroundColor Cyan

	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$outdir `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:DefineConstants=`" MVC$mvc_version`;$net_version_upper`" `
			/property:TargetFrameworkVersion=$targetFramework `
			/property:DocumentationFile=`"$documentation_file`"
	}

	dir $outdir | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }
}

function Build-MvcSiteMapProvider-WebActivator-Version ([string] $net_version, [string] $mvc_version) {
	$net_version_upper = $net_version.toUpper()
	Write-Host "Compiling MvcSiteMapProvider.WebActivator for $net_version_upper, MVC$mvc_version" -ForegroundColor Blue
	$outdir = "$build_directory\mvcsitemapprovider.mvc$mvc_version\lib\$net_version\"
	$documentation_file = $outdir + "MvcSiteMapProvider.WebActivator.xml"
	$targetFramework = Get-TargetFramework-Version $net_version
	Write-Host "Targeting framework $targetFramework" -ForegroundColor Cyan

	#Create a webactivator for all versions except .NET 3.5
	if ($net_version -ne "net35") {
		exec { 
			msbuild $source_directory\MvcSiteMapProvider.WebActivator\MvcSiteMapProvider.WebActivator.csproj `
				/property:outdir=$outdir `
				/verbosity:quiet `
				/property:Configuration=$configuration `
				"/t:Clean;Rebuild" `
				/property:WarningLevel=3 `
				/property:DefineConstants=`" MVC$mvc_version`;$net_version_upper`" `
				/property:TargetFrameworkVersion=$targetFramework `
				/property:DocumentationFile=`"$documentation_file`"
		}

		dir $outdir | ?{ -not($_.Name -match 'MvcSiteMapProvider.WebActivator') } | %{ del $_.FullName }
	}
}

function Create-MvcSiteMapProvider-Legacy-Package ([string] $mvc_version) {
	$output_nuspec_file = "$build_directory\mvcsitemapprovider\mvcsitemapprovider.nuspec"
	Ensure-Directory-Exists $output_nuspec_file
	Copy-Item $nuget_directory\mvcsitemapprovider.legacy\mvcsitemapprovider.nuspec "$output_nuspec_file.template"

	$prerelease = Get-Prerelease-Text

	#replace the tokens
	(cat "$output_nuspec_file.template") `
		-replace '#mvc_version#', "$mvc_version" `
		-replace '#prerelease#', "$prerelease" `
		> $output_nuspec_file 
	
	#delete the template file
	Remove-Item "$output_nuspec_file.template" -Force -ErrorAction SilentlyContinue

	Copy-Item $nuget_directory\mvcsitemapprovider.legacy\* $build_directory\mvcsitemapprovider -Recurse -Exclude @("*.nuspec", "*.nutrans")
	
	exec { 
		&"$tools_directory\nuget\NuGet.exe" pack $output_nuspec_file -Symbols -Version $packageVersion -OutputDirectory $output_directory
	}
}

function Create-MvcSiteMapProvider-Package ([string] $mvc_version) {
	$output_nuspec_file = "$build_directory\mvcsitemapprovider.mvc$mvc_version\mvcsitemapprovider.nuspec"
	Ensure-Directory-Exists $output_nuspec_file
	Copy-Item $nuget_directory\mvcsitemapprovider\mvcsitemapprovider.shared.nuspec "$output_nuspec_file.template"

	$prerelease = Get-Prerelease-Text

	#replace the tokens
	(cat "$output_nuspec_file.template") `
		-replace '#mvc_version#', "$mvc_version" `
		-replace '#prerelease#', "$prerelease" `
		> $output_nuspec_file 
	
	#delete the template file
	Remove-Item "$output_nuspec_file.template" -Force -ErrorAction SilentlyContinue

	Copy-Item $nuget_directory\mvcsitemapprovider\* $build_directory\mvcsitemapprovider.mvc$mvc_version -Recurse -Exclude @("*.nuspec", "*.nutrans")

	exec { 
		&"$tools_directory\nuget\NuGet.exe" pack $output_nuspec_file -Symbols -Version $packageVersion -OutputDirectory $output_directory
	}
}

function Create-MvcSiteMapProvider-Core-Package ([string] $mvc_version) {
	$output_nuspec_file = "$build_directory\mvcsitemapprovider.mvc$mvc_version.core\mvcsitemapprovider.core.nuspec"
	Ensure-Directory-Exists $output_nuspec_file
	Transform-Nuspec $nuget_directory\mvcsitemapprovider.core\mvcsitemapprovider.core.shared.nuspec $nuget_directory\mvcsitemapprovider.core\mvcsitemapprovider.mvc$mvc_version.core.nutrans "$output_nuspec_file.template"
	
	$prerelease = Get-Prerelease-Text

	#replace the tokens
	(cat "$output_nuspec_file.template") `
		-replace '#mvc_version#', "$mvc_version" `
		-replace '#prerelease#', "$prerelease" `
		> $output_nuspec_file 
	
	#delete the template file
	Remove-Item "$output_nuspec_file.template" -Force -ErrorAction SilentlyContinue

	Copy-Item $nuget_directory\mvcsitemapprovider.core\* $build_directory\mvcsitemapprovider.mvc$mvc_version.core -Recurse -Exclude @("*.nuspec", "*.nutrans")
		
	#copy sources for symbols package
	Copy-Item -Recurse -Filter *.cs -Force "$source_directory\MvcSiteMapProvider" "$build_directory\mvcsitemapprovider.mvc$mvc_version.core\src"

	exec { 
		&"$tools_directory\nuget\NuGet.exe" pack $output_nuspec_file -Symbols -Version $packageVersion -OutputDirectory $output_directory
	}
}

function Create-MvcSiteMapProvider-Web-Package {

	Copy-Item $nuget_directory\mvcsitemapprovider.web\ $build_directory\mvcsitemapprovider.web -Recurse

	Ensure-Directory-Exists $build_directory\mvcsitemapprovider.web\content\MvcSiteMapSchema.xsd
	Copy-Item $source_directory\MvcSiteMapProvider\Xml\MvcSiteMapSchema.xsd $build_directory\mvcsitemapprovider.web\content\MvcSiteMapSchema.xsd

	$display_templates_output = "$build_directory\mvcsitemapprovider.web\content\Views\Shared\DisplayTemplates"
	Ensure-Directory-Exists $display_templates_output\test.temp
	Copy-Item $source_directory\MvcSiteMapProvider\Web\Html\DisplayTemplates\* $display_templates_output -Recurse
	
	exec { 
		&"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.web\mvcsitemapprovider.web.nuspec -Symbols -Version $packageVersion -OutputDirectory $output_directory
	}
}

function Create-DIContainer-Packages ([string[]] $di_containers) {
	#create the build for each DI container
	foreach ($di_container in $di_containers) {
		Write-Host $di_container -ForegroundColor Yellow

		#exception: SimpleInjector version 2 doesn't support .NET 3.5
		#exception: Grace doesn't support .NET 3.5
		if ($di_container -ne "SimpleInjector" -and $di_container -ne "Grace") {
			Create-DIContainer-Package $di_container ("net35", "net40", "net45") -mvc_version "2"
			Create-DIContainer-Modules-Package $di_container ("net35", "net40", "net45") -mvc_version "2"
		}

		Create-DIContainer-Package $di_container ("net40", "net45") -mvc_version "3"
		Create-DIContainer-Modules-Package $di_container ("net40", "net45") -mvc_version "3"

		Create-DIContainer-Package $di_container ("net40", "net45") -mvc_version "4"
		Create-DIContainer-Modules-Package $di_container ("net40", "net45") -mvc_version "4"

		Create-DIContainer-Package $di_container ("net45") -mvc_version "5"
		Create-DIContainer-Modules-Package $di_container ("net45") -mvc_version "5"
	}
}

function Create-DIContainer-Package ([string] $di_container, [string[]] $net_versions, [string] $mvc_version) {
	#create the build for each version of the framework
	foreach ($net_version in $net_versions) {
		Create-DIContainer-Build $di_container $net_version $mvc_version
	}

	#copy readme file
	Copy-Item $source_directory\codeasconfiguration\mvcsitemapprovider_di_readme.txt $build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container\content\MvcSiteMapProvider_DI_ReadMe.txt

	#copy readme file to root also so it will open automatically
	Copy-Item $source_directory\codeasconfiguration\mvcsitemapprovider_di_readme.txt $build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container\readme.txt


	#package the build
	exec { 
		&"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container\mvcsitemapprovider.mvc$mvc_version.di.$di_container.nuspec -Symbols -Version $packageVersion -OutputDirectory $output_directory
	}
}

function Create-DIContainer-Build ([string] $di_container, [string] $net_version, [string] $mvc_version) {

	Write-Host "Creating DI build for $di_container, $net_version, MVC$mvc_version" -ForegroundColor Blue

	#create nuspec file
	Create-DIContainer-Nuspec-File $di_container $mvc_version
	
	#create output directores
	$output_directory = "$build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container\content\$net_version"
	Ensure-Directory-Exists "$output_directory\App_Start\test.temp"
	Ensure-Directory-Exists "$output_directory\DI\$di_container\test.temp"

	#copy configuration files
	Copy-Item $source_directory\codeasconfiguration\shared\App_Start\* $output_directory\App_Start -Recurse
	Copy-Item $source_directory\codeasconfiguration\shared\DI\* $output_directory\DI -Recurse	
	Copy-Item $source_directory\codeasconfiguration\$di_container\App_Start\* $output_directory\App_Start -Recurse
	Copy-Item "$source_directory\codeasconfiguration\$di_container\DI\$di_container\$($di_container)DependencyInjectionContainer.cs" "$output_directory\DI\$di_container\$($di_container)DependencyInjectionContainer.cs"

	#remove common conventions (auto registration) - this is in the modules package already
	Remove-Item $output_directory\DI\CommonConventions.cs -Force -ErrorAction SilentlyContinue

	if ($mvc_version -eq "2") {
		#remove dependency resolver file if this is MVC 2 - not supported
		Remove-Item $output_directory\DI\InjectableDependencyResolver.cs -Force -ErrorAction SilentlyContinue
	}

	#pre-process the compiler symbols in the configuration files
	Preprocess-Code-Files $output_directory $net_version $mvc_version
}

function Create-DIContainer-Nuspec-File ([string] $di_container, [string] $mvc_version) {
	$nuspec_shared = "$nuget_directory\mvcsitemapprovider.di\mvcsitemapprovider.di.shared.nuspec"
	$output_file = "$build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container\mvcsitemapprovider.mvc$mvc_version.di.$di_container.nuspec"
	Ensure-Directory-Exists $output_file
	Copy-Item $nuspec_shared "$output_file.template"

	$prerelease = Get-Prerelease-Text

	#replace the tokens
	(cat "$output_file.template") `
		-replace '#di_container_name#', "$di_container" `
		-replace '#mvc_version#', "$mvc_version" `
		-replace '#prerelease#', "$prerelease" `
		> $output_file 

	#delete the template file
	Remove-Item "$output_file.template" -Force -ErrorAction SilentlyContinue
}

function Create-DIContainer-Modules-Package ([string] $di_container, [string[]] $net_versions, [string] $mvc_version) {
	#create the build for each version of the framework
	foreach ($net_version in $net_versions) {
		Create-DIContainer-Modules-Build $di_container $net_version $mvc_version
	}

	#copy readme file to root so it will open automatically
	Copy-Item $source_directory\codeasconfiguration\$di_container\di\$di_container\readme.txt $build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules\readme.txt

	# copy the tools directory
	Copy-Item $nuget_directory\mvcsitemapprovider.di.modules\tools $build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules\tools -Recurse

	#package the build
	exec { 
		&"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules.nuspec -Symbols -Version $packageVersion -OutputDirectory $output_directory
	}
}

function Create-DIContainer-Modules-Build ([string] $di_container, [string] $net_version, [string] $mvc_version) {

	Write-Host "Creating DI Modules build for $di_container, $net_version, MVC$mvc_version" -ForegroundColor Blue

	#create nuspec file
	Create-DIContainer-Modules-Nuspec-File $di_container $mvc_version
	
	#create output directores
	$output_directory = "$build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules\content\$net_version"
	Ensure-Directory-Exists "$output_directory\DI\test.temp"

	#copy configuration files	
	$source = "$source_directory\codeasconfiguration\$di_container\DI"
	$dest = "$output_directory\DI"
	Get-ChildItem $source -Recurse -Exclude @("*Container.cs") | Copy-Item -Destination {Join-Path $dest $_.FullName.Substring($source.length)}

	#copy common conventions (auto registration)
	Copy-Item $source_directory\codeasconfiguration\shared\DI\CommonConventions.cs $output_directory\DI\CommonConventions.cs

	#pre-process the compiler symbols in the configuration files
	Preprocess-Code-Files $output_directory $net_version $mvc_version
}

function Create-DIContainer-Modules-Nuspec-File ([string] $di_container, [string] $mvc_version) {
	$nuspec_shared = "$nuget_directory\mvcsitemapprovider.di.modules\mvcsitemapprovider.di.modules.shared.nuspec"
	$output_file = "$build_directory\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules.nuspec"
	Ensure-Directory-Exists $output_file
	Transform-Nuspec $nuspec_shared "$nuget_directory\mvcsitemapprovider.di.modules\mvcsitemapprovider.di.$di_container.modules.nutrans" "$output_file.template"
	if (Test-Path "$nuget_directory\mvcsitemapprovider.di.modules\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules.nutrans") {
		Transform-Nuspec $nuspec_shared "$nuget_directory\mvcsitemapprovider.di.modules\mvcsitemapprovider.mvc$mvc_version.di.$di_container.modules.nutrans" "$output_file.template"
	}
	
	$prerelease = Get-Prerelease-Text

	#replace the tokens
	(cat "$output_file.template") `
		-replace '#di_container_name#', "$di_container" `
		-replace '#mvc_version#', "$mvc_version" `
		-replace '#prerelease#', "$prerelease" `
		> $output_file 

	#delete the template file
	Remove-Item "$output_file.template" -Force -ErrorAction SilentlyContinue
}

function Get-Prerelease-Text {
	if ($packageVersion.Contains("-")) {
		$prerelease = $packageVersion.SubString($packageVersion.IndexOf("-")) -replace "\d+", ""
		return ".0$prerelease"
	}
	return ""
}

function Get-TargetFramework-Version ([string] $net_version) {
	$targetFramework = "v4.0"
	if ($net_version -eq "net35") {
		$targetFramework = "v3.5"
	}
	if ($net_version -eq "net40") {
		$targetFramework = "v4.0"
	}
	if ($net_version -eq "net45") {
		$targetFramework = "v4.5"
	}
	return $targetFramework
}