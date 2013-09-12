# install.ps1
param($rootPath, $toolsPath, $package, $project)

#Delete the dummy readme file
(Get-Project).ProjectItems | ?{ $_.Name -eq "MvcSiteMapProvider_Temp_ReadMe.txt" } | %{ $_.Delete() }

$DTE.ItemOperations.Navigate("https://github.com/maartenba/MvcSiteMapProvider/wiki/Getting-Started", $DTE.vsNavigateOptions.vsNavigateOptionsNewWindow)
