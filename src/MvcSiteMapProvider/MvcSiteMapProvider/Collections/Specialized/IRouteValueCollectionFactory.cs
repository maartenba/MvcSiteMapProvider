using System;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.IRouteValueCollection"/> at runtime.
    /// </summary>
    public interface IRouteValueCollectionFactory
    {
        IRouteValueCollection Create(ISiteMap siteMap);
    }
}
