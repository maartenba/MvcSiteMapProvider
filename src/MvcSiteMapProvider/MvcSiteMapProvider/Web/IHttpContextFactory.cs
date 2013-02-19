using System;
using System.Web;
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
        IRequestCache GetRequestCache();
    }
}
