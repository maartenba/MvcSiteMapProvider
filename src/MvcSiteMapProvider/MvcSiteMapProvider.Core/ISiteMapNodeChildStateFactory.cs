using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Core.Globalization;
using MvcSiteMapProvider.Core.Collections;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNodeChildStateFactory
    {
        ILocalizationService CreateLocalizationService(string implicitResourceKey);
        IAttributeCollection CreateAttributeCollection(ISiteMap siteMap, ILocalizationService localizationService);
        IRouteValueCollection CreateRouteValueCollection(ISiteMap siteMap);
        IList<string> CreatePreservedRouteParameterCollection(ISiteMap siteMap);
        IList<string> CreateRoleCollection(ISiteMap siteMap);
    }
}
