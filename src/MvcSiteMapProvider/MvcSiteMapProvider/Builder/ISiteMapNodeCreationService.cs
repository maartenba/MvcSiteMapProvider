using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for a set of services useful for creating SiteMap nodes.
    /// </summary>
    public interface ISiteMapNodeCreationService
    {
        ISiteMapNode CreateSiteMapNode(ISiteMap siteMap, string key, string implicitResourceKey);
        ISiteMapNode CreateDynamicSiteMapNode(ISiteMap siteMap, string key, string implicitResourceKey);
        string GenerateSiteMapNodeKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable);
        ISiteMapNodeParentMap CreateSiteMapNodeParentMap(string parentKey, ISiteMapNode node, string sourceName);
    }
}
