using System.Web.Hosting;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.DI.Bootstrap
{
    public class MvcSiteMapProviderConfig
    {
        public static void Register(IDependencyInjectionContainer container)
        {
            // Setup global sitemap loader
            MvcSiteMapProvider.SiteMaps.Loader = container.GetInstance<ISiteMapLoader>();

            // Check all configured .sitemap files to ensure they follow the XSD for MvcSiteMapProvider
            var validator = container.GetInstance<ISiteMapXmlValidator>();
            validator.ValidateXml(HostingEnvironment.MapPath("~/Mvc.sitemap"));
        }
    }
}