Integrating MvcSiteMapProvider with StructureMap

To add MvcSiteMapProvider to your DI configuration,
simply add the following code to your composition root.

	// Create the DI container
	var container = new StructureMapContainer();

	// Setup configuration of DI
	container.Configure(r => r.AddRegistry<MvcSiteMapProviderRegistry>());

For more help consult the StructureMap documantation at
http://docs.structuremap.net/RegistryDSL.htm