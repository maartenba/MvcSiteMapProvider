param($rootPath, $toolsPath, $package, $project)

function CountSolutionFilesByExtension($extension) {
	$path = [System.IO.Path]::GetDirectoryName($project.FullName)
	$totalfiles = [System.IO.Directory]::EnumerateFiles("$path", "*." + $extension, [System.IO.SearchOption]::AllDirectories)
	[int]$totalcount = ($totalfiles | Measure-Object).Count
	
	[int]$count = $totalcount
	# Don't count the DisplayTemplates directory - need to subtract them.
	if (($extension -eq "cshtml") -or ($extension -eq "vbhtml")) {
		$razorfiles = [System.IO.Directory]::EnumerateFiles("$path\Views\Shared\DisplayTemplates", "*." + $extension)
		[int]$razorcount = ($razorfiles | Measure-Object).Count
		[int]$count = $totalcount - $razorcount
	}
	
	Write-Host "Project has $count $extension extensions"
	return $count
}

### Copied from MvcScaffolding
function InferPreferredViewEngine() {
	# Assume you want Razor except if you already have some ASPX views and no Razor ones
	Write-Host "Checking for .aspx extensions"
	if ((CountSolutionFilesByExtension "aspx") -eq 0) { return "razor" }
	Write-Host "Checking for razor extensions"
	if (((CountSolutionFilesByExtension "cshtml") -gt 0) -or ((CountSolutionFilesByExtension vbhtml) -gt 0)) { return "razor" }
	Write-Host "No razor found, using aspx"
	return "aspx"
}

# Infer which view engine you're using based on the files in your project
### End copied

function Install-MVC4-Config-Sections() {
	$xml = New-Object xml

	# find the Web.config file
	$config = $project.ProjectItems | where {$_.Name -eq "Web.config"}

	# find its path on the file system
	$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

	# load Web.config as XML
	$xml.Load($localPath.Value)

	# select the node
	$ws = $xml.SelectSingleNode("configuration/system.webServer")
	if ($ws -eq $null) {
		$ws = $xml.CreateElement("system.webServer")
		$xml.AppendChild($ws)
	}
	
	$modules = $xml.SelectSingleNode("configuration/system.webServer/modules")
	if ($modules -eq $null) {
		$modules = $xml.CreateElement("modules")
		$ws.AppendChild($modules)
	}
	
	$remove = $xml.SelectSingleNode("configuration/system.webServer/modules/remove[@name='UrlRoutingModule-4.0']")
	if ($remove -eq $null) {
		$remove = $xml.CreateElement("remove")
		
		$name = $xml.CreateAttribute("name")
		$name.Value = "UrlRoutingModule-4.0"
		$remove.Attributes.Append($name)
		
		$modules.AppendChild($remove)
	}
	
	$add = $xml.SelectSingleNode("configuration/system.webServer/modules/add[@name='UrlRoutingModule-4.0']")
	if ($add -eq $null) {
		$add = $xml.CreateElement("add")
		
		$name = $xml.CreateAttribute("name")
		$name.Value = "UrlRoutingModule-4.0"
		$add.Attributes.Append($name)
		
		$type = $xml.CreateAttribute("type")
		$type.Value = "System.Web.Routing.UrlRoutingModule"
		$add.Attributes.Append($type)
		
		$modules.AppendChild($add)
	}
	
	# save the Web.config file
	$xml.Save($localPath.Value)
}



if ([string](InferPreferredViewEngine) -eq 'aspx') { 
	(Get-Project).ProjectItems | ?{ $_.Name -eq "Views" } | %{ $_.ProjectItems | ?{ $_.Name -eq "Shared" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "DisplayTemplates" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "MenuHelperModel.cshtml" -or  $_.Name -eq "SiteMapHelperModel.cshtml" -or  $_.Name -eq "SiteMapNodeModel.cshtml" -or  $_.Name -eq "SiteMapNodeModelList.cshtml" -or  $_.Name -eq "SiteMapPathHelperModel.cshtml" -or  $_.Name -eq "SiteMapTitleHelperModel.cshtml" -or  $_.Name -eq "CanonicalHelperModel.cshtml" -or  $_.Name -eq "MetaRobotsHelperModel.cshtml" } } | %{ $_.Delete() }
} else {
	(Get-Project).ProjectItems | ?{ $_.Name -eq "Views" } | %{ $_.ProjectItems | ?{ $_.Name -eq "Shared" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "DisplayTemplates" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "MenuHelperModel.ascx" -or  $_.Name -eq "SiteMapHelperModel.ascx" -or  $_.Name -eq "SiteMapNodeModel.ascx" -or  $_.Name -eq "SiteMapNodeModelList.ascx" -or  $_.Name -eq "SiteMapPathHelperModel.ascx" -or  $_.Name -eq "SiteMapTitleHelperModel.ascx" -or  $_.Name -eq "CanonicalHelperModel.ascx" -or  $_.Name -eq "MetaRobotsHelperModel.ascx" } } | %{ $_.Delete() }
}

# If MVC 4, install web.config section to fix 404 not found on sitemap.xml (#124)

if ($project.Object.References.Find("System.Web.Mvc").Version -eq "4.0.0.0")
{
	Write-Output "Detected MVC 4"
	
	Install-MVC4-Config-Sections
}