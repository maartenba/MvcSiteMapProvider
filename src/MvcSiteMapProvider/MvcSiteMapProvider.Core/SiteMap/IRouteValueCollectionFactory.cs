using System;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRouteValueCollectionFactory
    {
        IRouteValueCollection Create(ISiteMap siteMap);
    }
}
