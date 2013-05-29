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
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider\lib\net35\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:DefineConstants=`" MVC3`;NET35`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider\lib\net35\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }
	
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\mvcsitemapprovider\lib\net40\ `
			/verbosity:quiet `
			/property:Configuration=$configuration `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC3`;NET40`" `
			/property:EnableNuGetPackageRestore=true
	}
	dir $build_directory\mvcsitemapprovider\lib\net40\ | ?{ -not($_.Name -match 'MvcSiteMapProvider') } | %{ del $_.FullName }
}

task NuGet -depends Compile -description "This tasks makes creates the NuGet packages" {  
	Copy-Item $source_directory\mvcsitemapprovider\mvcsitemapprovider.nuspec $build_directory\mvcsitemapprovider\mvcsitemapprovider.nuspec
	Copy-Item $nuget_directory\mvcsitemapprovider\* $build_directory\mvcsitemapprovider -Recurse
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider\mvcsitemapprovider.nuspec -Symbols -Version $packageVersion
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

    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $nuget_directory\mvcsitemapprovider.bootstrapper.unity\mvcsitemapprovider.bootstrapper.unity.nuspec -Symbols -Version $packageVersion
    }
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $nuget_directory\mvcsitemapprovider.bootstrapper.structuremap\mvcsitemapprovider.bootstrapper.structuremap.nuspec -Symbols -Version $packageVersion
    }

    Move-Item *.nupkg $base_directory\release
}

task Finalize -depends NuGet -description "This tasks finalizes the build" {  

}