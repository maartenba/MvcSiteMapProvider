using System;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRouteValueCollectionFactory
    {
        IRouteValueCollection Create(ISiteMap siteMap);
    }
}
