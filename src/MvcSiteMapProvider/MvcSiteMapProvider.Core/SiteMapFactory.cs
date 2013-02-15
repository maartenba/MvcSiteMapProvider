using System;
using MvcSiteMapProvider.Core.Builder;
using MvcSiteMapProvider.Core.Security;
using MvcSiteMapProvider.Core.Mvc;
using MvcSiteMapProvider.Core.Collections;
using MvcSiteMapProvider.Core.RequestCache;
using MvcSiteMapProvider.Core.Web;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// TODO: Update summary.
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
            IRequestCache requestCache
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
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.aclModule = aclModule;
            this.httpContextFactory = httpContextFactory;
            this.siteMapNodeCollectionFactory = siteMapNodeCollectionFactory;
            this.genericDictionaryFactory = genericDictionaryFactory;
            this.urlPath = urlPath;
            this.requestCache = requestCache;
        }

        protected readonly IAclModule aclModule;
        protected readonly IHttpContextFactory httpContextFactory;
        protected readonly ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory;
        protected readonly IGenericDictionaryFactory genericDictionaryFactory;
        protected readonly IUrlPath urlPath;
        protected readonly IRequestCache requestCache;

        #region ISiteMapFactory Members

        public ISiteMap Create(ISiteMapBuilder siteMapBuilder)
        {
            return new RequestCacheableSiteMap(
                siteMapBuilder,
                httpContextFactory,
                aclModule,
                siteMapNodeCollectionFactory,
                genericDictionaryFactory,
                urlPath,
                requestCache);
        }

        #endregion
    }
}
