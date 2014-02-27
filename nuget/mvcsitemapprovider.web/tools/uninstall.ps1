# uninstall.ps1
param($installPath, $toolsPath, $package, $project)

function Remove-AppSettings() {
	$xml = New-Object xml

	# find the Web.config file
	$config = $project.ProjectItems | where {$_.Name -eq "Web.config"}

	# find its path on the file system
	$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

	# load Web.config as XML
	$xml.Load($localPath.Value)

	# remove MvcSiteMapProvider_UseExternalDIContainer
	$ext_di = $xml.SelectSingleNode("configuration/appSettings/add[@key='MvcSiteMapProvider_UseExternalDIContainer']")
	if ($ext_di -ne $null) {
		$ext_di.ParentNode.RemoveChild($ext_di)
	}
	
	# remove MvcSiteMapProvider_ScanAssembliesForSiteMapNodes
	$scan = $xml.SelectSingleNode("configuration/appSettings/add[@key='MvcSiteMapProvider_ScanAssembliesForSiteMapNodes']")
	if ($scan -ne $null) {
		$scan.ParentNode.RemoveChild($scan)
	}
	
	$appSettings = $xml.SelectSingleNode("configuration/appSettings")
	if ($appSettings -ne $null) {
		if (($appSettings.HasChildNodes -eq $false) -and ($appSettings.Attributes.Count -eq 0)) {
			$appSettings.ParentNode.RemoveChild($appSettings)
		}
	}
	
	# save the Web.config file
	$xml.Save($localPath.Value)
}

function Remove-Pages-Namespaces() {
	$xml = New-Object xml

	# find the Web.config file
	$config = $project.ProjectItems | where {$_.Name -eq "Web.config"}

	# find its path on the file system
	$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

	# load Web.config as XML
	$xml.Load($localPath.Value)

	# remove MvcSiteMapProvider.Web.Html if it exists
	$html = $xml.SelectSingleNode("configuration/system.web/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html']")
	if ($html -ne $null) {
		$html.ParentNode.RemoveChild($html)
	}
	
	# remove MvcSiteMapProvider.Web.Html.Models if it exists
	$html_models = $xml.SelectSingleNode("configuration/system.web/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html.Models']")
	if ($html_models -ne $null) {
		$html_models.ParentNode.RemoveChild($html_models)
	}
	
	$namespaces = $xml.SelectSingleNode("configuration/system.web/pages/namespaces")
	if ($namespaces -ne $null) {
		if (($namespaces.HasChildNodes -eq $false) -and ($namespaces.Attributes.Count -eq 0)) {
			$namespaces.ParentNode.RemoveChild($namespaces)
		}
	}
	
	$pages = $xml.SelectSingleNode("configuration/system.web/pages")
	if ($pages -ne $null) {
		if (($pages.HasChildNodes -eq $false) -and ($pages.Attributes.Count -eq 0)) {
			$pages.ParentNode.RemoveChild($pages)
		}
	}
	
	$system_web = $xml.SelectSingleNode("configuration/system.web")
	if ($system_web -ne $null) {
		if (($system_web.HasChildNodes -eq $false) -and ($system_web.Attributes.Count -eq 0)) {
			$system_web.ParentNode.RemoveChild($system_web)
		}
	}
	
	# save the Web.config file
	$xml.Save($localPath.Value)
}

