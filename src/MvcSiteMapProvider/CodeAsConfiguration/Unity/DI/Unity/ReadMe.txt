Integrating MvcSiteMapProvider with Unity

To add MvcSiteMapProvider to your DI configuration,
add the following code to your composition root.

	// Create the container (typically part of your DI setup already)
	var container = new UnityContainer();

	// Add the extension module (required)
	container.AddNewExtension<MvcSiteMapProviderContainerExtension>();

	// Setup global sitemap loader (required)
    MvcSiteMapProvider.SiteMaps.Loader = container.Resolve<ISiteMapLoader>();

    // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider (optional)
    var validator = container.Resolve<ISiteMapXmlValidator>();
    validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));

    // Register the Sitemaps routes for search engines (optional)
    XmlSiteMapController.RegisterRoutes(RouteTable.Routes);

For more help consult the Unity documantation at
http://msdn.microsoft.com/en-us/library/ff660845%28v=pandp.20%29.aspx