using System;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Loader
{
    public interface ISiteMapLoader
    {
        ISiteMap GetSiteMap();
        ISiteMap GetSiteMap(string siteMapKey);
    }
}
