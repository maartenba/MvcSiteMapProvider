using System;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Loader;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;


namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// A specialized dependency injection container for resolving a <see cref="T:MvcSiteMapProvider.Web.Mvc.XmlSiteMapResultFactory"/> instance.
    /// </summary>
    public class XmlSiteMapResultFactoryContainer
    {
        public XmlSiteMapResultFactoryContainer(ConfigurationSettings settings)
        {
            var siteMapLoaderContainer = new SiteMapLoaderContainer(settings);
            this.siteMapLoader = siteMapLoaderContainer.ResolveSiteMapLoader();
            this.urlPath = new UrlPath(new MvcContextFactory());
            this.cultureContextFactory = new CultureContextFactory();
        }

        private readonly ISiteMapLoader siteMapLoader;
        private readonly IUrlPath urlPath;
        private readonly ICultureContextFactory cultureContextFactory;

        public IXmlSiteMapResultFactory ResolveXmlSiteMapResultFactory()
        {
            return new XmlSiteMapResultFactory(
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }
    }
}