function Remove-Razor-Pages-Namespaces() {
	$xml = New-Object xml

	$path = [System.IO.Path]::GetDirectoryName($project.FullName)
	$web_config_file = "$path\Views\Web.config"

	# load Web.config as XML
	$xml.Load($web_config_file)
	
	# remove MvcSiteMapProvider.Web.Html if it exists
	$html = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html']")
	if ($html -ne $null) {
		$html.ParentNode.RemoveChild($html)
	}
	
	# remove MvcSiteMapProvider.Web.Html.Models if it exists
	$html_models = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html.Models']")
	if ($html_models -ne $null) {
		$html_models.ParentNode.RemoveChild($html_models)
	}
	
	$namespaces = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages/namespaces")
	if ($namespaces -ne $null) {
		if (($namespaces.HasChildNodes -eq $false) -and ($namespaces.Attributes.Count -eq 0)) {
			$namespaces.ParentNode.RemoveChild($namespaces)
		}
	}
	
	$pages = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages")
	if ($pages -ne $null) {
		if (($pages.HasChildNodes -eq $false) -and ($pages.Attributes.Count -eq 0)) {
			$pages.ParentNode.RemoveChild($pages)
		}
	}

	$system_web_webpages_razor = $xml.SelectSingleNode("configuration/system.web.webPages.razor")
	if ($system_web_webpages_razor -ne $null) {
		if (($system_web_webpages_razor.HasChildNodes -eq $false) -and ($system_web_webpages_razor.Attributes.Count -eq 0)) {
			$system_web_webpages_razor.ParentNode.RemoveChild($system_web_webpages_razor)
		}
	}
	
	# save the Web.config file
	$xml.Save($web_config_file)
}

function Update-SiteMap-Element() {
	$xml = New-Object xml

	# find the Web.config file
	$config = $project.ProjectItems | where {$_.Name -eq "Web.config"}

	# find its path on the file system
	$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

	# load Web.config as XML
	$xml.Load($localPath.Value)

	$siteMap = $xml.SelectSingleNode("configuration/system.web/siteMap")
	if ($siteMap -ne $null) {
		if ($xml.SelectSingleNode("configuration/system.web/siteMap[@enabled]") -ne $null) {
			$siteMap.SetAttribute("enabled", "true")
		} else {
			$enabled = $xml.CreateAttribute("enabled")
			$enabled.Value = "true"
			$siteMap.Attributes.Append($enabled)
		}
	}
	
	# save the Web.config file
	$xml.Save($localPath.Value)
}

function Remove-MVC4-Config-Sections() {
	$xml = New-Object xml

	# find the Web.config file
	$config = $project.ProjectItems | where {$_.Name -eq "Web.config"}

	# find its path on the file system
	$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

	# load Web.config as XML
	$xml.Load($localPath.Value)
	
	$add = $xml.SelectSingleNode("configuration/system.webServer/modules/add[@name='UrlRoutingModule-4.0']")
	if ($add -ne $null) {
		$add.ParentNode.RemoveChild($add)
	}
	
	$remove = $xml.SelectSingleNode("configuration/system.webServer/modules/remove[@name='UrlRoutingModule-4.0']")
	if ($remove -ne $null) {
		$remove.ParentNode.RemoveChild($remove)
	}
	
	$modules = $xml.SelectSingleNode("configuration/system.webServer/modules")
	if ($modules -ne $null) {
		if (($modules.HasChildNodes -eq $false) -and ($modules.Attributes.Count -eq 0)) {
			$modules.ParentNode.RemoveChild($modules)
		}
	}
	
	$ws = $xml.SelectSingleNode("configuration/system.webServer")
	if ($ws -ne $null) {
		if (($ws.HasChildNodes -eq $false) -and ($ws.Attributes.Count -eq 0)) {
			$ws.ParentNode.RemoveChild($ws)
		}
	}
	
	# save the Web.config file
	$xml.Save($localPath.Value)
}

# If MVC 4 or higher, remove web.config section to fix 404 not found on sitemap.xml (#124)
$mvc_version = $project.Object.References.Find("System.Web.Mvc").Version
Write-Host "MVC Version: $mvc_version"
if ($mvc_version -notmatch '^[123]\.' -or [string]::IsNullOrEmpty($mvc_version))
{
	Write-Host "Removing config sections for MVC >= 4"
	Remove-MVC4-Config-Sections
}

# Undo the changes made to the config file
Remove-AppSettings
Remove-Pages-Namespaces
Remove-Razor-Pages-Namespaces
Update-SiteMap-Element




