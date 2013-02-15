using System;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// An abstract factory interface for creating sitemap nodes at runtime.
    /// </summary>
    public interface ISiteMapNodeFactory
    {
        ISiteMapNode Create(ISiteMap siteMap, string key, string implicitResourceKey);
        ISiteMapNode CreateDynamic(ISiteMap siteMap, string key, string implicitResourceKey);
    }
}
