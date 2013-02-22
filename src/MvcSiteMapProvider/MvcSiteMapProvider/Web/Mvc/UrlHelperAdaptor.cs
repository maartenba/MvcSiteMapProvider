using System;
//using System.Globalization;
//using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
//using System.Web.Mvc.Properties;
using System.Web.Routing;
//using System.Web.WebPages;

namespace MvcSiteMapProvider.Web
{
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
 
        //public UrlHelperAdaptor(UrlHelper helper)
        //    : base(helper.RequestContext, helper.RouteCollection)
        //{
        //}
    }
}
