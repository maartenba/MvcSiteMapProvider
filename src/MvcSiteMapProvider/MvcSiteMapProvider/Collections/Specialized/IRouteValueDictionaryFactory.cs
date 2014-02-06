using System;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of 
    /// <see cref="T:MvcSiteMapProvider.Collections.Specialized.IRouteValueDictionary"/> at runtime.
    /// </summary>
    public interface IRouteValueDictionaryFactory
    {
        IRouteValueDictionary Create(ISiteMap siteMap);
    }
}
