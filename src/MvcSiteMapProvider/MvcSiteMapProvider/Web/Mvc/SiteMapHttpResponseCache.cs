using System;
using System.Web;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// HttpResponseCache wrapper.
    /// </summary>
    public class SiteMapHttpResponseCache
        : HttpCachePolicyBase
    {
        public override void SetProxyMaxAge(TimeSpan delta)
        {
            // No implementation - skip this call when AuthorizeAttribute calls it
        }

        public override void AddValidationCallback(HttpCacheValidateHandler handler, object data)
        {
            // No implementation - skip this call when AuthorizeAttribute calls it
        }
    }
}
