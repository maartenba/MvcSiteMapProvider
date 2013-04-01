properties {
    $base_directory   = resolve-path "..\."
    $build_directory  = "$base_directory\release"
    $source_directory = "$base_directory\src\MvcSiteMapProvider"
    $nuget_directory  = "$base_directory\nuget"
    $tools_directory  = "$base_directory\tools"
    $version          = "4.0.0"
    $packageVersion   = "$version-alpha-01"
	$builds = @(
		@{Name = "MvcSiteMapProvider.Mvc4"; Constants="MVC4"; NuGetDir = "net40"; TargetFramework="v4.0"},
		@{Name = "MvcSiteMapProvider.Mvc3"; Constants="MVC3"; NuGetDir = "net35"; TargetFramework="v3.5"}
		@{Name = "MvcSiteMapProvider.Mvc2"; Constants="MVC2"; NuGetDir = "net20"; TargetFramework="v3.5"}
	)
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
	foreach ($build in $builds)
	{
		$name = $build.Name
		$finalDir = $build.NuGetDir
		$constants = $build.Constants
		$targetFrameworkVersion = $build.TargetFramework

		exec { 
			msbuild $source_directory\MvcSiteMapProvider\MvcSiteMapProvider.csproj `
				/p:outdir=$build_directory\\$finalDir\\ `
				/verbosity:quiet `
				/p:Configuration=Release `
				"/t:Clean;Rebuild" `
				/p:WarningLevel=3 `
				/p:TargetFrameworkVersion=$targetFrameworkVersion `
				/p:DefineConstants=$constants `
				/p:EnableNuGetPackageRestore=true
		}
	}
}

task NuGet -depends Compile -description "This tasks makes creates the NuGet packages" {  
    Generate-Nuspec-File `
		-file "$nuget_directory\mvcsitemapprovider.nuspec" `
		-version $version

    Copy-Item $source_directory\MvcSiteMapProvider\MvcSiteMapSchema.xsd $nuget_directory\content
    Copy-Item $base_directory\release\MvcSiteMapProvider\* $nuget_directory\lib\net40 -Recurse

    exec { 
        &"$tools_directory\nuget\NuGet.exe" pack $nuget_directory\mvcsitemapprovider.nuspec -Symbols
    }

    Remove-Item $nuget_directory\lib\* -Recurse
    Move-Item *.nupkg $base_directory\release
}

task Finalize -depends Compile -description "This tasks finalizes the build" {  

}