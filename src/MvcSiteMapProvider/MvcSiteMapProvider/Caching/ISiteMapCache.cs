using System;
using System.Collections.Generic;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract to provide caching-technology agnostic access to a specific type of cache.
    /// </summary>
    public interface ISiteMapCache
    {
        int Count { get; }
        void Insert(string key, ISiteMap siteMap, ICacheDetails cacheDetails);
        ISiteMap this[string key] { get; set; }
        void Remove(string key);
        bool TryGetValue(string key, out ISiteMap value);
        event EventHandler<SiteMapCacheItemRemovedEventArgs> SiteMapRemoved;
    }
}
