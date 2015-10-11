using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Removes non-clickable nodes that have no accessible and/or visible children.
    /// </summary>
    public class TrimEmptyGroupingNodesVisibilityProvider
        : SiteMapNodeVisibilityProviderBase
    {
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            // Optimization - return quickly if clickable.
            if (node.Clickable)
            {
                return true;
            }

            var childNodes = node.ChildNodes;
            return childNodes == null || childNodes.Any(c => c.IsVisible(sourceMetadata));
        }
    }
}
