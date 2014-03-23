# install.ps1
param($rootPath, $toolsPath, $package, $project)

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

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

function Add-Or-Update-AppSettings() {
	$xml = New-Object xml

	$web_config_path = Get-Web-Config-Path
	$xml.Load($web_config_path)

	$conf = $xml.SelectSingleNode("configuration")
	if ($conf -eq $null)
	{
		$conf = $xml.CreateElement("configuration")
		$xml.AppendChild($conf)
	}
	
	$appSettings = $xml.SelectSingleNode("configuration/appSettings")
	if ($appSettings -eq $null) {
		$appSettings = $xml.CreateElement("appSettings")
		$conf.AppendChild($appSettings)
	}
	
	# add or update MvcSiteMapProvider_UseExternalDIContainer
	$ext_di = $xml.SelectSingleNode("configuration/appSettings/add[@key='MvcSiteMapProvider_UseExternalDIContainer']")
	if ($ext_di -ne $null) {
		$ext_di.SetAttribute("value", "false")
	} else {
		$ext_di = $xml.CreateElement("add")
		
		$key = $xml.CreateAttribute("key")
		$key.Value = "MvcSiteMapProvider_UseExternalDIContainer"
		$ext_di.Attributes.Append($key)
		
		$value = $xml.CreateAttribute("value")
		$value.Value = "false"
		$ext_di.Attributes.Append($value)
		
		$appSettings.AppendChild($ext_di)
	}
	
	# add or update MvcSiteMapProvider_ScanAssembliesForSiteMapNodes
	$scan = $xml.SelectSingleNode("configuration/appSettings/add[@key='MvcSiteMapProvider_ScanAssembliesForSiteMapNodes']")
	if ($scan -ne $null) {
		$scan.SetAttribute("value", "true")
	} else {
		$scan = $xml.CreateElement("add")
		
		$key = $xml.CreateAttribute("key")
		$key.Value = "MvcSiteMapProvider_ScanAssembliesForSiteMapNodes"
		$scan.Attributes.Append($key)
		
		$value = $xml.CreateAttribute("value")
		$value.Value = "true"
		$scan.Attributes.Append($value)
		
		$appSettings.AppendChild($scan)
	}
	
	# add MvcSiteMapProvider_IncludeAssembliesForScan (never update)
	$include_scan = $xml.SelectSingleNode("configuration/appSettings/add[@key='MvcSiteMapProvider_IncludeAssembliesForScan']")
	if ($include_scan -eq $null) {
		$assembly_name = Get-Project-Property-Value "AssemblyName"
		$include_scan = $xml.CreateElement("add")
		
		$key = $xml.CreateAttribute("key")
		$key.Value = "MvcSiteMapProvider_IncludeAssembliesForScan"
		$include_scan.Attributes.Append($key)
		
		$value = $xml.CreateAttribute("value")
		$value.Value = "$assembly_name"
		$include_scan.Attributes.Append($value)
		
		$appSettings.AppendChild($include_scan)
	}
	
	Save-Document-With-Formatting $xml $web_config_path
}

function Add-Pages-Namespaces() {
	$xml = New-Object xml
	
	$web_config_path = Get-Web-Config-Path
	$xml.Load($web_config_path)

	$conf = $xml.SelectSingleNode("configuration")
	if ($conf -eq $null)
	{
		$conf = $xml.CreateElement("configuration")
		$xml.AppendChild($conf)
	}
	
	$system_web = $xml.SelectSingleNode("configuration/system.web")
	if ($system_web -eq $null) {
		$system_web = $xml.CreateElement("system.web")
		$conf.AppendChild($system_web)
	}
	
	$pages = $xml.SelectSingleNode("configuration/system.web/pages")
	if ($pages -eq $null) {
		$pages = $xml.CreateElement("pages")
		$system_web.AppendChild($pages)
	}
	
	$namespaces = $xml.SelectSingleNode("configuration/system.web/pages/namespaces")
	if ($namespaces -eq $null) {
		$namespaces = $xml.CreateElement("namespaces")
		$pages.AppendChild($namespaces)
	}
	
	# add MvcSiteMapProvider.Web.Html if it doesn't already exist
	$html = $xml.SelectSingleNode("configuration/system.web/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html']")
	if ($html -eq $null) {
		$html = $xml.CreateElement("add")
		
		$namespace_html = $xml.CreateAttribute("namespace")
		$namespace_html.Value = "MvcSiteMapProvider.Web.Html"
		$html.Attributes.Append($namespace_html)
		
		$namespaces.AppendChild($html)
	}
	
	# add MvcSiteMapProvider.Web.Html.Models if it doesn't already exist
	$html_models = $xml.SelectSingleNode("configuration/system.web/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html.Models']")
	if ($html_models -eq $null) {
		$html_models = $xml.CreateElement("add")
		
		$namespace_models = $xml.CreateAttribute("namespace")
		$namespace_models.Value = "MvcSiteMapProvider.Web.Html.Models"
		$html_models.Attributes.Append($namespace_models)
		
		$namespaces.AppendChild($html_models)
	}
	
	Save-Document-With-Formatting $xml $web_config_path
}

