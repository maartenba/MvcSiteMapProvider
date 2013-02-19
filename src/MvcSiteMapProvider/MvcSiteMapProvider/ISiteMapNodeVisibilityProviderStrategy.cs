using System;
using System.Collections.Generic;
using System.Web;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Provides a means to yield control of the lifetime of provider
    /// instances to the DI container, while allowing the right instance
    /// for the job to be selected at runtime.
    /// 
    /// http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern#1501517
    /// </summary>
    public interface ISiteMapNodeVisibilityProviderStrategy
    {
        ISiteMapNodeVisibilityProvider GetProvider(string providerName);

        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        bool IsVisible(string providerName, ISiteMapNode node, IDictionary<string, object> sourceMetadata);
    }
}
