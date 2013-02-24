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
            IMvcResolverFactory mvcResolverFactory,
            IMvcContextFactory mvcContextFactory,
            IAclModule aclModule,
            ISiteMapChildStateFactory siteMapChildStateFactory,
            IUrlPath urlPath,
            IControllerTypeResolverFactory controllerTypeResolverFactory,
            IActionMethodParameterResolverFactory actionMethodParameterResolverFactory
            )
        {
            if (mvcResolverFactory == null)
                throw new ArgumentNullException("mvcResolverFactory");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");
            if (siteMapChildStateFactory == null)
                throw new ArgumentNullException("siteMapChildStateFactory");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (controllerTypeResolverFactory == null)
                throw new ArgumentNullException("controllerTypeResolverFactory");
            if (actionMethodParameterResolverFactory == null)
                throw new ArgumentNullException("actionMethodParameterResolverFactory");

            this.mvcResolverFactory = mvcResolverFactory;
            this.mvcContextFactory = mvcContextFactory;
            this.aclModule = aclModule;
            this.siteMapChildStateFactory = siteMapChildStateFactory;
            this.urlPath = urlPath;
            this.controllerTypeResolverFactory = controllerTypeResolverFactory;
            this.actionMethodParameterResolverFactory = actionMethodParameterResolverFactory;
        }

        protected readonly IMvcResolverFactory mvcResolverFactory;
        protected readonly IMvcContextFactory mvcContextFactory;
        protected readonly IAclModule aclModule;
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
            // one instance of ActionMethodParameterResolver per SiteMap instance.
            var controllerTypeResolver = this.controllerTypeResolverFactory.Create(routes);
            var actionMethodParameterResolver = actionMethodParameterResolverFactory.Create();

            var mvcResolver = mvcResolverFactory.Create(controllerTypeResolver, actionMethodParameterResolver);

            return new RequestCacheableSiteMap(
                siteMapBuilder,
                mvcResolver,
                mvcContextFactory,
                aclModule,
                siteMapChildStateFactory,
                urlPath,
                requestCache);
        }

        #endregion
    }
}
