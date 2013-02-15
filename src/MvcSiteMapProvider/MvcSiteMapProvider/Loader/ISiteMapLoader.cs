using System;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Loader
{
    public interface ISiteMapLoader
    {
        ISiteMap GetSiteMap();
        ISiteMap GetSiteMap(string siteMapKey);
    }
}
