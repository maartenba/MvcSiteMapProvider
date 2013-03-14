using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Container for passing caching instructions around as a group.
    /// </summary>
    public class CacheDetails
        : ICacheDetails
    {
        public CacheDetails(
            string instanceName,
            TimeSpan absoluteCacheExpiration,
            TimeSpan slidingCacheExpiration,
            ICacheDependencyFactory cacheDependencyFactory
            )
        {
            if (String.IsNullOrEmpty(instanceName))
                throw new ArgumentNullException("instanceName");
            if (absoluteCacheExpiration == null)
                throw new ArgumentNullException("absoluteCacheExpiration");
            if (slidingCacheExpiration == null)
                throw new ArgumentNullException("slidingCacheExpiration");
            if (cacheDependencyFactory == null)
                throw new ArgumentNullException("cacheDependencyFactory");

            this.instanceName = instanceName;
            this.absoluteCacheExpiration = absoluteCacheExpiration;
            this.slidingCacheExpiration = slidingCacheExpiration;
            this.cacheDependencyFactory = cacheDependencyFactory;
        }

        protected readonly string instanceName;
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

        public bool AppliesTo(string cacheDetailsName)
        {
            return this.instanceName.Equals(cacheDetailsName, StringComparison.InvariantCulture);
        }

        #endregion


    }
}
