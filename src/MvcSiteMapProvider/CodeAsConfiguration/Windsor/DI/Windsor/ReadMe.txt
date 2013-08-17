Integrating MvcSiteMapProvider with Castle Windsor

To add MvcSiteMapProvider to your DI configuration,
add the following code to your composition root.

	// Create the DI container (typically part of your DI setup already)
    var container = new WindsorContainer();

    // Setup configuration of DI
    container.Install(new MvcSiteMapProviderInstaller()); // Required
    container.Install(new MvcInstaller()); // Required by MVC. Typically already part of your setup (double check the contents of the module).

	// Setup global sitemap loader (required)
    MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

    // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
    var validator = container.Resolve<ISiteMapXmlValidator>();
    validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

    // Register the Sitemaps routes for search engines (optional)
    XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

For more help consult the Castle Windsor documantation at
http://docs.castleproject.org/Default.aspx?Page=Installers&NS=Windsor&AspxAutoDetectCookieSupport=1