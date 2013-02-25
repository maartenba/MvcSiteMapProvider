using System;
using System.Web.Routing;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.RequestCacheableSiteMap"/>
    /// at runtime.
    /// </summary>
    public class SiteMapFactory
        : ISiteMapFactory
    {
        public SiteMapFactory(
            ISiteMapPluginProviderFactory pluginProviderFactory,
            IMvcResolverFactory mvcResolverFactory,
            IMvcContextFactory mvcContextFactory,
            ISiteMapChildStateFactory siteMapChildStateFactory,
            IUrlPath urlPath,
            IControllerTypeResolverFactory controllerTypeResolverFactory,
            IActionMethodParameterResolverFactory actionMethodParameterResolverFactory
            )
        {
            if (pluginProviderFactory == null)
                throw new ArgumentNullException("pluginProviderFactory");
            if (mvcResolverFactory == null)
                throw new ArgumentNullException("mvcResolverFactory");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            if (siteMapChildStateFactory == null)
                throw new ArgumentNullException("siteMapChildStateFactory");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (controllerTypeResolverFactory == null)
                throw new ArgumentNullException("controllerTypeResolverFactory");
            if (actionMethodParameterResolverFactory == null)
                throw new ArgumentNullException("actionMethodParameterResolverFactory");

            this.pluginProviderFactory = pluginProviderFactory;
            this.mvcResolverFactory = mvcResolverFactory;
            this.mvcContextFactory = mvcContextFactory;
            this.siteMapChildStateFactory = siteMapChildStateFactory;
            this.urlPath = urlPath;
            this.controllerTypeResolverFactory = controllerTypeResolverFactory;
            this.actionMethodParameterResolverFactory = actionMethodParameterResolverFactory;
        }

        protected readonly ISiteMapPluginProviderFactory pluginProviderFactory;
        protected readonly IMvcResolverFactory mvcResolverFactory;
        protected readonly IMvcContextFactory mvcContextFactory;
        protected readonly ISiteMapChildStateFactory siteMapChildStateFactory;
        protected readonly IUrlPath urlPath;
        protected readonly IControllerTypeResolverFactory controllerTypeResolverFactory;
        protected readonly IActionMethodParameterResolverFactory actionMethodParameterResolverFactory;
        

        #region ISiteMapFactory Members

        public virtual ISiteMap Create(ISiteMapBuilder siteMapBuilder)
        {
            var routes = mvcContextFactory.GetRoutes();
            var requestCache = mvcContextFactory.GetRequestCache();

            // IMPORTANT: We need to ensure there is one instance of controllerTypeResolver and 
            // one instance of ActionMethodParameterResolver per SiteMap instance because each of
            // these classes does internal caching.
            var controllerTypeResolver = controllerTypeResolverFactory.Create(routes);
            var actionMethodParameterResolver = actionMethodParameterResolverFactory.Create();
            var mvcResolver = mvcResolverFactory.Create(controllerTypeResolver, actionMethodParameterResolver);
            var pluginProvider = pluginProviderFactory.Create(siteMapBuilder, mvcResolver);

            return new RequestCacheableSiteMap(
                pluginProvider,
                mvcContextFactory,
                siteMapChildStateFactory,
                urlPath,
                requestCache);
        }

        #endregion
    }
}
