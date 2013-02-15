using System;
using MvcSiteMapProvider.Core;

namespace MvcSiteMapProvider.Core.Loader
{
    public interface ISiteMapLoader
    {
        ISiteMap GetSiteMap();
        ISiteMap GetSiteMap(string siteMapKey);
    }
}
