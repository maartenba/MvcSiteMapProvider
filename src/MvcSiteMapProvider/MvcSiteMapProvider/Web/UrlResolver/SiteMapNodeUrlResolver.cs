using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Web.UrlResolver
{
    /// <summary>
    /// Default SiteMapNode URL resolver.
    /// </summary>
    public class SiteMapNodeUrlResolver
        : SiteMapNodeUrlResolverBase
    {
        public SiteMapNodeUrlResolver(
            IMvcContextFactory mvcContextFactory,
            IUrlPath urlPath
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");

            this.mvcContextFactory = mvcContextFactory;
            this.urlPath = urlPath;
        }

        protected readonly IMvcContextFactory mvcContextFactory;
        protected readonly IUrlPath urlPath;

        #region ISiteMapNodeUrlResolver Members

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="node">The MVC site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        public override string ResolveUrl(ISiteMapNode node, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            if (!String.IsNullOrEmpty(node.UnresolvedUrl))
            {
                return this.ResolveVirtualPath(node);
            }
            return this.ResolveRouteUrl(node, area, controller, action, routeValues);
        }

        #endregion

        protected virtual string ResolveVirtualPath(ISiteMapNode node)
        {
            var url = node.UnresolvedUrl;
            if (!urlPath.IsAbsoluteUrl(url))
            {
                return urlPath.MakeVirtualPathAppAbsolute(urlPath.Combine(urlPath.AppDomainAppVirtualPath, url));
            }
            return url;
        }

        protected virtual string ResolveRouteUrl(ISiteMapNode node, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            string result = String.Empty;
            var urlHelper = mvcContextFactory.CreateUrlHelper();
            var routeValueDictionary = new RouteValueDictionary(routeValues);

            if (!string.IsNullOrEmpty(node.Route))
            {
                routeValueDictionary.Remove("route");
                result = urlHelper.RouteUrl(node.Route, routeValueDictionary);
            }
            else
            {
                result = urlHelper.Action(action, controller, routeValueDictionary);
            }

            if (string.IsNullOrEmpty(result))
            {
                // fixes #115 - UrlResolver should not throw exception.
                return urlPath.MakeVirtualPathAppAbsolute("~") + Guid.NewGuid().ToString();
            }

            return result;
        }
    }
}
