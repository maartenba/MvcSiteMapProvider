using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for dynamic node builder.
    /// </summary>
    [Obsolete("Use IDynamicNodeParentMapBuilder instead. IDynamicNodeBuilder will be removed in version 5.")]
    public interface IDynamicNodeBuilder
    {
        IEnumerable<ISiteMapNode> BuildDynamicNodesFor(ISiteMap siteMap, ISiteMapNode node, ISiteMapNode parentNode);
    }
}
