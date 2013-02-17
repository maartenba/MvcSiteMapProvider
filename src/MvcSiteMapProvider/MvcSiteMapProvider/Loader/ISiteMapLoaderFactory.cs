using System;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapLoaderFactory
    {
        ISiteMapLoader Create(TimeSpan absoluteCacheExpiration, TimeSpan slidingCacheExpiration);
    }
}
