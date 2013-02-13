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
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory,
            IGenericDictionaryFactory genericDictionaryFactory,
            IRequestCache requestCache
            )
        {
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");
            if (siteMapNodeCollectionFactory == null)
                throw new ArgumentNullException("siteMapNodeCollectionFactory");
            if (genericDictionaryFactory == null)
                throw new ArgumentNullException("genericDictionaryFactory");
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.aclModule = aclModule;
            this.siteMapNodeCollectionFactory = siteMapNodeCollectionFactory;
            this.genericDictionaryFactory = genericDictionaryFactory;
            this.requestCache = requestCache;
        }

        protected readonly IAclModule aclModule;
        protected readonly ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory;
        protected readonly IGenericDictionaryFactory genericDictionaryFactory;
        protected readonly IRequestCache requestCache;

        #region ISiteMapFactory Members

        public ISiteMap Create(ISiteMapBuilder siteMapBuilder)
        {
            return new RequestCacheableSiteMap(
                siteMapBuilder,
                aclModule,
                siteMapNodeCollectionFactory,
                genericDictionaryFactory,
                requestCache);
        }

        #endregion
    }
}
