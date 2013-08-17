Integrating MvcSiteMapProvider with Autofac

To add MvcSiteMapProvider to your DI configuration,
add the following code to your composition root.

	// Create a container builder (typically part of your DI setup already)
	var builder = new ContainerBuilder();

	// Register modules
	builder.RegisterModule(new MvcSiteMapProviderModule()); // Required
	builder.RegisterModule(new MvcModule()); // Required by MVC. Typically already part of your setup (double check the contents of the module).

	// Create the DI container (typically part of your config already)
	var container = builder.Build();

	// Setup global sitemap loader (required)
    MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

    // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
    var validator = container.Resolve<ISiteMapXmlValidator>();
    validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

    // Register the Sitemaps routes for search engines (optional)
    XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

For more help consult the Autofac documentation at
http://code.google.com/p/autofac/wiki/StructuringWithModules