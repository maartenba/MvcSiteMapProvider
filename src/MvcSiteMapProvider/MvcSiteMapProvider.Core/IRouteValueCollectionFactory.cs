using System;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRouteValueCollectionFactory
    {
        IRouteValueCollection Create(ISiteMap siteMap);
    }
}
