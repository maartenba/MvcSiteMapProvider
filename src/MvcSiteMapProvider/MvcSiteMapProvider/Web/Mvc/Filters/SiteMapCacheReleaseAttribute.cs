using System;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc.Filters
{
    /// <summary>
    /// Releases the cache for the specified SiteMapCacheKey, or default site map for the current request if not supplied.
    /// Can be used on the CRUD controller action methods to force the sitemap to be rebuilt with the updated data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SiteMapCacheReleaseAttribute
        : ActionFilterAttribute
    {
        /// <summary>
        /// Cache key for the sitemap instance this attribute applies to.
        /// If not supplied, the default SiteMap instance for this request will be used.
        /// </summary>
        public string SiteMapCacheKey { get; set; }

        /// <summary>
        /// Called by the MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            SiteMaps.ReleaseSiteMap(this.SiteMapCacheKey);
        }
    }
}
