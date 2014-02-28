using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// Contract for an abstract factory that provides context-related instances at runtime.
    /// </summary>
    public interface IMvcContextFactory
    {
        HttpContextBase CreateHttpContext();
        HttpContextBase CreateHttpContext(ISiteMapNode node, Uri uri, TextWriter writer);
        RequestContext CreateRequestContext(ISiteMapNode node, RouteData routeData);
        RequestContext CreateRequestContext();
        RequestContext CreateRequestContext(HttpContextBase httpContext);
        RequestContext CreateRequestContext(HttpContextBase httpContext, RouteData routeData);
        ControllerContext CreateControllerContext(RequestContext requestContext, ControllerBase controller);
        IRequestCache GetRequestCache();
        RouteCollection GetRoutes();
        IUrlHelper CreateUrlHelper(RequestContext requestContext);
        IUrlHelper CreateUrlHelper();
        AuthorizationContext CreateAuthorizationContext(ControllerContext controllerContext, ActionDescriptor actionDescriptor);
    }
}
