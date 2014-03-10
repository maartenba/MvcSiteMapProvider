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
            this.mvcContextFactory = new MvcContextFactory();
            this.bindingFactory = new BindingFactory();
            this.bindingProvider = new BindingProvider(this.bindingFactory, this.mvcContextFactory);
            this.urlPath = new UrlPath(this.mvcContextFactory, this.bindingProvider);
            this.cultureContextFactory = new CultureContextFactory();
        }

        private readonly ISiteMapLoader siteMapLoader;
        private readonly IUrlPath urlPath;
        private readonly IMvcContextFactory mvcContextFactory;
        private readonly ICultureContextFactory cultureContextFactory;
        private readonly IBindingProvider bindingProvider;
        private readonly IBindingFactory bindingFactory;

        public IXmlSiteMapResultFactory ResolveXmlSiteMapResultFactory()
        {
            return new XmlSiteMapResultFactory(
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }
    }
}
