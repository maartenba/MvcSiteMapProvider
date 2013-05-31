param($rootPath, $toolsPath, $package, $project)

function CountSolutionFilesByExtension($extension) {
	$files = (Get-Project).DTE.Solution `
		| ?{ $_.FileName } `
		| %{ [System.IO.Path]::GetDirectoryName($_.FileName) } `
		| %{ [System.IO.Directory]::EnumerateFiles($_, "*." + $extension, [System.IO.SearchOption]::AllDirectories) }
	($files | Measure-Object).Count
}

function InferPreferredViewEngine() {
	# Assume you want Razor except if you already have some ASPX views and no Razor ones
	if ((CountSolutionFilesByExtension("aspx")) -eq 0) { return "razor" }
	if (((CountSolutionFilesByExtension("cshtml")) -gt 0) -or ((CountSolutionFilesByExtension("vbhtml")) -gt 0)) { return "razor" }
	return "aspx"
}

if ([string](InferPreferredViewEngine) -eq "aspx") { 
	(Get-Project).ProjectItems | ?{ $_.Name -eq "Views" } | %{ $_.ProjectItems | ?{ $_.Name -eq "Shared" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "DisplayTemplates" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "MenuHelperModel.cshtml" -or  $_.Name -eq "SiteMapHelperModel.cshtml" -or  $_.Name -eq "SiteMapNodeModel.cshtml" -or  $_.Name -eq "SiteMapNodeModelList.cshtml" -or  $_.Name -eq "SiteMapPathHelperModel.cshtml" -or  $_.Name -eq "SiteMapTitleHelperModel.cshtml" -or  $_.Name -eq "CanonicalHelperModel.cshtml" -or  $_.Name -eq "MetaRobotsHelperModel.cshtml" } } | %{ $_.Delete() }
} else {
	(Get-Project).ProjectItems | ?{ $_.Name -eq "Views" } | %{ $_.ProjectItems | ?{ $_.Name -eq "Shared" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "DisplayTemplates" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "MenuHelperModel.ascx" -or  $_.Name -eq "SiteMapHelperModel.ascx" -or  $_.Name -eq "SiteMapNodeModel.ascx" -or  $_.Name -eq "SiteMapNodeModelList.ascx" -or  $_.Name -eq "SiteMapPathHelperModel.ascx" -or  $_.Name -eq "SiteMapTitleHelperModel.ascx" -or  $_.Name -eq "CanonicalHelperModel.ascx" -or  $_.Name -eq "MetaRobotsHelperModel.ascx" } } | %{ $_.Delete() }
}
 