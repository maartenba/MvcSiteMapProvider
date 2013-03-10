using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider.Caching
{
    // TODO: Create CacheDetialsStrategy for registering multiple named instances that can be selected at runtime.

    /// <summary>
    /// Container for passing caching instructions around as a group.
    /// </summary>
    public class CacheDetails
        : ICacheDetails
    {
        public CacheDetails(
            TimeSpan absoluteCacheExpiration,
            TimeSpan slidingCacheExpiration,
            ICacheDependencyFactory cacheDependencyFactory
            )
        {
            if (absoluteCacheExpiration == null)
                throw new ArgumentNullException("absoluteCacheExpiration");
            if (slidingCacheExpiration == null)
                throw new ArgumentNullException("slidingCacheExpiration");
            if (cacheDependencyFactory == null)
                throw new ArgumentNullException("cacheDependencyFactory");

            this.absoluteCacheExpiration = absoluteCacheExpiration;
            this.slidingCacheExpiration = slidingCacheExpiration;
            this.cacheDependencyFactory = cacheDependencyFactory;
        }

        protected readonly TimeSpan absoluteCacheExpiration;
        protected readonly TimeSpan slidingCacheExpiration;
        protected readonly ICacheDependencyFactory cacheDependencyFactory;

        #region ICacheDetails Members

        public TimeSpan AbsoluteCacheExpiration
        {
            get { return this.absoluteCacheExpiration; }
        }

        public TimeSpan SlidingCacheExpiration
        {
            get { return this.slidingCacheExpiration; }
        }

        public ICacheDependencyFactory CacheDependencyFactory
        {
            get { return this.cacheDependencyFactory; }
        }

        #endregion
    }
}
