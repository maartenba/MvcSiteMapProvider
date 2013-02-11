using System;
using MvcSiteMapProvider.Core.SiteMap.Builder;
using MvcSiteMapProvider.Core.Security;
using MvcSiteMapProvider.Core.Mvc;
using MvcSiteMapProvider.Core.Collections;
using MvcSiteMapProvider.Core.RequestCache;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapFactory
        : ISiteMapFactory
    {
        public SiteMapFactory(
            IAclModule aclModule,
            IControllerTypeResolver controllerTypeResolver,
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory,
            IGenericDictionaryFactory genericDictionaryFactory,
            IRequestCache requestCache
            )
        {
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");
            if (controllerTypeResolver == null)
                throw new ArgumentNullException("controllerTypeResolver");
            if (siteMapNodeCollectionFactory == null)
                throw new ArgumentNullException("siteMapNodeCollectionFactory");
            if (genericDictionaryFactory == null)
                throw new ArgumentNullException("genericDictionaryFactory");
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.aclModule = aclModule;
            this.controllerTypeResolver = controllerTypeResolver;
            this.siteMapNodeCollectionFactory = siteMapNodeCollectionFactory;
            this.genericDictionaryFactory = genericDictionaryFactory;
            this.requestCache = requestCache;
        }

        private readonly IAclModule aclModule;
        private readonly IControllerTypeResolver controllerTypeResolver;
        private readonly ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory;
        private readonly IGenericDictionaryFactory genericDictionaryFactory;
        private readonly IRequestCache requestCache;

        #region ISiteMapFactory Members

        public ISiteMap Create(ISiteMapBuilder siteMapBuilder)
        {
            var siteMap = new SiteMap(
                siteMapBuilder, 
                aclModule, 
                controllerTypeResolver, 
                siteMapNodeCollectionFactory,
                genericDictionaryFactory);

            // Decorate the class with additional responsibilities.
            var lockableSiteMap = new LockableSiteMap(siteMap);
            var requestCacheableSiteMap = new RequestCacheableSiteMap(lockableSiteMap, requestCache);


            return requestCacheableSiteMap;
        }

        #endregion
    }
}
