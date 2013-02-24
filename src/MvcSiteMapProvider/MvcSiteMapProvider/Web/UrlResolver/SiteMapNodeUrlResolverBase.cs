using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Reflection;

namespace MvcSiteMapProvider.Web.UrlResolver
{
    /// <summary>
    /// Base class to make it easier to implement a custom Url Resolver.
    /// </summary>
    public abstract class SiteMapNodeUrlResolverBase
        : ISiteMapNodeUrlResolver
    {
        #region ISiteMapNodeUrlResolver Members

        /// <summary>
        /// Resolves the URL. Override this member to provide alternate implementations of UrlResolver.
        /// </summary>
        /// <param name="siteMapNode">The site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        public abstract string ResolveUrl(ISiteMapNode siteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues);


        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the URL resolver. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// <c>true</c> if the provider name matches; otherwise <c>false</c>.
        /// </returns>
        public virtual bool AppliesTo(string providerName)
        {
            return this.GetType().ShortAssemblyQualifiedName().Equals(providerName, StringComparison.InvariantCulture);
        }

        #endregion
    }
}