function Add-Razor-Pages-Namespaces() {
	$xml = New-Object xml

	$path = [System.IO.Path]::GetDirectoryName($project.FullName)
	$web_config_path = "$path\Views\Web.config"

	# load Web.config as XML
	$xml.Load($web_config_path)

	$conf = $xml.SelectSingleNode("configuration")
	if ($conf -eq $null)
	{
		$conf = $xml.CreateElement("configuration")
		$xml.AppendChild($conf)
	}
	
	$system_web_webpages_razor = $xml.SelectSingleNode("configuration/system.web.webPages.razor")
	if ($system_web_webpages_razor -eq $null) {
		$system_web_webpages_razor = $xml.CreateElement("system.web.webPages.razor")
		$conf.AppendChild($system_web_webpages_razor)
	}
	
	$pages = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages")
	if ($pages -eq $null) {
		$pages = $xml.CreateElement("pages")
		
		$page_base_type = $xml.CreateAttribute("pageBaseType")
		$page_base_type.Value = "System.Web.Mvc.WebViewPage"
		$pages.Attributes.Append($page_base_type)
		
		$system_web_webpages_razor.AppendChild($pages)
	}
	
	$namespaces = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages/namespaces")
	if ($namespaces -eq $null) {
		$namespaces = $xml.CreateElement("namespaces")
		$pages.AppendChild($namespaces)
	}
	
	# add MvcSiteMapProvider.Web.Html if it doesn't already exist
	$html = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html']")
	if ($html -eq $null) {
		$html = $xml.CreateElement("add")
		
		$namespace_html = $xml.CreateAttribute("namespace")
		$namespace_html.Value = "MvcSiteMapProvider.Web.Html"
		$html.Attributes.Append($namespace_html)
		
		$namespaces.AppendChild($html)
	}
	
	# add MvcSiteMapProvider.Web.Html.Models if it doesn't already exist
	$html_models = $xml.SelectSingleNode("configuration/system.web.webPages.razor/pages/namespaces/add[@namespace='MvcSiteMapProvider.Web.Html.Models']")
	if ($html_models -eq $null) {
		$html_models = $xml.CreateElement("add")
		
		$namespace_models = $xml.CreateAttribute("namespace")
		$namespace_models.Value = "MvcSiteMapProvider.Web.Html.Models"
		$html_models.Attributes.Append($namespace_models)
		
		$namespaces.AppendChild($html_models)
	}
	
	Save-Document-With-Formatting $xml $web_config_path
}

function Update-SiteMap-Element() {
	$xml = New-Object xml

	$web_config_path = Get-Web-Config-Path
	$xml.Load($web_config_path)

	$siteMap = $xml.SelectSingleNode("configuration/system.web/siteMap")
	if ($siteMap -ne $null) {
		if ($xml.SelectSingleNode("configuration/system.web/siteMap[@enabled]") -ne $null) {
			$siteMap.SetAttribute("enabled", "false")
		} else {
			$enabled = $xml.CreateAttribute("enabled")
			$enabled.Value = "false"
			$siteMap.Attributes.Append($enabled)
		}
	}
	
	Save-Document-With-Formatting $xml $web_config_path
}

