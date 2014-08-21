using System;
using System.Web;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// ControllerExtensions
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Gets the current site map node.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static ISiteMapNode GetCurrentSiteMapNode(this ControllerBase controller)
        {
            return GetCurrentSiteMapNode(controller, SiteMaps.Current);
        }

        /// <summary>
        /// Gets the current site map node.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="siteMap">The site map.</param>
        /// <returns></returns>
        public static ISiteMapNode GetCurrentSiteMapNode(this ControllerBase controller, ISiteMap siteMap)
        {
            return siteMap.CurrentNode;
        }

        /// <summary>
        /// Gets the current site map node for child action.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static ISiteMapNode GetCurrentSiteMapNodeForChildAction(this ControllerBase controller)
        {
            return GetCurrentSiteMapNodeForChildAction(controller, SiteMaps.Current);
        }

        /// <summary>
        /// Gets the current site map node for child action.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="siteMap">The SiteMap.</param>
        /// <returns></returns>
        public static ISiteMapNode GetCurrentSiteMapNodeForChildAction(this ControllerBase controller, ISiteMap siteMap)
        {
            return siteMap.FindSiteMapNode(controller.ControllerContext);
        }
    }
}
