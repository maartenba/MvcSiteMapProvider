using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web.UrlResolver
{
    /// <summary>
    /// Provides a means to yield control of the lifetime of provider
    /// instances to the DI container, while allowing the right instance
    /// for the job to be selected at runtime.
    /// 
    /// http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern#1501517
    /// </summary>
    public interface ISiteMapNodeUrlResolverStrategy
    {
        ISiteMapNodeUrlResolver GetProvider(string providerName);

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="providerName">The name or type of the provider.</param>
        /// <param name="node">The site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveUrl(string providerName, ISiteMapNode node, string area, string controller, string action, IDictionary<string, object> routeValues);
    }
}
