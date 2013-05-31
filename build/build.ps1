properties {
    $base_directory   = resolve-path "..\."
    $build_directory  = "$base_directory\release"
    $source_directory = "$base_directory\src\MvcSiteMapProvider"
    $nuget_directory  = "$base_directory\nuget"
    $tools_directory  = "$base_directory\tools"
    $version          = "3.3.6.0"
}

include .\psake_ext.ps1

task default -depends Finalize

task Clean -description "This task cleans up the build directory" {
    Remove-Item $build_directory\\$solution -Force -Recurse -ErrorAction SilentlyContinue
}

task Init -description "This tasks makes sure the build environment is correctly setup" {  
    Generate-Assembly-Info `
		-file "$source_directory\MvcSiteMapProvider\Properties\AssemblyInfo.cs" `
		-title "MvcSiteMapProvider $version" `
		-description "An ASP.NET SiteMapProvider implementation for the ASP.NET MVC framework." `
		-company "MvcSiteMapProvider" `
		-product "MvcSiteMapProvider $version" `
		-version $version `
		-copyright "Copyright (c) Maarten Balliauw 2009 - 2013" `
		-clsCompliant "false"
        
    if ((Test-Path $build_directory) -eq $false) {
        New-Item $build_directory\\$solution\net35 -ItemType Directory
        New-Item $build_directory\\$solution\net40 -ItemType Directory
    }
}

# task Compile35 -depends Clean, Init -description "This task compiles the solution" {
    # exec { 
        # msbuild $source_directory\$solution.sln `
            # /p:outdir=$build_directory\\$solution\\net35\\ `
            # /verbosity:quiet `
            # /p:Configuration=Release `
			# /t:Rebuild `
			# /property:WarningLevel=3 `
            # /property:TargetFrameworkVersion=v3.5 `
            # /property:DefineConstants=NET35
    # }
# }

task Compile -depends Clean, Init -description "This task compiles the solution" {
    exec { 
        msbuild $source_directory\$solution.sln `
            /p:outdir=$build_directory\\$solution\\net40\\ `
            /verbosity:quiet `
            /p:Configuration=Release `
            /t:Rebuild `
            /property:WarningLevel=3 `
            /property:TargetFrameworkVersion=v4.0 `
            /property:DefineConstants=`" MVC3`;NET40`" 
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

task Finalize -depends NuGet -description "This tasks finalizes the build" {  

}
