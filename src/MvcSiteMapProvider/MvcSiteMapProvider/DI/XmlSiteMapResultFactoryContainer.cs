using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Loader;

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
        }

        private readonly ISiteMapLoader siteMapLoader;
        private readonly IUrlPath urlPath;

        public IXmlSiteMapResultFactory ResolveXmlSiteMapResultFactory()
        {
            return new XmlSiteMapResultFactory(
                this.siteMapLoader,
                this.urlPath);
        }
    }
}
