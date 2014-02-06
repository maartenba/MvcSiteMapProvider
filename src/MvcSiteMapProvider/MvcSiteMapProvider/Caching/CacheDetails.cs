using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Container for passing caching instructions around as a group.
    /// </summary>
    public class CacheDetails
        : ICacheDetails
    {
        public CacheDetails(
            TimeSpan absoluteCacheExpiration,
            TimeSpan slidingCacheExpiration,
            ICacheDependency cacheDependency
            )
        {
            if (absoluteCacheExpiration == null)
                throw new ArgumentNullException("absoluteCacheExpiration");
            if (slidingCacheExpiration == null)
                throw new ArgumentNullException("slidingCacheExpiration");
            if (cacheDependency == null)
                throw new ArgumentNullException("cacheDependency");

            this.absoluteCacheExpiration = absoluteCacheExpiration;
            this.slidingCacheExpiration = slidingCacheExpiration;
            this.cacheDependency = cacheDependency;
        }

        protected readonly TimeSpan absoluteCacheExpiration;
        protected readonly TimeSpan slidingCacheExpiration;
        protected readonly ICacheDependency cacheDependency;

        #region ICacheDetails Members

        public TimeSpan AbsoluteCacheExpiration
        {
            get { return this.absoluteCacheExpiration; }
        }

        public TimeSpan SlidingCacheExpiration
        {
            get { return this.slidingCacheExpiration; }
        }

        public ICacheDependency CacheDependency
        {
            get { return this.cacheDependency; }
        }

        #endregion
    }
}
