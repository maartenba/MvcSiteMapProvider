# uninstall.ps1
param($installPath, $toolsPath, $package, $project)

$xml = New-Object xml

# find the Web.config file
$config = $project.ProjectItems | where {$_.Name -eq "Web.config"}

# find its path on the file system
$localPath = $config.Properties | where {$_.Name -eq "LocalPath"}

# load Web.config as XML
$xml.Load($localPath.Value)

# select the node
$node = $xml.SelectSingleNode("configuration/appSettings/add[@key='MvcSiteMapProvider_UseExternalDIContainer']")

if ($node -ne $null) {
	# change the value
	$node.SetAttribute("value", "false")

	# save the Web.config file
	$xml.Save($localPath.Value)
}