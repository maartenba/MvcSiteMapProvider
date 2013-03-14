using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract used to provide caching instructions.
    /// </summary>
    public interface ICacheDetails
    {
        TimeSpan AbsoluteCacheExpiration { get; }
        TimeSpan SlidingCacheExpiration { get; }
        ICacheDependencyFactory CacheDependencyFactory { get; }

        /// <summary>
        /// Determines whether the ICacheDetails instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the ICacheDetails instance.</param>
        /// <returns>
        /// True if the cache details name matches.
        /// </returns>
        bool AppliesTo(string cacheDetailsName);
    }
}
