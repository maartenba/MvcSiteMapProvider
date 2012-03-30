using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Extensibility;
using MvcSiteMapProvider.Resources;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Default SiteMapNode URL resolver.
    /// </summary>
    public class DefaultSiteMapNodeUrlResolver
        : ISiteMapNodeUrlResolver
    {
        /// <summary>
        /// UrlHelperCacheKey
        /// </summary>
        private const string UrlHelperCacheKey = "6F0F34DE-2981-454E-888D-28080283EF65";

        /// <summary>
        /// Gets the URL helper.
        /// </summary>
        /// <value>The URL helper.</value>
        protected UrlHelper UrlHelper
        {
            get
            {
                if (HttpContext.Current.Items[UrlHelperCacheKey] == null)
                {
                    RequestContext ctx;
                    if (HttpContext.Current.Handler is MvcHandler)
                        ctx = ((MvcHandler)HttpContext.Current.Handler).RequestContext;
                    else
                        ctx = new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData());

                    HttpContext.Current.Items[UrlHelperCacheKey] = new UrlHelper(ctx);
                }
                return (UrlHelper)HttpContext.Current.Items[UrlHelperCacheKey];
            }
        }

        #region ISiteMapNodeUrlResolver Members

        private string _urlkey = string.Empty;
        private string _url = string.Empty;

        /// <summary>
        /// Resolves the URL.
        /// </summary>
        /// <param name="mvcSiteMapNode">The MVC site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        public virtual string ResolveUrl(MvcSiteMapNode mvcSiteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            if (mvcSiteMapNode["url"] != null)
            {
                if (mvcSiteMapNode["url"].StartsWith("~"))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute(mvcSiteMapNode["url"]);
                }
                else
                {
                    return mvcSiteMapNode["url"];
                }
            }

            if (!string.IsNullOrEmpty(mvcSiteMapNode.PreservedRouteParameters))
            {
                var routeDataValues = UrlHelper.RequestContext.RouteData.Values;
                var queryStringValues = UrlHelper.RequestContext.HttpContext.Request.QueryString;
                foreach (var item in mvcSiteMapNode.PreservedRouteParameters.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
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
            var key = mvcSiteMapNode.Route ?? string.Empty;
            foreach (var routeValue in routeValues)
                key += routeValue.Key + (routeValue.Value ?? string.Empty);
            if (_urlkey == key) return _url;

            string returnValue = null;
            if (!string.IsNullOrEmpty(mvcSiteMapNode.Route))
            {
                returnValue = UrlHelper.RouteUrl(mvcSiteMapNode.Route, new RouteValueDictionary(routeValues));
            }
            else
            {
                returnValue = UrlHelper.Action(action, controller, new RouteValueDictionary(routeValues));
            }

            if (string.IsNullOrEmpty(returnValue))
            {
                throw new UrlResolverException(string.Format(Messages.CouldNotResolve, mvcSiteMapNode.Title, action, controller, mvcSiteMapNode.Route ?? ""));
            }

            _urlkey = key;
            _url = returnValue;
            return returnValue;
        }

        #endregion
    }
}
