using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Core;

namespace MvcSiteMapProvider.Core.Mvc.UrlResolver
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNodeUrlResolverStrategy
    {
        ISiteMapNodeUrlResolver GetProvider(string providerName);

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="providerName">The name or type of the provider.</param>
        /// <param name="mvcSiteMapNode">The MVC site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveUrl(string providerName, ISiteMapNode mvcSiteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues);
    }
}
