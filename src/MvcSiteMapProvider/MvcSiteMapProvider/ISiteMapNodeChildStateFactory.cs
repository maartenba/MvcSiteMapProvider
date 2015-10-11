using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of types required by the <see cref="T:MvcSiteMapProvider.SiteMapNode"/>
    /// at runtime.
    /// </summary>
    public interface ISiteMapNodeChildStateFactory
    {
        IAttributeDictionary CreateAttributeDictionary(string siteMapNodeKey, string memberName, ISiteMap siteMap, ILocalizationService localizationService);
        IRouteValueDictionary CreateRouteValueDictionary(string siteMapNodeKey, string memberName, ISiteMap siteMap);
        IPreservedRouteParameterCollection CreatePreservedRouteParameterCollection(ISiteMap siteMap);
        IRoleCollection CreateRoleCollection(ISiteMap siteMap);
        IMetaRobotsValueCollection CreateMetaRobotsValueCollection(ISiteMap siteMap);
    }
}
