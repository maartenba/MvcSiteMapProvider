using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcSiteMapProvider
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
        public static SiteMapNode GetCurrentSiteMapNode(this ControllerBase controller)
        {
            return GetCurrentSiteMapNode(controller, SiteMap.Provider);
        }

        /// <summary>
        /// Gets the current site map node.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static SiteMapNode GetCurrentSiteMapNode(this ControllerBase controller, SiteMapProvider provider)
        {
            return provider.CurrentNode;
        }

        /// <summary>
        /// Gets the current site map node for child action.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns></returns>
        public static SiteMapNode GetCurrentSiteMapNodeForChildAction(this ControllerBase controller)
        {
            return GetCurrentSiteMapNodeForChildAction(controller, SiteMap.Provider);
        }

        /// <summary>
        /// Gets the current site map node for child action.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static SiteMapNode GetCurrentSiteMapNodeForChildAction(this ControllerBase controller, SiteMapProvider provider)
        {
            // Is DefaultSiteMapProvider in use?
            DefaultSiteMapProvider mvcProvider = provider as DefaultSiteMapProvider;
            if (mvcProvider != null && controller.ControllerContext.IsChildAction)
            {
                return mvcProvider.FindSiteMapNode(controller.ControllerContext);
            }
            else
            {
                return provider.CurrentNode;
            }
        }
    }
}
