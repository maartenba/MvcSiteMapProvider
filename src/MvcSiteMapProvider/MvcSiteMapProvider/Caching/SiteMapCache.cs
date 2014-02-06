using System;

namespace MvcSiteMapProvider.Caching
{
    public class SiteMapCache
        : MicroCache<ISiteMap>, ISiteMapCache
    {
        public SiteMapCache(
            ICacheProvider<ISiteMap> cacheProvider
            ) : base(cacheProvider)
        {
        }
    }
}
