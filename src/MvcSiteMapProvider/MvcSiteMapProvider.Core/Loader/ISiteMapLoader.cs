using System;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Loader
{
    public interface ISiteMapLoader
    {
        ISiteMap GetSiteMap(string builderSetName);
        ISiteMap GetSiteMap(string siteMapKey, string builderSetName);
    }
}
