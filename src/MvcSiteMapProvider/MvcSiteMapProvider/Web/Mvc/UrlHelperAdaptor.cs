using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Wraps the <see cref="T:System.Web.Mvc.UrlHelper"/> class so a test double can be passed between methods.
    /// </summary>
    public class UrlHelperAdaptor 
        : UrlHelper, IUrlHelper
    {
        public UrlHelperAdaptor(RequestContext requestContext)
            : base(requestContext)
        {
        }

        private UrlHelperAdaptor(RequestContext requestContext, RouteCollection routeCollection)
            : base(requestContext, routeCollection)
        {
        }
    }
}
