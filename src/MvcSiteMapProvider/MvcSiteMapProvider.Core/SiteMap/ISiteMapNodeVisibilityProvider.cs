#region Using directives

using System.Collections.Generic;
using System.Web;

#endregion

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// ISiteMapNode Visibility Provider contract.
    /// </summary>
    public interface ISiteMapNodeVisibilityProvider
    {
        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="context">The context.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        bool IsVisible(SiteMapNode node, HttpContext context, IDictionary<string, object> sourceMetadata);
    }
}
