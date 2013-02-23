using System;
using System.Web.Routing;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Contract for <see cref="T:System.Web.Mvc.UrlHelper"/> wrapper class.
    /// </summary>
    public interface IUrlHelper
    {
        string Action(string actionName);
        string Action(string actionName, object routeValues);
        string Action(string actionName, string controllerName);
        string Action(string actionName, string controllerName, object routeValues);
        string Action(string actionName, string controllerName, object routeValues, string protocol);
        string Content(string contentPath);
        string Encode(string url);
        string RouteUrl(object routeValues);
        string RouteUrl(string routeName);
        string RouteUrl(string routeName, object routeValues);
        string RouteUrl(string routeName, object routeValues, string protocol);

#if !MVC2
        string Action(string actionName, string controllerName, RouteValueDictionary routeValues);
        string Action(string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName);
        string Action(string actionName, RouteValueDictionary routeValues);
        bool IsLocalUrl(string url);
        RequestContext RequestContext { get; }
        RouteCollection RouteCollection { get; }
        string RouteUrl(string routeName, RouteValueDictionary routeValues);
        string RouteUrl(string routeName, RouteValueDictionary routeValues, string protocol, string hostName);
        string RouteUrl(RouteValueDictionary routeValues);
#endif
#if MVC4
        string HttpRouteUrl(string routeName, object routeValues);
        string HttpRouteUrl(string routeName, RouteValueDictionary routeValues);
#endif

    }
}
