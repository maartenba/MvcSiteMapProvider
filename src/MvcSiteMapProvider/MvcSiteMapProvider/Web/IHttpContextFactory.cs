using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Contract for an abstract factory that provides context-related instances at runtime.
    /// </summary>
    public interface IHttpContextFactory
    {
        HttpContextBase Create();
        RequestContext CreateRequestContext(RouteData routeData);
        RequestContext CreateRequestContext();
        ControllerContext CreateControllerContext(RequestContext requestContext, ControllerBase controller);
        IRequestCache GetRequestCache();
        IUrlHelper CreateUrlHelper(RequestContext requestContext);
        IUrlHelper CreateUrlHelper();
    }
}
