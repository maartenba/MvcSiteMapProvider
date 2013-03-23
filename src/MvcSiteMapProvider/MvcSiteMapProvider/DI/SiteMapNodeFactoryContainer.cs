using System;
using System.Linq;
using System.Collections.Generic;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// A specialized dependency injection container for resolving a <see cref="T:MvcSiteMapProvider.SiteMapNodeFactory"/> instance.
    /// </summary>
    public class SiteMapNodeFactoryContainer
    {
        public SiteMapNodeFactoryContainer(
            ConfigurationSettings settings,
            IMvcContextFactory mvcContextFactory,
            IUrlPath urlPath)
        {
            this.settings = settings;
            this.mvcContextFactory = mvcContextFactory;
            this.requestCache = this.mvcContextFactory.GetRequestCache();
            this.urlPath = urlPath;
            this.dynamicNodeProviders = this.ResolveDynamicNodeProviders();
            this.siteMapNodeUrlResolvers = this.ResolveSiteMapNodeUrlResolvers();
            this.siteMapNodeVisibilityProviders = this.ResolveSiteMapNodeVisibilityProviders();
        }

        private readonly ConfigurationSettings settings;
        private readonly IMvcContextFactory mvcContextFactory;
        private readonly IRequestCache requestCache;
        private readonly IUrlPath urlPath;
        private readonly IDynamicNodeProvider[] dynamicNodeProviders;
        private readonly ISiteMapNodeUrlResolver[] siteMapNodeUrlResolvers;
        private readonly ISiteMapNodeVisibilityProvider[] siteMapNodeVisibilityProviders;

        private readonly XmlDistinctAttributeAggregator xmlAggergator 
            = new XmlDistinctAttributeAggregator(new SiteMapXmlNameProvider());

        public ISiteMapNodeFactory ResolveSiteMapNodeFactory()
        {
            return new SiteMapNodeFactory(
                this.ResolveSiteMapNodeChildStateFactory(),
                this.ResolveLocalizationServiceFactory(),
                this.ResolveSiteMapNodePluginProvider(),
                this.urlPath,
                this.mvcContextFactory); 
        }

        private ISiteMapNodeChildStateFactory ResolveSiteMapNodeChildStateFactory()
        {
            return new SiteMapNodeChildStateFactory(
                new AttributeCollectionFactory(this.requestCache),
                new RouteValueCollectionFactory(this.requestCache));
        }

        private ILocalizationServiceFactory ResolveLocalizationServiceFactory()
        {
            return new LocalizationServiceFactory(
                new ExplicitResourceKeyParser(),
                new StringLocalizer(this.mvcContextFactory));
        }

        private ISiteMapNodePluginProvider ResolveSiteMapNodePluginProvider()
        {
            return new SiteMapNodePluginProvider(
                new DynamicNodeProviderStrategy(this.dynamicNodeProviders),
                new SiteMapNodeUrlResolverStrategy(this.siteMapNodeUrlResolvers),
                new SiteMapNodeVisibilityProviderStrategy(this.siteMapNodeVisibilityProviders));
        }

        private IDynamicNodeProvider[] ResolveDynamicNodeProviders()
        {
            var instantiator = new PluginInstantiator<IDynamicNodeProvider>();
            var xmlSource = new FileXmlSource(this.settings.SiteMapFileName);
            var typeNames = xmlAggergator.GetAttributeValues(xmlSource, "dynamicNodeProvider");
            var providers = instantiator.GetInstances(typeNames);
            return providers.ToArray();
        }

        private ISiteMapNodeUrlResolver[] ResolveSiteMapNodeUrlResolvers()
        {
            var instantiator = new PluginInstantiator<ISiteMapNodeUrlResolver>();
            var xmlSource = new FileXmlSource(this.settings.SiteMapFileName);
            var typeNames = xmlAggergator.GetAttributeValues(xmlSource, "urlResolver");

            // Add the default provider if it is missing
            var defaultName = typeof(SiteMapNodeUrlResolver).ShortAssemblyQualifiedName();
            if (!typeNames.Contains(defaultName))
            {
                typeNames.Add(defaultName);
            }

            var providers = instantiator.GetInstances(typeNames, new object[] { this.mvcContextFactory });
            return providers.ToArray();
        }

        private ISiteMapNodeVisibilityProvider[] ResolveSiteMapNodeVisibilityProviders()
        {
            var instantiator = new PluginInstantiator<ISiteMapNodeVisibilityProvider>();
            var xmlSource = new FileXmlSource(this.settings.SiteMapFileName);
            var typeNames = xmlAggergator.GetAttributeValues(xmlSource, "visibilityProvider");
            var providers = instantiator.GetInstances(typeNames);
            return providers.ToArray();
        }
    }
}
