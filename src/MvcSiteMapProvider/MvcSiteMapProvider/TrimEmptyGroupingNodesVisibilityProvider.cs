using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Removes non-clickable nodes that have no accessible children.
    /// </summary>
    public class TrimEmptyGroupingNodesVisibilityProvider
        : SiteMapNodeVisibilityProviderBase
    {
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            if (!node.HasChildNodes && !node.Clickable)
            {
                return false;
            }
            return true;
        }
    }
}
