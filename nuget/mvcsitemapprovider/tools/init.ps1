param($rootPath, $toolsPath, $package, $project)

function CountSolutionFilesByExtension($extension) {
	$files = (Get-Project).DTE.Solution `
		| ?{ $_.FileName } `
		| %{ [System.IO.Path]::GetDirectoryName($_.FileName) } `
		| %{ [System.IO.Directory]::EnumerateFiles($_, "*." + $extension, [System.IO.SearchOption]::AllDirectories) }
	($files | Measure-Object).Count
}

if (CountSolutionFilesByExtension(".cshtml") -gt 1 -or CountSolutionFilesByExtension(".aspx") -gt 1) {
	Install-Package MvcSiteMapProvider.Web -IncludePrerelease
}