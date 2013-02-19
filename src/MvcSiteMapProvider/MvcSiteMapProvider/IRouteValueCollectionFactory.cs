using System;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for the abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.IRouteValueCollection"/> at runtime.
    /// </summary>
    public interface IRouteValueCollectionFactory
    {
        IRouteValueCollection Create(ISiteMap siteMap);
    }
}
