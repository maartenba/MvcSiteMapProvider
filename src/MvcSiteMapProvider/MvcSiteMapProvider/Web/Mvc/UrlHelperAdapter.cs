using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Adapter for the <see cref="T:System.Web.Mvc.UrlHelper"/> class so a test double can be passed between methods.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class UrlHelperAdapter
        : UrlHelper, IUrlHelper
    {
        private UrlHelperAdapter(RequestContext requestContext)
            : base(requestContext)
        {
        }

        public UrlHelperAdapter(RequestContext requestContext, RouteCollection routeCollection)
            : base(requestContext, routeCollection)
        {
        }
    }
}
