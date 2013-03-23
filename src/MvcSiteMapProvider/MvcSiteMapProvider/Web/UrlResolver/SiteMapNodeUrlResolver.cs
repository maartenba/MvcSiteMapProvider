using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Gets the URL helper.
        /// </summary>
        /// <value>The URL helper.</value>
        protected IUrlHelper UrlHelper
        {
            get
            {
                var key = "6F0F34DE-2981-454E-888D-28080283EF65";
                var requestCache = mvcContextFactory.GetRequestCache();
                var result = requestCache.GetValue<IUrlHelper>(key);
                if (result == null)
                {
                    result = mvcContextFactory.CreateUrlHelper();
                    requestCache.SetValue<IUrlHelper>(key, result);
                }
                return result;
            }
        } 

        #region ISiteMapNodeUrlResolver Members

        private string _urlkey = string.Empty;
        private string _url = string.Empty;

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
                    return System.Web.VirtualPathUtility.ToAbsolute(node.UnresolvedUrl);
                }
                else
                {
                    return node.UnresolvedUrl;
                }
            }

            if (node.PreservedRouteParameters.Count > 0)
            {
                var routeDataValues = UrlHelper.RequestContext.RouteData.Values;
                var queryStringValues = UrlHelper.RequestContext.HttpContext.Request.QueryString;
                foreach (var item in node.PreservedRouteParameters)
                {
                    var preservedParameterName = item.Trim();
                    if (!string.IsNullOrEmpty(preservedParameterName))
                    {
                        if (routeDataValues.ContainsKey(preservedParameterName))
                        {
                            routeValues[preservedParameterName] =
                                routeDataValues[preservedParameterName];
                        }
                        else if (queryStringValues[preservedParameterName] != null)
                        {
                            routeValues[preservedParameterName] =
                                queryStringValues[preservedParameterName];
                        }
                    }
                }
            }

            //cache already generated routes. 
            //I don't know why the result of Url was not saved to this["url"], perhaps because
            //theoretically it is possible to change RouteValues dynamically. So I decided to 
            //store last version
            var key = node.Route ?? string.Empty;
            foreach (var routeValue in routeValues)
                key += routeValue.Key + (routeValue.Value ?? string.Empty);
            if (_urlkey == key) return _url;

            string returnValue;
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            if (!string.IsNullOrEmpty(node.Route))
            {
                routeValueDictionary.Remove("route");
                returnValue = UrlHelper.RouteUrl(node.Route, routeValueDictionary);
            }
            else
            {
                returnValue = UrlHelper.Action(action, controller, routeValueDictionary);
            }

            if (string.IsNullOrEmpty(returnValue))
            {
                // fixes #115 - UrlResolver should not throw exception.
                return Guid.NewGuid().ToString();
            }
            else
            {
                _urlkey = key;
                _url = returnValue;
                return returnValue;
            }
        }

        #endregion
    }
}
