using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for dynamic node parent map builder.
    /// </summary>
    public interface IDynamicSiteMapNodeBuilder
    {
        IEnumerable<ISiteMapNodeParentMap> BuildDynamicNodes(ISiteMapNode node, string defaultParentKey);
    }
}
