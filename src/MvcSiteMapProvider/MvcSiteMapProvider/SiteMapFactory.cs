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
            IAclModule aclModule,
            IHttpContextFactory httpContextFactory,
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory,
            IGenericDictionaryFactory genericDictionaryFactory,
            IUrlPath urlPath,
            RouteCollection routes,
            IRequestCache requestCache,
            IControllerTypeResolverFactory controllerTypeResolverFactory
            )
        {
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");
            if (siteMapNodeCollectionFactory == null)
                throw new ArgumentNullException("siteMapNodeCollectionFactory");
            if (genericDictionaryFactory == null)
                throw new ArgumentNullException("genericDictionaryFactory");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (routes == null)
                throw new ArgumentNullException("routes");
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");
            if (controllerTypeResolverFactory == null)
                throw new ArgumentNullException("controllerTypeResolverFactory");

            this.aclModule = aclModule;
            this.httpContextFactory = httpContextFactory;
            this.siteMapNodeCollectionFactory = siteMapNodeCollectionFactory;
            this.genericDictionaryFactory = genericDictionaryFactory;
            this.urlPath = urlPath;
            this.routes = routes;
            this.requestCache = requestCache;
            this.controllerTypeResolverFactory = controllerTypeResolverFactory;
        }

        protected readonly IAclModule aclModule;
        protected readonly IHttpContextFactory httpContextFactory;
        protected readonly ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory;
        protected readonly IGenericDictionaryFactory genericDictionaryFactory;
        protected readonly IUrlPath urlPath;
        protected readonly RouteCollection routes;
        protected readonly IRequestCache requestCache;
        protected readonly IControllerTypeResolverFactory controllerTypeResolverFactory;

        #region ISiteMapFactory Members

        public virtual ISiteMap Create(ISiteMapBuilder siteMapBuilder)
        {
            // IMPORTANT: We need to ensure there is one instance of controllerTypeResolver and 
            // one instance of ActionMethodParameterResolver per SiteMap instance.
            var controllerTypeResolver = this.controllerTypeResolverFactory.Create(routes);

            // TODO: create factories for ControllerTypeResolver and ActionMethodParameterResolver
            var actionMethodParameterResolver = new ActionMethodParameterResolver();

            return new RequestCacheableSiteMap(
                siteMapBuilder,
                controllerTypeResolver,
                actionMethodParameterResolver,
                httpContextFactory,
                aclModule,
                siteMapNodeCollectionFactory,
                genericDictionaryFactory,
                urlPath,
                routes,
                requestCache);
        }

        #endregion
    }
}
