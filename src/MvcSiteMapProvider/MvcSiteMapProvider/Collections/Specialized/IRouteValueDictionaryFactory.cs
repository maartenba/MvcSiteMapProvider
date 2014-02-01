using System;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of 
    /// <see cref="T:MvcSiteMapProvider.Collections.Specialized.IRouteValueDictionary"/> at runtime.
    /// </summary>
    public interface IRouteValueDictionaryFactory
    {
        IRouteValueDictionary Create(string siteMapNodeKey, ISiteMap siteMap);

        [Obsolete("Use the overload that accepts a siteMapNodeKey instead. This overload will be removed in version 5.")]
        IRouteValueDictionary Create(ISiteMap siteMap);
    }
}
