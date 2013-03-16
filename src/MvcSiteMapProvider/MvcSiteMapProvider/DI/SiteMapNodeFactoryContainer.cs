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
            var aggregator = new XmlDistinctAttributeAggregator();
            var instantiator = new PluginInstantiator<IDynamicNodeProvider>();
            var typeNames = aggregator.GetAttributeValues(this.settings.SiteMapFileName, "dynamicNodeProvider");
            var providers = instantiator.GetInstances(typeNames);
            return providers.ToArray();
        }

        private ISiteMapNodeUrlResolver[] ResolveSiteMapNodeUrlResolvers()
        {
            var aggregator = new XmlDistinctAttributeAggregator();
            var instantiator = new PluginInstantiator<ISiteMapNodeUrlResolver>();
            var typeNames = aggregator.GetAttributeValues(this.settings.SiteMapFileName, "urlResolver");

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
            var aggregator = new XmlDistinctAttributeAggregator();
            var instantiator = new PluginInstantiator<ISiteMapNodeVisibilityProvider>();
            var typeNames = aggregator.GetAttributeValues(this.settings.SiteMapFileName, "visibilityProvider");
            var providers = instantiator.GetInstances(typeNames);
            return providers.ToArray();
        }
    }
}
