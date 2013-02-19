using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

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
