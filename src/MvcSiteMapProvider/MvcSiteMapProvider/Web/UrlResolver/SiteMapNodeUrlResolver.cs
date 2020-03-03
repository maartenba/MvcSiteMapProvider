﻿using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Routing;

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
            if (!string.IsNullOrEmpty(node.UnresolvedUrl))
            {
                return this.ResolveVirtualPath(node);
            }
            return this.ResolveRouteUrl(node, area, controller, action, routeValues);
        }

        #endregion

        protected virtual string ResolveVirtualPath(ISiteMapNode node)
        {
            return this.urlPath.ResolveUrl(node.UnresolvedUrl, node.Protocol, node.HostName);
        }

        protected virtual string ResolveRouteUrl(ISiteMapNode node, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            string result = string.Empty;
            // Create a TextWriter with null stream as a backing stream 
            // which doesn't consume resources
            using (var nullWriter = new StreamWriter(Stream.Null))
            {
                var requestContext = this.CreateRequestContext(node, nullWriter);
                result = this.ResolveRouteUrl(node, area, controller, action, routeValues, requestContext);
            }

            if (string.IsNullOrEmpty(result))
            {
                // fixes #115 - UrlResolver should not throw exception.
                result = "#";
            }

            return result;
        }

        protected virtual string ResolveRouteUrl(ISiteMapNode node, string area, string controller, string action, IDictionary<string, object> routeValues, RequestContext requestContext)
        {
            string result = string.Empty;
            var urlHelper = this.mvcContextFactory.CreateUrlHelper(requestContext);
            var routeValueDictionary = new RouteValueDictionary(routeValues);

            if (!string.IsNullOrEmpty(node.Route))
            {
                // (comment copied from MVC source code)
                // We only include MVC-specific values like 'controller' and 'action' if we are generating an action link.
                // If we are generating a route link [as to MapRoute("Foo", "any/url", new { controller = ... })], including
                // the current controller name will cause the route match to fail if the current controller is not the same
                // as the destination controller.
                routeValueDictionary.Remove("route");
                // However, removing controller and action potentially have the unintended side-effect of preventing 
                // routes from matching when the route depends on either to satisfy a constraint. 
                // It is possible that these values should NEVER be removed to allow proper construction of a
                // named route without defaults or with unspecified tokens.
                //routeValueDictionary.Remove("controller")
                //routeValueDictionary.Remove("action")
                var route = urlHelper.RouteCollection[node.Route] as Route;
                if (!(route?.Constraints.ContainsKey("controller") ?? false)) routeValueDictionary.Remove("controller");
                if (!(route?.Constraints.ContainsKey("action") ?? false)) routeValueDictionary.Remove("action");
                result = urlHelper.RouteUrl(node.Route, routeValueDictionary);
            }
            else
            {
                result = urlHelper.Action(action, controller, routeValueDictionary);
            }

            if (!string.IsNullOrEmpty(result))
            {
                // NOTE: We are purposely using the current request's context when resolving any absolute 
                // URL so it will read the headers and use the HTTP_HOST of the client machine, if included,
                // in the case where HostName is null or empty and protocol is not. The public facing port is
                // also used when the protocol matches.
                return this.urlPath.ResolveUrl(result, node.Protocol, node.HostName);
            }

            return result;
        }

        protected virtual HttpContextBase CreateHttpContext(ISiteMapNode node, TextWriter writer)
        {
            var currentHttpContext = this.mvcContextFactory.CreateHttpContext();

            // Create a URI with the home page and no query string values.
            var uri = new Uri(currentHttpContext.Request.Url, "/");
            return this.mvcContextFactory.CreateHttpContext(node, uri, writer);
        }

        protected virtual RequestContext CreateRequestContext(ISiteMapNode node, TextWriter writer)
        {
            var realRequestContext = this.mvcContextFactory.CreateRequestContext();

            // For interop with MvcCodeRouting, we need to build a data token with the appropriate value.
            realRequestContext.RouteData.SetMvcCodeRoutingContext(node);

            if (!node.IncludeAmbientValuesInUrl)
            {
                var httpContext = this.CreateHttpContext(node, writer);
                var fakeRequestContext = this.mvcContextFactory.CreateRequestContext(httpContext);
                fakeRequestContext.RouteData.MergeDataTokens(realRequestContext.RouteData.DataTokens);
                fakeRequestContext.RouteData.Route = realRequestContext.RouteData.Route;
                fakeRequestContext.RouteData.RouteHandler = realRequestContext.RouteData.RouteHandler;
                return fakeRequestContext;
            }

            return realRequestContext;
        }
    }
}
