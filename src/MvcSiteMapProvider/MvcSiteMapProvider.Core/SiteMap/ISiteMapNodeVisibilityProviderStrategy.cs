// -----------------------------------------------------------------------
// <copyright file="ISiteMapNodeVisibilityProviderStrategy.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNodeVisibilityProviderStrategy
    {
        ISiteMapNodeVisibilityProvider GetProvider(string providerName);

        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="context">The context.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        bool IsVisible(string providerName, ISiteMapNode node, HttpContext context, IDictionary<string, object> sourceMetadata);
    }
}
