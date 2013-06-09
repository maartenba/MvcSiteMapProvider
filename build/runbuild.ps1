properties {
    $base_directory   = resolve-path "..\."
    $build_directory  = "$base_directory\release"
    $source_directory = "$base_directory\src\MvcSiteMapProvider"
    $nuget_directory  = "$base_directory\nuget"
    $tools_directory  = "$base_directory\tools"
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
    Generate-Assembly-Info `
		-file "$source_directory\Shared\CommonAssemblyInfo.cs" `
		-company "MvcSiteMapProvider" `
		-version $version `
		-copyright "Copyright © MvcSiteMapProvider 2009 - 2013"
}

task Compile -depends Clean, Init -description "This task compiles the solution" {

	Write-Host "Compiling..." -ForegroundColor Green
	# MVC 2
	Build-MvcSiteMapProvider-Versions ("net35", "net40", "net45") "2"
	# MVC 3
	Build-MvcSiteMapProvider-Versions ("net35", "net40", "net45") "3"
	# MVC 4
	Build-MvcSiteMapProvider-Versions ("net40", "net45") "4"
}

task NuGet -depends Compile -description "This tasks makes creates the NuGet packages" {
	# MVC 2
	Create-MvcSiteMapProvider-Package "2"
	# MVC 3
	Create-MvcSiteMapProvider-Package "3"
	# MVC 4
	Create-MvcSiteMapProvider-Package "4"
	Create-MvcSiteMapProvider-Web-Package
	Create-Configuration-DIContainer-Packages ("Autofac", "Ninject", "StructureMap", "Unity", "Windsor")

    Move-Item *.nupkg $base_directory\release
}

task Finalize -depends NuGet -description "This tasks finalizes the build" {  

}

function Transform-Nuspec ($source, $transform, $destination) {
    $transform_xml = "$tools_directory\TransformXml.proj"
    Write-Host "Creating nuspec for $destination" -ForegroundColor Green
    Exec { msbuild $transform_xml /p:Source=$source /p:Transform=$transform /p:Destination=$destination /v:minimal /nologo }
    $nuspec
}

function Preprocess-Code-File ($source, $net_version, $mvc_version) {
	$net_version_upper = $net_version.toUpper()
    Write-Host "Preprocessing code for $source, $net_version_upper, MVC$mvc_version" -ForegroundColor Green
	Copy-Item $source "$source.temp"
    Exec { &"$tools_directory\mcpp-2.7.2\mcpp" -C -P "-D$net_version_upper" "-DMVC$mvc_version" "$source.temp" $source }
	Remove-Item "$source.temp" -Force -ErrorAction SilentlyContinue
}

function Preprocess-Code-Files ($path, $net_version, $mvc_version) {
	$net_version_upper = $net_version.toUpper()
	Get-Childitem -path "$path\*" -recurse -include *.cs | % {
		$file = $_
		Preprocess-Code-File "$file" $net_version $mvc_version
	}
}

function Build-MvcSiteMapProvider-Versions ([string[]] $net_versions, [string] $mvc_version) {
	#create the build for each version of the framework
	foreach ($net_version in $net_versions) {
		Build-MvcSiteMapProvider-Version $net_version $mvc_version
	}
}

function Build-MvcSiteMapProvider-Version ([string] $net_version, [string] $mvc_version) {
	$net_version_upper = $net_version.toUpper()
	Write-Host "Compiling MvcSiteMapProvider for $net_version_upper, MVC$mvc_version" -ForegroundColor Blue
	$outdir = "$build_directory\mvcsitemapprovider.mvc$mvc_version\lib\$net_version\"
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$outdir `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:DefineConstants=`" MVC$mvc_version`;$net_version_upper`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $outdir | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }
}

function Create-MvcSiteMapProvider-Package ([string] $mvc_version) {
	$output_nuspec_file = "$build_directory\mvcsitemapprovider.mvc$mvc_version\mvcsitemapprovider.nuspec"
	Ensure-Directory-Exists $output_nuspec_file
	Transform-Nuspec $nuget_directory\mvcsitemapprovider\mvcsitemapprovider.shared.nuspec $nuget_directory\mvcsitemapprovider\mvcsitemapprovider.mvc$mvc_version.nutrans $output_nuspec_file
	Copy-Item $nuget_directory\mvcsitemapprovider\* $build_directory\mvcsitemapprovider.mvc$mvc_version -Recurse -Exclude @("*.nuspec", "*.nutrans")
	
	#determine if we are prerelease
	$prerelease_switch = ""
	if ($packageVersion.Contains("-")) {
		$prerelease_switch = "-Prerelease"
	}

	#replace the tokens in init.ps1
	$init_file = "$build_directory\mvcsitemapprovider.mvc$mvc_version\tools\init.ps1"
	Copy-Item $init_file "$init_file.template"
	(cat "$init_file.template") `
		-replace '#prerelease_switch#', "$prerelease_switch" `
		> $init_file 

	#delete the template file
	Remove-Item "$init_file.template" -Force -ErrorAction SilentlyContinue

    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc$mvc_version\mvcsitemapprovider.nuspec -Symbols -Version $packageVersion
    }
}

