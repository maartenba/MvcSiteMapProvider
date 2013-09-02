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
            IMvcContextFactory mvcContextFactory
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            this.mvcContextFactory = mvcContextFactory;
        }

        protected readonly IMvcContextFactory mvcContextFactory;

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
                if (node.UnresolvedUrl.StartsWith("~"))
                {
                    return VirtualPathUtility.ToAbsolute(node.UnresolvedUrl);
                }
                else
                {
                    return node.UnresolvedUrl;
                }
            }

            var urlHelper = mvcContextFactory.CreateUrlHelper();

            string returnValue;
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            if (!string.IsNullOrEmpty(node.Route))
            {
                routeValueDictionary.Remove("route");
                returnValue = urlHelper.RouteUrl(node.Route, routeValueDictionary);
            }
            else
            {
                returnValue = urlHelper.Action(action, controller, routeValueDictionary);
            }

            if (string.IsNullOrEmpty(returnValue))
            {
                // fixes #115 - UrlResolver should not throw exception.
                return VirtualPathUtility.ToAbsolute("~/") + Guid.NewGuid().ToString();
            }
            else
            {
                return returnValue;
            }
        }

        #endregion
    }
}
