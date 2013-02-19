using System;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// Contract for a loader implementation to get a specific <see cref="T:MvcSiteMapProvider.ISiteMap"/> instance.
    /// </summary>
    public interface ISiteMapLoader
    {
        ISiteMap GetSiteMap();
        ISiteMap GetSiteMap(string siteMapKey);
    }
}
