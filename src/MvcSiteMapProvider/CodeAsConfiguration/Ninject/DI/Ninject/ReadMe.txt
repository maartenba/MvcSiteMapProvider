Integrating MvcSiteMapProvider with Ninject

To add MvcSiteMapProvider to your DI configuration,
add the following code to your composition root.

    // Create the DI container (typically part of your DI setup already)
    var container = new StandardKernel();

    // Setup configuration of DI (required)
    container.Load(new MvcSiteMapProviderModule());

	// Setup global sitemap loader (required)
    MvcSiteMapProvider.SiteMaps.Loader = container.Get<ISiteMapLoader>();

    // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
    var validator = container.Get<ISiteMapXmlValidator>();
    validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

    // Register the Sitemaps routes for search engines (optional)
    XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

For more help consult the Ninject documentation at
https://github.com/ninject/ninject/wiki/Modules-and-the-Kernel