using System;
using System.Collections.Generic;
using System.Web.Routing;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.RequestCacheableSiteMapNode"/>
    /// at runtime.
    /// </summary>
    public class SiteMapNodeFactory 
        : ISiteMapNodeFactory
    {
        public SiteMapNodeFactory(
            ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory,
            ILocalizationServiceFactory localizationServiceFactory,
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy,
            ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy,
            IUrlPath urlPath,
            IMvcContextFactory mvcContextFactory
            ) 
        {
            if (siteMapNodeChildStateFactory == null)
                throw new ArgumentNullException("siteMapNodeChildStateFactory");
            if (localizationServiceFactory == null)
                throw new ArgumentNullException("localizationServiceFactory");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");
            if (siteMapNodeVisibilityProviderStrategy == null)
                throw new ArgumentNullException("siteMapNodeVisibilityProviderStrategy");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");

            this.siteMapNodeChildStateFactory = siteMapNodeChildStateFactory;
            this.localizationServiceFactory = localizationServiceFactory;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
            this.siteMapNodeVisibilityProviderStrategy = siteMapNodeVisibilityProviderStrategy;
            this.urlPath = urlPath;
            this.mvcContextFactory = mvcContextFactory;
        }

        // Services
        protected readonly ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory;
        protected readonly ILocalizationServiceFactory localizationServiceFactory;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;
        protected readonly ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy;
        protected readonly IUrlPath urlPath;
        protected readonly IMvcContextFactory mvcContextFactory;


        #region ISiteMapNodeFactory Members

        public ISiteMapNode Create(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return CreateInternal(siteMap, key, implicitResourceKey, false);
        }

        public ISiteMapNode CreateDynamic(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return CreateInternal(siteMap, key, implicitResourceKey, true);
        }

        protected ISiteMapNode CreateInternal(ISiteMap siteMap, string key, string implicitResourceKey, bool isDynamic)
        {
            var routes = mvcContextFactory.GetRoutes();
            var requestCache = mvcContextFactory.GetRequestCache();

            // IMPORTANT: we must create one localization service per node because the service contains its own state that applies to the node
            var localizationService = localizationServiceFactory.Create(implicitResourceKey);

            return new RequestCacheableSiteMapNode(
                siteMap,
                key,
                isDynamic,
                siteMapNodeChildStateFactory,
                localizationService,
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy,
                siteMapNodeVisibilityProviderStrategy,
                urlPath,
                routes,
                requestCache);
        }

        #endregion
    }
}
