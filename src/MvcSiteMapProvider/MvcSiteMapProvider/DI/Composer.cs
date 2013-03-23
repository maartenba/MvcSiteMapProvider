using System;
using System.Web.Mvc;
using System.Web.Hosting;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// This is the default dependency injection composer for the MvcSiteMapProvider that is fired automatically
    /// using a <see cref="WebActivatorEx.PostApplicationStartMethodAttribute"/> during application startup.
    /// </summary>
    public class Composer
    {
        public static void Compose()
        {
            // Get the configuration settings
            var settings = new ConfigurationSettings();

            // If the config specifies to use an external DI container, skip the internal container.
            if (!settings.UseExternalDIContainer)
            {
                // Validate the Xml File.
                var validator = new SiteMapXmlValidator();
                validator.ValidateXml(HostingEnvironment.MapPath(settings.SiteMapFileName));

                // Setup the Controller Factory with a decorator that can resolve the internal controllers
                var currentFactory = ControllerBuilder.Current.GetControllerFactory();
                ControllerBuilder.Current.SetControllerFactory(
                    new ControllerFactoryDecorator(currentFactory, settings));

                // Set the static loader instance
                var siteMapLoaderContainer = new SiteMapLoaderContainer(settings);
                SiteMaps.Loader = siteMapLoaderContainer.ResolveSiteMapLoader();
            }
        }
    }
}
