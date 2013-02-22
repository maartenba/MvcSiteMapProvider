using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider.Web.UrlResolver
{
    /// <summary>
    /// Default SiteMapNode URL resolver.
    /// </summary>
    public class SiteMapNodeUrlResolver
        : ISiteMapNodeUrlResolver
    {
        public SiteMapNodeUrlResolver(
            IHttpContextFactory httpContextFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");
            this.httpContextFactory = httpContextFactory;
        }

        protected readonly IHttpContextFactory httpContextFactory;

        /// <summary>
        /// Gets the URL helper.
        /// </summary>
        /// <value>The URL helper.</value>
        protected IUrlHelper UrlHelper
        {
            get
            {
                var key = "6F0F34DE-2981-454E-888D-28080283EF65";
                var requestCache = httpContextFactory.GetRequestCache();
                var result = requestCache.GetValue<IUrlHelper>(key);
                if (result == null)
                {
                    result = httpContextFactory.CreateUrlHelper();
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
        /// <param name="mvcSiteMapNode">The MVC site map node.</param>
        /// <param name="area">The area.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The resolved URL.</returns>
        public virtual string ResolveUrl(ISiteMapNode siteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            if (!String.IsNullOrEmpty(siteMapNode.UnresolvedUrl))
            {
                if (siteMapNode.UnresolvedUrl.StartsWith("~"))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute(siteMapNode.UnresolvedUrl);
                }
                else
                {
                    return siteMapNode.UnresolvedUrl;
                }
            }

            if (siteMapNode.PreservedRouteParameters.Count > 0)
            {
                var routeDataValues = UrlHelper.RequestContext.RouteData.Values;
                var queryStringValues = UrlHelper.RequestContext.HttpContext.Request.QueryString;
                foreach (var item in siteMapNode.PreservedRouteParameters)
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
            var key = siteMapNode.Route ?? string.Empty;
            foreach (var routeValue in routeValues)
                key += routeValue.Key + (routeValue.Value ?? string.Empty);
            if (_urlkey == key) return _url;

            string returnValue;
            var routeValueDictionary = new RouteValueDictionary(routeValues);
            if (!string.IsNullOrEmpty(siteMapNode.Route))
            {
                routeValueDictionary.Remove("route");
                returnValue = UrlHelper.RouteUrl(siteMapNode.Route, routeValueDictionary);
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

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        public virtual bool AppliesTo(string providerName)
        {
            return this.GetType().ShortAssemblyQualifiedName().Equals(providerName);
        }

        #endregion
    }
}
