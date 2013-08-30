using System;
using System.Web;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// HttpResponse wrapper.
    /// </summary>
    public class SiteMapHttpResponse
        : HttpResponseWrapper
    {
        public SiteMapHttpResponse(HttpResponse httpResponse)
            : base(httpResponse)
        {
        }
        
        public override HttpCachePolicyBase Cache
        {
            get { return new SiteMapHttpResponseCache(); }
        }
    }
}