function Add-MVC4-Config-Sections() {
	$xml = New-Object xml
	
	$web_config_path = Get-Web-Config-Path
	$xml.Load($web_config_path)
	
	$conf = $xml.SelectSingleNode("configuration")
	if ($conf -eq $null)
	{
		$conf = $xml.CreateElement("configuration")
		$xml.AppendChild($conf)
	}
	
	$ws = $xml.SelectSingleNode("configuration/system.webServer")
	if ($ws -eq $null) {
		$ws = $xml.CreateElement("system.webServer")
		$conf.AppendChild($ws)
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
	
	Save-Document-With-Formatting $xml $web_config_path
}

#Gets the encoding from an open xml document as a System.Text.Encoding type
function Get-Document-Encoding([xml] $xml) {
	[string] $encodingStr = ""
	if ($xml.FirstChild.NodeType -eq [System.Xml.XmlNodeType]::XmlDeclaration) {
		[System.Xml.XmlDeclaration] $declaration = $xml.FirstChild
		$encodingStr = $declaration.Encoding
	}
	if ([string]::IsNullOrEmpty($encodingStr) -eq $false) {
		$encoding = $null
		Try {
			$encoding = [System.Text.Encoding]::GetEncoding($encodingStr)
		}
		Catch [System.Exception] {
			$encoding = $null
		}
		return $encoding
	} else {
		return $null
	}
}

function Save-Document-With-Formatting([xml] $xml, [string] $path) {
	# save the xml file with formatting and original encoding
	$encoding = Get-Document-Encoding $xml
	$writer = New-Object System.Xml.XmlTextWriter -ArgumentList @($path, $encoding)
	$writer.Formatting = [System.Xml.Formatting]::Indented
	$xml.Save($writer)
	$writer.Close()
}

function Get-Web-Config-Path() {
	$path = [System.IO.Path]::GetDirectoryName($project.FullName)
	$web_config_path = "$path\Web.config"
	return $web_config_path
}

function Get-Project-Property-Value([string] $property_name) {
	$ms_build_project = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($Project.FullName) | Select-Object -First 1
	foreach ($property in $ms_build_project.Properties) {
		if ($property.Name -eq $property_name) {
			$value = $property.EvaluatedValue
			break
		}
	}
	return $value
}

# Infer which view engine you're using based on the files in your project
if ([string](InferPreferredViewEngine) -eq 'aspx') { 
	(Get-Project).ProjectItems | ?{ $_.Name -eq "Views" } | %{ $_.ProjectItems | ?{ $_.Name -eq "Shared" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "DisplayTemplates" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "MenuHelperModel.cshtml" -or  $_.Name -eq "SiteMapHelperModel.cshtml" -or  $_.Name -eq "SiteMapNodeModel.cshtml" -or  $_.Name -eq "SiteMapNodeModelList.cshtml" -or  $_.Name -eq "SiteMapPathHelperModel.cshtml" -or  $_.Name -eq "SiteMapTitleHelperModel.cshtml" -or  $_.Name -eq "CanonicalHelperModel.cshtml" -or  $_.Name -eq "MetaRobotsHelperModel.cshtml" } } | %{ $_.Delete() }
} else {
	(Get-Project).ProjectItems | ?{ $_.Name -eq "Views" } | %{ $_.ProjectItems | ?{ $_.Name -eq "Shared" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "DisplayTemplates" } } | %{ $_.ProjectItems | ?{ $_.Name -eq "MenuHelperModel.ascx" -or  $_.Name -eq "SiteMapHelperModel.ascx" -or  $_.Name -eq "SiteMapNodeModel.ascx" -or  $_.Name -eq "SiteMapNodeModelList.ascx" -or  $_.Name -eq "SiteMapPathHelperModel.ascx" -or  $_.Name -eq "SiteMapTitleHelperModel.ascx" -or  $_.Name -eq "CanonicalHelperModel.ascx" -or  $_.Name -eq "MetaRobotsHelperModel.ascx" } } | %{ $_.Delete() }
}

# If MVC 4 or higher, install web.config section to fix 404 not found on sitemap.xml (#124)
$mvc_version = $project.Object.References.Find("System.Web.Mvc").Version
Write-Host "MVC Version: $mvc_version"
if ($mvc_version -notmatch '^[123]\.' -or [string]::IsNullOrEmpty($mvc_version))
{
	Write-Host "Installing config sections for MVC >= 4"
	Add-MVC4-Config-Sections
}

# Fixup the web.config files
Add-Or-Update-AppSettings
Add-Pages-Namespaces
Add-Razor-Pages-Namespaces
Update-SiteMap-Element
