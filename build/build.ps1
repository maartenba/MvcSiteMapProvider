properties {
    $base_directory   = resolve-path "..\."
    $build_directory  = "$base_directory\release"
    $source_directory = "$base_directory\src\MvcSiteMapProvider"
    $nuget_directory  = "$base_directory\nuget"
    $tools_directory  = "$base_directory\tools"
    $version          = "4.0.0"
    $packageVersion   = "$version-alpha-01"
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
			/property:outdir=$build_directory\lib\net35\ `
			/verbosity:quiet `
			/property:Configuration=Release `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:DefineConstants=`" MVC3`;NET35`" `
			/property:EnableNuGetPackageRestore=true
	}
		
	exec { 
		msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
			/property:outdir=$build_directory\lib\net40\ `
			/verbosity:quiet `
			/property:Configuration=Release `
			"/t:Clean;Rebuild" `
			/property:WarningLevel=3 `
			/property:TargetFrameworkVersion=v4.0 `
			/property:DefineConstants=`" MVC3`;NET40`" `
			/property:EnableNuGetPackageRestore=true
	}
}

task NuGet -depends Compile -description "This tasks makes creates the NuGet packages" {  
    Generate-Nuspec-File `
		-file "$build_directory\mvcsitemapprovider.nuspec" `
		-version $version

	Copy-Item $nuget_directory\* $build_directory -Recurse
    Copy-Item $source_directory\MvcSiteMapProvider\Xml\MvcSiteMapSchema.xsd $build_directory\content\MvcSiteMapSchema.xsd

    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $build_directory\mvcsitemapprovider.nuspec -Symbols -Version $packageVersion
    }

    Move-Item *.nupkg $base_directory\release
}

task Finalize -depends NuGet -description "This tasks finalizes the build" {  

}