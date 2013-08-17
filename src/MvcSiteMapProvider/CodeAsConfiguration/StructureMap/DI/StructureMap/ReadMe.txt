Integrating MvcSiteMapProvider with StructureMap

To add MvcSiteMapProvider to your DI configuration,
add the following code to your composition root.

	// Create the DI container (typically part of your DI setup already)
	var container = new StructureMapContainer();

	// Setup configuration of DI (required)
	container.Configure(r => r.AddRegistry<MvcSiteMapProviderRegistry>());

	// Setup global sitemap loader (required)
    MvcSiteMapProvider.SiteMaps.Loader = container.GetInstance<ISiteMapLoader>();

    // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
    var validator = container.GetInstance<ISiteMapXmlValidator>();
    validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

    // Register the Sitemaps routes for search engines (optional)
    XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

For more help consult the StructureMap documantation at
http://docs.structuremap.net/RegistryDSL.htm