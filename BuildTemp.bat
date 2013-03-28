set nuget=.\src\MvcSiteMapProvider\.nuget\NuGet.exe

mkdir packages
%nuget% pack .\src\MvcSiteMapProvider\MvcSiteMapProvider\MvcSiteMapProvider.csproj -NonInteractive -Build -Symbols -OutputDirectory .\packages