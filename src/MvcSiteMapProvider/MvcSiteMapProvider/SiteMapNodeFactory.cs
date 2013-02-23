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
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy,
            ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy,
            IActionMethodParameterResolver actionMethodParameterResolver,
            IUrlPath urlPath,
            RouteCollection routes,
            IRequestCache requestCache
            ) 
        {
            if (siteMapNodeChildStateFactory == null)
                throw new ArgumentNullException("siteMapNodeChildStateFactory");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");
            if (siteMapNodeVisibilityProviderStrategy == null)
                throw new ArgumentNullException("siteMapNodeVisibilityProviderStrategy");
            if (actionMethodParameterResolver == null)
                throw new ArgumentNullException("actionMethodParameterResolver");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (routes == null)
                throw new ArgumentNullException("routes");
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.siteMapNodeChildStateFactory = siteMapNodeChildStateFactory;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
            this.siteMapNodeVisibilityProviderStrategy = siteMapNodeVisibilityProviderStrategy;
            this.actionMethodParameterResolver = actionMethodParameterResolver;
            this.urlPath = urlPath;
            this.routes = routes;
            this.requestCache = requestCache;
        }

        // Services
        protected readonly ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;
        protected readonly ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy;
        protected readonly IActionMethodParameterResolver actionMethodParameterResolver;
        protected readonly IUrlPath urlPath;
        protected readonly RouteCollection routes;
        protected readonly IRequestCache requestCache;


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
            // IMPORTANT: we must create one localization service per node because the service contains its own state that applies to the node
            var localizationService = siteMapNodeChildStateFactory.CreateLocalizationService(implicitResourceKey);

            return new RequestCacheableSiteMapNode(
                siteMap,
                key,
                isDynamic,
                siteMapNodeChildStateFactory,
                localizationService,
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy,
                siteMapNodeVisibilityProviderStrategy,
                actionMethodParameterResolver,
                urlPath,
                routes,
                requestCache);
        }

        #endregion
    }
}
