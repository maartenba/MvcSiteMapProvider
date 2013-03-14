using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract to provide a concrete implementation that tracks all of the registered instances of 
    /// <see cref="T:MvcSiteMapProvider.Caching.ICacheDetails"/> and allows the caller to get a specific 
    /// named instance of this interface at runtime.
    /// </summary>
    public interface ICacheDetailsStrategy
    {
        ICacheDetails GetCacheDetails(string cacheDetailsName);
    }
}
