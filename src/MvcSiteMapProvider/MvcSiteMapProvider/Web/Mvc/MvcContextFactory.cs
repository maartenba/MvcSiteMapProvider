using System;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of MVC
    /// context-related instances at runtime.
    /// </summary>
    public class MvcContextFactory
        : IMvcContextFactory
    {
        #region IMvcContextFactory Members

        public virtual HttpContextBase CreateHttpContext()
        {
            return CreateHttpContext(null);
        }

        protected virtual HttpContextBase CreateHttpContext(ISiteMapNode node)
        {
            return new SiteMapHttpContext(HttpContext.Current, node);
        }

        public virtual RequestContext CreateRequestContext(ISiteMapNode node, RouteData routeData)
        {
            var httpContext = this.CreateHttpContext(node);
            return new RequestContext(httpContext, routeData);
        }

        public virtual RequestContext CreateRequestContext()
        {
            var httpContext = this.CreateHttpContext();
            if (httpContext.Handler is MvcHandler)
                return ((MvcHandler)httpContext.Handler).RequestContext;
#if !NET35
            else if (httpContext.Handler is Page) // Fixes #15 for interop with ASP.NET Webforms
                return new RequestContext(httpContext, ((Page)HttpContext.Current.Handler).RouteData);
#endif
            else
                return new RequestContext(httpContext, new RouteData());
        }

        public virtual ControllerContext CreateControllerContext(RequestContext requestContext, ControllerBase controller)
        {
            return new ControllerContext(requestContext, controller);
        }

        public virtual IRequestCache GetRequestCache()
        {
            return new RequestCache(this);
        }

        public virtual RouteCollection GetRoutes()
        {
            return RouteTable.Routes;
        }

        public virtual IUrlHelper CreateUrlHelper(RequestContext requestContext)
        {
            return new UrlHelperAdapter(requestContext);
        }

        public virtual IUrlHelper CreateUrlHelper()
        {
            var requestContext = this.CreateRequestContext();
            return new UrlHelperAdapter(requestContext);
        }

        public virtual AuthorizationContext CreateAuthorizationContext(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return new AuthorizationContext(controllerContext, actionDescriptor);
        }

        #endregion
    }
}
