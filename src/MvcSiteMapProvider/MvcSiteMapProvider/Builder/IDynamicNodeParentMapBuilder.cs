using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for dynamic node parent map builder.
    /// </summary>
    public interface IDynamicNodeParentMapBuilder
    {
        IEnumerable<ISiteMapNodeParentMap> BuildDynamicNodeParentMaps(ISiteMap siteMap, ISiteMapNode node, string parentKey);
    }
}
