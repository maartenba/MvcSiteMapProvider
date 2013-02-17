using System;
using System.Collections.Generic;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Caching
{
    public interface ISiteMapCache
    {
        int Count { get; }
        void Insert(string key, ISiteMap siteMap, TimeSpan absoluteExpiration, TimeSpan slidingExpiration);
        void Insert(string key, ISiteMap siteMap, TimeSpan absoluteExpiration, TimeSpan slidingExpiration, IEnumerable<string> fileDependencies);
        ISiteMap this[string key] { get; set; }
        void Remove(string key);
        bool TryGetValue(string key, out ISiteMap value);
        event EventHandler<SiteMapCacheItemRemovedEventArgs> SiteMapRemoved;
    }
}
