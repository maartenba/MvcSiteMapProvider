using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Web.UrlResolver
{
    /// <summary>
    /// SiteMapNode URL resolver contract.
    /// </summary>
    public interface ISiteMapNodeUrlResolver
    {
        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="siteMapNode">The site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        string ResolveUrl(ISiteMapNode siteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues);

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        bool AppliesTo(string providerName);
    }
}
