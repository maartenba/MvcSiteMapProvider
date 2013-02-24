using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of types required by the <see cref="T:MvcSiteMapProvider.SiteMapNode"/>
    /// at runtime.
    /// </summary>
    public interface ISiteMapNodeChildStateFactory
    {
        IAttributeCollection CreateAttributeCollection(ISiteMap siteMap, ILocalizationService localizationService);
        IRouteValueCollection CreateRouteValueCollection(ISiteMap siteMap);
        IPreservedRouteParameterCollection CreatePreservedRouteParameterCollection(ISiteMap siteMap);
        IRoleCollection CreateRoleCollection(ISiteMap siteMap);
        IMetaRobotsValueCollection CreateMetaRobotsValueCollection(ISiteMap siteMap);
    }
}
