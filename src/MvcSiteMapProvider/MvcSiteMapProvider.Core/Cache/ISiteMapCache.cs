using System;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Cache
{
    public interface ISiteMapCache
    {
        int Count { get; }
        void Insert(string key, ISiteMap siteMap, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        ISiteMap this[string key] { get; set; }
    }
}
