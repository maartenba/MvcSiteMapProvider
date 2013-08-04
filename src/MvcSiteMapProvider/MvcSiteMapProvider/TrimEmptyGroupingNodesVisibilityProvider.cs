using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
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
