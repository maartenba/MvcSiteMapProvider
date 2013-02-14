using System;
using System.Web;
using System.Web.Routing;

using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Web
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HttpContextFactory
        : IHttpContextFactory
    {
        //public HttpContextFactory(
        //    HttpContext context
        //    )
        //{
        //    if (context == null)
        //        throw new ArgumentNullException("context");
        //    this.context = context;
        //}

        //protected readonly HttpContext context;

        #region IHttpContextFactory Members

        public HttpContextBase Create()
        {
            // TODO: Change this to HttpContextWrapper to make it generic to pass around and add
            // methods here to wrap in specified classes when needed.
            //return new MvcHttpContext(context);

            return new MvcHttpContext(HttpContext.Current);
        }

        public RequestContext CreateRequestContext(RouteData routeData)
        {
            var context = this.Create();
            return new RequestContext(context, routeData);
        }

        #endregion
    }
}
