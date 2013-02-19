using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HttpContextFactory
        : IHttpContextFactory
    {
        //public HttpContextFactory(
        //    IRequestCache requestCache
        //    )
        //{
        //    if (requestCache == null)
        //        throw new ArgumentNullException("requestCache");
        //    this.requestCache = requestCache;
        //}

        protected readonly IRequestCache requestCache;

        #region IHttpContextFactory Members

        public virtual HttpContextBase Create()
        {
            // TODO: Change this to HttpContextWrapper to make it generic to pass around and add
            // methods here to wrap in specified classes when needed.
            //return new MvcHttpContext(context);

            return new MvcHttpContext(HttpContext.Current);
        }

        public virtual RequestContext CreateRequestContext(RouteData routeData)
        {
            var httpContext = this.Create();
            return new RequestContext(httpContext, routeData);

            //if (httpContext.Handler is MvcHandler)
            //    return ((MvcHandler)httpContext.Handler).RequestContext;
            //else
            //    return new RequestContext(httpContext, new RouteData());
        }

        public virtual IRequestCache GetRequestCache()
        {
            return new RequestCache(this);
        }

        #endregion
    }
}
