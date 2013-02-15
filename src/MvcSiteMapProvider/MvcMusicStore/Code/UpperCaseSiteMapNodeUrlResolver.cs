using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider.Web.UrlResolver;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web;

namespace MvcMusicStore.Code
{
    /// <summary>
    /// Sample ISiteMapNodeUrlResolver implementation.
    /// </summary>
    public class UpperCaseSiteMapNodeUrlResolver
        : SiteMapNodeUrlResolver
    {
        public UpperCaseSiteMapNodeUrlResolver(
            IHttpContextFactory httpContextFactory
            ) 
            : base(httpContextFactory)
        {
        }

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="mvcSiteMapNode">The MVC site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        public override string ResolveUrl(ISiteMapNode mvcSiteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            return base.ResolveUrl(mvcSiteMapNode, area, controller, action, routeValues).ToUpperInvariant();
        }
    }
}