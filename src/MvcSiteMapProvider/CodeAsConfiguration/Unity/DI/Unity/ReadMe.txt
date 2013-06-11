Integrating MvcSiteMapProvider with Unity

To add MvcSiteMapProvider to your DI configuration,
simply add the following code to your composition root.

	var container = new UnityContainer();
	container.AddNewExtension<MvcSiteMapProviderContainerExtension>();

For more help consult the Unity documantation at
http://msdn.microsoft.com/en-us/library/ff660845%28v=pandp.20%29.aspx