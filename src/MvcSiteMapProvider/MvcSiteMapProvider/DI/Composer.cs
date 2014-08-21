using System;
using System.Web.Mvc;
using System.Web.Hosting;
using System.Web.Routing;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Web.Mvc;
#if !MVC2
using System.Web.WebPages.Razor;
#endif

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// This is the default dependency injection composer for the MvcSiteMapProvider that is fired automatically
    /// using a WebActivatorEx.PostApplicationStartMethodAttribute during application startup.
    /// </summary>
    public class Composer
    {
        public static void Compose()
        {
#if !MVC2
            // Register global namespaces with Razor so we don't have to import them in Web.config
            WebPageRazorHost.AddGlobalImport("MvcSiteMapProvider.Web.Html");
            WebPageRazorHost.AddGlobalImport("MvcSiteMapProvider.Web.Html.Models");
#endif

            // Get the configuration settings
            var settings = new ConfigurationSettings();

            // If the config specifies to use an external DI container, skip the internal container.
            if (!settings.UseExternalDIContainer)
            {
                if (settings.EnableSiteMapFile)
                {
                    // Validate the Xml File.
                    var validator = new SiteMapXmlValidator();
                    validator.ValidateXml(HostingEnvironment.MapPath(settings.SiteMapFileName));
                }

#if !MVC2
                // If not using a custom DependencyResolver, we prefer to use IControllerFactory
                if (DependencyResolver.Current.GetType().FullName.Equals("System.Web.Mvc.DependencyResolver+DefaultDependencyResolver"))
                {
#endif
                    // Setup the Controller Factory with a decorator that can resolve the internal controllers
                    var currentFactory = ControllerBuilder.Current.GetControllerFactory();
                    ControllerBuilder.Current.SetControllerFactory(
                        new ControllerFactoryDecorator(currentFactory, settings));
#if !MVC2
                }
                else
                {
                    // If using a custom IDependencyResolver, decorate it with our IDependencyResolver so we can resolve
                    // our internal controller.
                    var currentResolver = DependencyResolver.Current;
                    DependencyResolver.SetResolver(new DependencyResolverDecorator(currentResolver, settings));
                }
#endif

                // Set the static loader instance
                var siteMapLoaderContainer = new SiteMapLoaderContainer(settings);
                SiteMaps.Loader = siteMapLoaderContainer.ResolveSiteMapLoader();

                if (settings.EnableSitemapsXml)
                {
                    // Register the route for SiteMaps XML
                    XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
                }
            }
        }
    }
}
