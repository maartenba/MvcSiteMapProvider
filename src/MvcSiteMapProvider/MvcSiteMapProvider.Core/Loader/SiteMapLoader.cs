using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading;
using MvcSiteMapProvider.Core.Cache;
using MvcSiteMapProvider.Core.SiteMap;
using MvcSiteMapProvider.Core.SiteMap.Builder;
using MvcSiteMapProvider.Core.Web;

namespace MvcSiteMapProvider.Core.Loader
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapLoader 
        : ISiteMapLoader
    {
        public SiteMapLoader(
            TimeSpan slidingCacheExpiration,
            ISiteMapCache siteMapCache,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator,
            ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy,
            ISiteMapFactory siteMapFactory,
            ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper
            
            )
        {
            if (slidingCacheExpiration == null)
                throw new ArgumentNullException("slidingCacheExpiration");
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
            

            this.slidingCacheExpiration = slidingCacheExpiration;
            this.siteMapCache = siteMapCache;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;
            this.siteMapBuilderSetStrategy = siteMapBuilderSetStrategy;
            this.siteMapFactory = siteMapFactory;
            this.siteMapCacheKeyToBuilderSetMapper = siteMapCacheKeyToBuilderSetMapper;
            
        }

        protected readonly TimeSpan slidingCacheExpiration;
        protected readonly ISiteMapCache siteMapCache;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        protected readonly ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy;
        protected readonly ISiteMapFactory siteMapFactory;
        protected readonly ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper;
        

        protected readonly ReaderWriterLockSlim synclock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        public virtual ISiteMap GetSiteMap()
        {
            var key = siteMapCacheKeyGenerator.GenerateKey();
            return GetSiteMap(key);
        }

        public virtual ISiteMap GetSiteMap(string siteMapCacheKey)
        {
            ISiteMap siteMap = null;

            synclock.EnterReadLock();
            try
            {
                siteMap = siteMapCache[siteMapCacheKey];
            }
            finally
            {
                synclock.ExitReadLock();
            }

            if (siteMap == null)
            {
                synclock.EnterWriteLock();
                try
                {
                    // Build sitemap
                    var builderSetName = siteMapCacheKeyToBuilderSetMapper.GetBuilderSetName(siteMapCacheKey);
                    var builder = siteMapBuilderSetStrategy.GetBuilder(builderSetName);
                    siteMap = siteMapFactory.Create(builder);
                    siteMap.BuildSiteMap();

                    siteMapCache.Insert(siteMapCacheKey, siteMap, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingCacheExpiration);
                }
                finally
                {
                    synclock.ExitWriteLock();
                }
            }
            return siteMap;
        }
    }
}
