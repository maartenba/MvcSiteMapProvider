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
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc2\lib\net35\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:DefineConstants=`" MVC2`;NET35`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc2\lib\net35\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }
	
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc2\lib\net40\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC2`;NET40`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc2\lib\net40\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }

	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc2\lib\net45\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC2`;NET45`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc2\lib\net45\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }

	# MVC 3
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc3\lib\net35\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:DefineConstants=`" MVC3`;NET35`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc3\lib\net35\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }
	
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc3\lib\net40\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC3`;NET40`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc3\lib\net40\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }

	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc3\lib\net45\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC3`;NET45`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc3\lib\net45\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }

	# MVC 4
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc4\lib\net40\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC4`;NET40`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc4\lib\net40\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }

	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider.mvc4\lib\net45\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC4`;NET45`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider.mvc4\lib\net45\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }

	    
}

task NuGet -depends Compile -description "This tasks makes creates the NuGet packages" {
	# MVC 2
	Transform-Nuspec $nuget_directory\mvcsitemapprovider.shared.nuspec $nuget_directory\mvcsitemapprovider.mvc2.nutrans $build_directory\mvcsitemapprovider.mvc2\mvcsitemapprovider.nuspec
	Copy-Item $nuget_directory\mvcsitemapprovider\* $build_directory\mvcsitemapprovider.mvc2 -Recurse
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc2\mvcsitemapprovider.nuspec -Symbols -Version $packageVersion
    }

	# MVC 3
	Transform-Nuspec $nuget_directory\mvcsitemapprovider.shared.nuspec $nuget_directory\mvcsitemapprovider.mvc3.nutrans $build_directory\mvcsitemapprovider.mvc3\mvcsitemapprovider.nuspec
	Copy-Item $nuget_directory\mvcsitemapprovider\* $build_directory\mvcsitemapprovider.mvc3 -Recurse
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc3\mvcsitemapprovider.nuspec -Symbols -Version $packageVersion
    }

	# MVC 4
	Transform-Nuspec $nuget_directory\mvcsitemapprovider.shared.nuspec $nuget_directory\mvcsitemapprovider.mvc4.nutrans $build_directory\mvcsitemapprovider.mvc4\mvcsitemapprovider.nuspec
	Copy-Item $nuget_directory\mvcsitemapprovider\* $build_directory\mvcsitemapprovider.mvc4 -Recurse
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc4\mvcsitemapprovider.nuspec -Symbols -Version $packageVersion
    }
		
    Generate-Nuspec-File `
		-id "MvcSiteMapProvider.Web" `
		-file "$build_directory\mvcsitemapprovider.web\mvcsitemapprovider.web.nuspec" `
		-version $packageVersion `
		-dependencies @("MvcSiteMapProvider `" version=`"4.0", "WebActivatorEx `" version=`"2.0")
	
    Copy-Item $nuget_directory\mvcsitemapprovider.web\* $build_directory\mvcsitemapprovider.web -Recurse
    Copy-Item $source_directory\MvcSiteMapProvider\Xml\MvcSiteMapSchema.xsd $build_directory\mvcsitemapprovider.web\content\MvcSiteMapSchema.xsd
	mkdir $build_directory\mvcsitemapprovider.web\content\Views\Shared\DisplayTemplates
    Copy-Item $source_directory\MvcSiteMapProvider\Web\Html\DisplayTemplates\* $build_directory\mvcsitemapprovider.web\content\Views\Shared\DisplayTemplates -Recurse
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.web\mvcsitemapprovider.web.nuspec -Symbols -Version $packageVersion
    }

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
	Write-Host $di_container -ForegroundColor Yellow

	#create the build for each version of the framework
	foreach ($net_version in $net_versions) {
		Create-Configuration-Build $di_container $net_version $mvc_version
	}

	#package the build
	exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container.nuspec -Symbols -Version $packageVersion
    }
}


function Create-Configuration-Build ([string] $di_container, [string] $net_version, [string] $mvc_version) {

	Write-Host "Creating configuration build for $di_container, $net_version, MVC$mvc_version" -ForegroundColor Blue

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
	
	#create output directores
	Ensure-Directory-Exists "$build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version\App_Start\test.tmp"
	Ensure-Directory-Exists "$build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version\DI\test.tmp"

	#copy configuration files
	Copy-Item $source_directory\codeasconfiguration\shared\App_Start\* $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version\App_Start -Recurse
	Copy-Item $source_directory\codeasconfiguration\shared\DI\* $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version\DI -Recurse	
	Copy-Item $source_directory\codeasconfiguration\$di_container\App_Start\* $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version\App_Start -Recurse
	Copy-Item $source_directory\codeasconfiguration\$di_container\DI\* $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version\DI -Recurse

	#pre-process the compiler symbols in the configuration files
	Preprocess-Code-Files $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version $net_version $mvc_version

	#copy readme file
	Copy-Item $source_directory\codeasconfiguration\mvcsitemapprovider_configuration_readme.txt $build_directory\mvcsitemapprovider.mvc$mvc_version.configuration.$di_container\content\$net_version\MvcSiteMapProvider_Configuration_ReadMe.txt
}