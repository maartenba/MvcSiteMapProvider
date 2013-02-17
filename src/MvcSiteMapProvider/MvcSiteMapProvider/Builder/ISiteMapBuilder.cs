using System;
using System.Collections.Generic;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Implement this interface to make a custom implementation that will build a sitemap.
    /// </summary>
    public interface ISiteMapBuilder
    {
        IEnumerable<string> GetDependencyFileNames();
        ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode);
    }
}
