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
        IAttributeDictionary CreateAttributeDictionary(ISiteMap siteMap, ILocalizationService localizationService);
        IRouteValueDictionary CreateRouteValueDictionary(ISiteMap siteMap);
        IPreservedRouteParameterCollection CreatePreservedRouteParameterCollection(ISiteMap siteMap);
        IRoleCollection CreateRoleCollection(ISiteMap siteMap);
        IMetaRobotsValueCollection CreateMetaRobotsValueCollection(ISiteMap siteMap);
    }
}
