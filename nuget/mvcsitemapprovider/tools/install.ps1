# install.ps1
param($rootPath, $toolsPath, $package, $project)

#Delete the dummy readme file
(Get-Project).ProjectItems | ?{ $_.Name -eq "MvcSiteMapProvider_Temp_ReadMe.txt" } | %{ $_.Delete() }

$DTE.ItemOperations.Navigate("http://maartenba.github.io/MvcSiteMapProvider/getting-started.html", $DTE.vsNavigateOptions.vsNavigateOptionsNewWindow)
