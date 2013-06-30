using System;
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
        RequestContext CreateRequestContext(ISiteMapNode node, RouteData routeData);
        RequestContext CreateRequestContext();
        ControllerContext CreateControllerContext(RequestContext requestContext, ControllerBase controller);
        IRequestCache GetRequestCache();
        RouteCollection GetRoutes();
        IUrlHelper CreateUrlHelper(RequestContext requestContext);
        IUrlHelper CreateUrlHelper();
    }
}
