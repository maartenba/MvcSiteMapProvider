using System;

namespace MvcSiteMapProvider.Web
{
    public interface IUrlHelper
    {
        string Action(string actionName);
        string Action(string actionName, object routeValues);
        string Action(string actionName, string controllerName);
        string Action(string actionName, string controllerName, object routeValues);
        string Action(string actionName, string controllerName, object routeValues, string protocol);
        string Action(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues);
        string Action(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, string protocol, string hostName);
        string Action(string actionName, System.Web.Routing.RouteValueDictionary routeValues);
        string Content(string contentPath);
        string Encode(string url);
        string HttpRouteUrl(string routeName, object routeValues);
        string HttpRouteUrl(string routeName, System.Web.Routing.RouteValueDictionary routeValues);
        bool IsLocalUrl(string url);
        System.Web.Routing.RequestContext RequestContext { get; }
        System.Web.Routing.RouteCollection RouteCollection { get; }
        string RouteUrl(object routeValues);
        string RouteUrl(string routeName);
        string RouteUrl(string routeName, object routeValues);
        string RouteUrl(string routeName, object routeValues, string protocol);
        string RouteUrl(string routeName, System.Web.Routing.RouteValueDictionary routeValues);
        string RouteUrl(string routeName, System.Web.Routing.RouteValueDictionary routeValues, string protocol, string hostName);
        string RouteUrl(System.Web.Routing.RouteValueDictionary routeValues);
    }
}