function Create-MvcSiteMapProvider-Web-Package {
    Generate-Nuspec-File `
		-id "MvcSiteMapProvider.Web" `
		-file "$build_directory\mvcsitemapprovider.web\mvcsitemapprovider.web.nuspec" `
		-version $packageVersion `
		-dependencies @("WebActivatorEx `" version=`"2.0")

		#-dependencies @("MvcSiteMapProvider `" version=`"4.0", "WebActivatorEx `" version=`"2.0")
	
    Copy-Item $nuget_directory\mvcsitemapprovider.web\* $build_directory\mvcsitemapprovider.web -Recurse
    Copy-Item $source_directory\MvcSiteMapProvider\Xml\MvcSiteMapSchema.xsd $build_directory\mvcsitemapprovider.web\content\MvcSiteMapSchema.xsd
	Ensure-Directory-Exists $build_directory\mvcsitemapprovider.web\content\Views\Shared\DisplayTemplates\test.temp
    Copy-Item $source_directory\MvcSiteMapProvider\Web\Html\DisplayTemplates\* $build_directory\mvcsitemapprovider.web\content\Views\Shared\DisplayTemplates -Recurse
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.web\mvcsitemapprovider.web.nuspec -Symbols -Version $packageVersion
    }
}

function Create-Configuration-DIContainer-Packages ([string[]] $di_containers) {
	#create the build for each DI container
	foreach ($di_container in $di_containers) {
		Write-Host $di_container -ForegroundColor Yellow
		Create-Configuration-DIContainer-Package $di_container ("net35", "net40", "net45") "2"
		Create-Configuration-DIContainer-Package $di_container ("net35", "net40", "net45") "3"
		Create-Configuration-DIContainer-Package $di_container ("net40", "net45") "4"
	}
}

function Create-Configuration-DIContainer-Package ([string] $di_container, [string[]] $net_versions, [string] $mvc_version) {
	#create the build for each version of the framework
	foreach ($net_version in $net_versions) {
		Create-Configuration-Build $di_container $net_version $mvc_version
	}

	#copy readme file
	Copy-Item $source_directory\codeasconfiguration\mvcsitemapprovider_configuration_readme.txt $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\MvcSiteMapProvider_Configuration_ReadMe.txt

	#copy web.config file
	Copy-Item $nuget_directory\mvcsitemapprovider.configuration\web.config.transform $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\web.config.transform

	#package the build
	exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container.nuspec -Symbols -Version $packageVersion
    }
}


function Create-Configuration-Build ([string] $di_container, [string] $net_version, [string] $mvc_version) {

	Write-Host "Creating configuration build for $di_container, $net_version, MVC$mvc_version" -ForegroundColor Blue

	#create nuspec file
	Create-Configuration-Nuspec-File $di_container $mvc_version
	
	#create output directores
	$output_directory = "$build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version"
	Ensure-Directory-Exists "$output_directory\App_Start\test.temp"
	Ensure-Directory-Exists "$output_directory\DI\test.temp"

	#copy configuration files
	Copy-Item $source_directory\codeasconfiguration\shared\App_Start\* $output_directory\App_Start -Recurse
	Copy-Item $source_directory\codeasconfiguration\shared\DI\* $output_directory\DI -Recurse	
	Copy-Item $source_directory\codeasconfiguration\$di_container\App_Start\* $output_directory\App_Start -Recurse
	Copy-Item $source_directory\codeasconfiguration\$di_container\DI\* $output_directory\DI -Recurse

	#pre-process the compiler symbols in the configuration files
	Preprocess-Code-Files $output_directory $net_version $mvc_version
}

function Create-Configuration-Nuspec-File ([string] $di_container, [string] $mvc_version) {
	$nuspec_shared = "$nuget_directory\mvcsitemapprovider.configuration\mvcsitemapprovider.configuration.shared.nuspec"
	$output_file = "$build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container.nuspec"
	Ensure-Directory-Exists $output_file
	Transform-Nuspec $nuspec_shared "$nuget_directory\mvcsitemapprovider.configuration\mvcsitemapprovider.configuration.$di_container.nutrans" "$output_file.template"
	
	#replace the tokens
	(cat "$output_file.template") `
		-replace '#di_container_name#', "$di_container" `
		-replace '#mvc_version#', "$mvc_version" `
		> $output_file 

	#delete the template file
	Remove-Item "$output_file.template" -Force -ErrorAction SilentlyContinue
}