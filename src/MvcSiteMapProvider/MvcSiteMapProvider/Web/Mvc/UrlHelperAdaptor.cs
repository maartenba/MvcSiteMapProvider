using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Adapter for the <see cref="T:System.Web.Mvc.UrlHelper"/> class so a test double can be passed between methods.
    /// </summary>
    [Obsolete("Please use the UrlHelperAdapter (spelled with an e) instead. This class will be removed in version 5.")]
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
