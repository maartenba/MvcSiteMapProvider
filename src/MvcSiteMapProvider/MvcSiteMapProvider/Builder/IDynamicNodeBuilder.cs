using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for dynamic node builder.
    /// </summary>
    public interface IDynamicNodeBuilder
    {
        //bool HasDynamicNodes(ISiteMapNode node);
        IEnumerable<ISiteMapNode> BuildDynamicNodesFor(ISiteMap siteMap, ISiteMapNode node, ISiteMapNode parentNode);
    }
}
