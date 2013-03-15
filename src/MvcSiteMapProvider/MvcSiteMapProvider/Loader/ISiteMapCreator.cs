using System;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// Contract for a creator implementation to build a specific <see cref="T:MvcSiteMapProvider.ISiteMap"/> instance.
    /// </summary>
    public interface ISiteMapCreator
    {
        ISiteMapCreatorResult CreateSiteMap(string siteMapCacheKey);
    }
}
