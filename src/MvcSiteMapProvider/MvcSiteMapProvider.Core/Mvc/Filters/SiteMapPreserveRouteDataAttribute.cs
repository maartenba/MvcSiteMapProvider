using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider.Core;

namespace MvcSiteMapProvider.Core.Mvc.Filters
{
    /// <summary>
    /// Apply this attribute to keep route data when rendering the sitemap (e.g. breadcrumbs).
    /// Note: Do NOT use this in conjunction with dynamic node providers!
    /// </summary>
    public class SiteMapPreserveRouteDataAttribute 
        : ActionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapPreserveRouteDataAttribute"/> class.
        /// </summary>
        public SiteMapPreserveRouteDataAttribute()
        {
            Target = AttributeTarget.CurrentNode;
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public AttributeTarget Target { get; set; }

        /// <summary>
        /// Ensures all routedata elements are included on the node whenever the mvc action is invoked.
        /// This allows the MVC site map to have route values preserved for breadcrumb trails.
        /// </summary>
        /// <param name="filterContext">The current filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var currentSiteMap = SiteMaps.Current;
            var node = currentSiteMap.CurrentNode;
            if (Target == AttributeTarget.ParentNode)
            {
                node = currentSiteMap.CurrentNode.ParentNode;
            }

            //var node = SiteMap.CurrentNode as MvcSiteMapNode;
            //if (Target == AttributeTarget.ParentNode)
            //{
            //    node = SiteMap.CurrentNode.ParentNode as MvcSiteMapNode;
            //}
            if (node != null)
            {
                foreach (var routeitem in filterContext.RouteData.Values)
                {
                    node.RouteValues[routeitem.Key] = routeitem.Value;
                }
            }
        }
    }
}
