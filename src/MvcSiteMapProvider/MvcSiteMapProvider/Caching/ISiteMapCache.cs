using System;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Caching
{
    public interface ISiteMapCache
    {
        int Count { get; }
        void Insert(string key, ISiteMap siteMap, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        ISiteMap this[string key] { get; set; }
        void Remove(string key);
    }
}
