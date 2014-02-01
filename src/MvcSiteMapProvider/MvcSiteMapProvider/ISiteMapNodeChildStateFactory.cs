using System;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of types required by the <see cref="T:MvcSiteMapProvider.SiteMapNode"/>
    /// at runtime.
    /// </summary>
    public interface ISiteMapNodeChildStateFactory
    {
        IAttributeDictionary CreateAttributeDictionary(string siteMapNodeKey, ISiteMap siteMap, ILocalizationService localizationService);

        [Obsolete("Use the overload that accepts a siteMapNodeKey instead. This overload will be removed in version 5.")]
        IAttributeDictionary CreateAttributeDictionary(ISiteMap siteMap, ILocalizationService localizationService);

        IRouteValueDictionary CreateRouteValueDictionary(string siteMapNodeKey, ISiteMap siteMap);

        [Obsolete("Use the overload that accepts a siteMapNodeKey instead. This overload will be removed in version 5.")]
        IRouteValueDictionary CreateRouteValueDictionary(ISiteMap siteMap);

        IPreservedRouteParameterCollection CreatePreservedRouteParameterCollection(ISiteMap siteMap);
        IRoleCollection CreateRoleCollection(ISiteMap siteMap);
        IMetaRobotsValueCollection CreateMetaRobotsValueCollection(ISiteMap siteMap);
    }
}
