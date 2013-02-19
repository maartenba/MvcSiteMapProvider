using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Loader.SiteMapLoader"/>
    /// at runtime.
    /// </summary>
    public class SiteMapLoaderFactory
        : ISiteMapLoaderFactory
    {
        public SiteMapLoaderFactory(
            ISiteMapCache siteMapCache,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator,
            ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy,
            ISiteMapFactory siteMapFactory,
            ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper
            )
        {
            if (siteMapCache == null)
                throw new ArgumentNullException("siteMapCache");
            if (siteMapCacheKeyGenerator == null)
                throw new ArgumentNullException("siteMapCacheKeyGenerator");
            if (siteMapBuilderSetStrategy == null)
                throw new ArgumentNullException("siteMapBuilderSetStrategy");
            if (siteMapFactory == null)
                throw new ArgumentNullException("siteMapFactory");
            if (siteMapCacheKeyToBuilderSetMapper == null)
                throw new ArgumentNullException("siteMapCacheKeyToBuilderSetMapper");

            this.siteMapCache = siteMapCache;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;
            this.siteMapBuilderSetStrategy = siteMapBuilderSetStrategy;
            this.siteMapFactory = siteMapFactory;
            this.siteMapCacheKeyToBuilderSetMapper = siteMapCacheKeyToBuilderSetMapper;
        }

        protected readonly ISiteMapCache siteMapCache;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        protected readonly ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy;
        protected readonly ISiteMapFactory siteMapFactory;
        protected readonly ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper;


        #region ISiteMapLoaderFactory Members

        public virtual ISiteMapLoader Create()
        {
            return new SiteMapLoader(
                siteMapCache,
                siteMapCacheKeyGenerator,
                siteMapBuilderSetStrategy,
                siteMapFactory,
                siteMapCacheKeyToBuilderSetMapper);
        }

        #endregion
    }
}
