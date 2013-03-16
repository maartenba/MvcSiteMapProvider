using System;
using System.Threading;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Builder;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// <see cref="T:MvcSiteMapProvider.Loader.SiteMapLoader"/> is responsible for thread-safe access to the cache as well as 
    /// managing whether a given request will return a cached sitemap or a new sitemap based on a 
    /// <see cref="T:MvcSiteMapProvider.Builder.IBuilderSet"/>.
    /// </summary>
    public class SiteMapLoader 
        : ISiteMapLoader
    {
        public SiteMapLoader(
            ISiteMapCache siteMapCache,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator,
            ISiteMapCreator siteMapCreator
            )
        {
            if (siteMapCache == null)
                throw new ArgumentNullException("siteMapCache");
            if (siteMapCacheKeyGenerator == null)
                throw new ArgumentNullException("siteMapCacheKeyGenerator");
            if (siteMapCreator == null)
                throw new ArgumentNullException("siteMapCreator");

            this.siteMapCache = siteMapCache;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;
            this.siteMapCreator = siteMapCreator;

            // Attach an event to the cache so when the SiteMap is removed, the Clear() method can be called on it to ensure
            // we don't have any circular references that aren't GC'd.
            siteMapCache.SiteMapRemoved += new EventHandler<SiteMapCacheItemRemovedEventArgs>(siteMapCache_SiteMapRemoved);
        }

        protected readonly ISiteMapCache siteMapCache;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        protected readonly ISiteMapCreator siteMapCreator;
        protected readonly ReaderWriterLockSlim synclock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);


        #region ISiteMapLoader Members

        public virtual ISiteMap GetSiteMap()
        {
            var key = siteMapCacheKeyGenerator.GenerateKey();
            return GetSiteMap(key);
        }

        public virtual ISiteMap GetSiteMap(string siteMapCacheKey)
        {
            if (String.IsNullOrEmpty(siteMapCacheKey))
            {
                throw new ArgumentNullException("siteMapCacheKey");
            }
            return RetrieveSiteMap(siteMapCacheKey);
        }

        public virtual void ReleaseSiteMap()
        {
            var key = siteMapCacheKeyGenerator.GenerateKey();
            ReleaseSiteMap(key);
        }

        public virtual void ReleaseSiteMap(string siteMapCacheKey)
        {
            if (String.IsNullOrEmpty(siteMapCacheKey))
            {
                throw new ArgumentNullException("siteMapCacheKey");
            }
            siteMapCache.Remove(siteMapCacheKey);
        }

        #endregion

        protected virtual ISiteMap RetrieveSiteMap(string siteMapCacheKey)
        {
            synclock.EnterUpgradeableReadLock();
            try
            {
                ISiteMap siteMap = null;
                if (siteMapCache.TryGetValue(siteMapCacheKey, out siteMap))
                {
                    return siteMap;
                }
                else
                {
                    synclock.EnterWriteLock();
                    try
                    {
                        if (!siteMapCache.TryGetValue(siteMapCacheKey, out siteMap))
                        {
                            var cacheDetails = siteMapCreator.GetCacheDetails(siteMapCacheKey);
                            siteMap = siteMapCreator.CreateSiteMap(siteMapCacheKey);

                            siteMapCache.Insert(siteMapCacheKey, siteMap, cacheDetails);
                        }
                        return siteMap;
                    }
                    finally
                    {
                        synclock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                synclock.ExitUpgradeableReadLock();
            }
        }

        protected virtual void siteMapCache_SiteMapRemoved(object sender, SiteMapCacheItemRemovedEventArgs e)
        {
            // Call clear to remove ISiteMapNode object references from internal collections. This
            // will release the circular references and free the memory.
            e.SiteMap.Clear();
        }
    }
}
