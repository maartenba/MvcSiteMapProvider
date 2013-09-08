using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for a set of services useful for building a SiteMap.
    /// </summary>
    public interface ISiteMapAssemblyService
    {
        ISiteMapNode CreateSiteMapNode(ISiteMap siteMap, string key, string implicitResourceKey);
        ISiteMapNodeParentMap CreateSiteMapNodeParentMap(string parentKey, ISiteMapNode node, string sourceName);
        IEnumerable<ISiteMapNodeParentMap> BuildDynamicNodeParentMaps(ISiteMap siteMap, ISiteMapNode node, string parentKey);
        string GenerateSiteMapNodeKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable);
        IEnumerable<ISiteMapNodeParentMap> BuildHierarchy(ISiteMap siteMap, IEnumerable<ISiteMapNodeParentMap> nodes);
        string SiteMapCacheKey { get; }
    }
}
