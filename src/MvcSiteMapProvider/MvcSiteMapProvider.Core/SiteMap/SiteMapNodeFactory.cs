using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Core.Mvc.UrlResolver;
using MvcSiteMapProvider.Core.Globalization;
using MvcSiteMapProvider.Core.Collections;
using MvcSiteMapProvider.Core.Mvc;
using MvcSiteMapProvider.Core.RequestCache;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
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
            IControllerTypeResolver controllerTypeResolver,
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
            if (controllerTypeResolver == null)
                throw new ArgumentNullException("controllerTypeResolver");
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.siteMapNodeChildStateFactory = siteMapNodeChildStateFactory;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
            this.siteMapNodeVisibilityProviderStrategy = siteMapNodeVisibilityProviderStrategy;
            this.actionMethodParameterResolver = actionMethodParameterResolver;
            this.controllerTypeResolver = controllerTypeResolver;
            this.requestCache = requestCache;
        }

        // Services
        protected readonly ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;
        protected readonly ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy;
        protected readonly IActionMethodParameterResolver actionMethodParameterResolver;
        protected readonly IControllerTypeResolver controllerTypeResolver;
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

            var siteMapNode = new SiteMapNode(
                siteMap,
                key,
                isDynamic,
                siteMapNodeChildStateFactory,
                localizationService,
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy,
                siteMapNodeVisibilityProviderStrategy,
                actionMethodParameterResolver,
                controllerTypeResolver);

            return siteMapNode;

            // Wrap our decorator classes around sitemapnode
            //var lockableSiteMapNode = new LockableSiteMapNode(siteMapNode);
            //return lockableSiteMapNode;
            //var requestCacheableSiteMapNode = new RequestCacheableSiteMapNode(lockableSiteMapNode, requestCache);
            //return requestCacheableSiteMapNode;
        }

        #endregion
    }
}
