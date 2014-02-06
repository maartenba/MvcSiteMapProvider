# install.ps1
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
	$node.SetAttribute("value", "true")
} else {
	# add a new node
	$node = $xml.SelectSingleNode("configuration/appSettings")
	
	if ($node -eq $null) {
		$node = $xml.CreateElement("appSettings")
		$xml.AppendChild($node)
	}
	
	$add = $xml.CreateElement("add")
	
	$key = $xml.CreateAttribute("key")
	$key.Value = "MvcSiteMapProvider_UseExternalDIContainer"
	$add.Attributes.Append($key)
	
	$value = $xml.CreateAttribute("value")
	$value.Value = "true"
	$add.Attributes.Append($value)
	
	$node.AppendChild($add)
}

# save the Web.config file
$xml.Save($localPath.Value)