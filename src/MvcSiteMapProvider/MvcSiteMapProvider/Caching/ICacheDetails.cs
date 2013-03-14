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
        ICacheDependency CacheDependency { get; }
    }
}
