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
		-file "$source_directory\MvcSiteMapProvider\Properties\AssemblyInfo.cs" `
		-title "MvcSiteMapProvider $version" `
		-description "An ASP.NET SiteMapProvider implementation for the ASP.NET MVC framework." `
		-company "MvcSiteMapProvider" `
		-product "MvcSiteMapProvider $version" `
		-version $version `
		-copyright "Copyright © Maarten Balliauw 2009 - 2013" `
		-clsCompliant "false"
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
}

task NuGet -depends Compile -description "This tasks makes creates the NuGet packages" {  
    Generate-Nuspec-File `
		-id "MvcSiteMapProvider" `
		-file "$build_directory\mvcsitemapprovider\mvcsitemapprovider.nuspec" `
		-version $packageVersion
		
	Copy-Item $nuget_directory\mvcsitemapprovider\* $build_directory\mvcsitemapprovider -Recurse
	
    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider\mvcsitemapprovider.nuspec -Symbols
    }
		
    Generate-Nuspec-File `
		-id "MvcSiteMapProvider.Web" `
		-file "$build_directory\mvcsitemapprovider.web\mvcsitemapprovider.web.nuspec" `
		-version $packageVersion `
		-dependencies @("MvcSiteMapProvider `" version=`"4.0", "WebActivatorEx `" version=`"2.0")

    Copy-Item $nuget_directory\mvcsitemapprovider.web\* $build_directory\mvcsitemapprovider.web -Recurse
    Copy-Item $source_directory\MvcSiteMapProvider\Xml\MvcSiteMapSchema.xsd $build_directory\mvcsitemapprovider.web\content\MvcSiteMapSchema.xsd

    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.web\mvcsitemapprovider.web.nuspec -Symbols
    }

    Move-Item *.nupkg $base_directory\release
}

task Finalize -depends NuGet -description "This tasks finalizes the build" {  

}