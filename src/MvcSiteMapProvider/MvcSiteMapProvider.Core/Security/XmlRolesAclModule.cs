#region Using directives

using System.Linq;
using System.Web;
using MvcSiteMapProvider.Core.Mvc;
using MvcSiteMapProvider.Core.SiteMap;

#endregion

namespace MvcSiteMapProvider.Core.Security
{
    /// <summary>
    /// XmlRolesAclModule class
    /// </summary>
    public class XmlRolesAclModule
        : IAclModule
    {
        #region IAclModule Members

        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="controllerTypeResolver">The controller type resolver.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAccessibleToUser(IControllerTypeResolver controllerTypeResolver, MvcSiteMapProvider.Core.SiteMap.SiteMap provider, HttpContext context, SiteMapNode node)
        {
            // Is security trimming enabled?
            if (!provider.SecurityTrimmingEnabled)
            {
                return true;
            }

            // Is it a regular node?
            var mvcNode = node as MvcSiteMapNode;
            if (mvcNode == null)
            {
                if (provider.ParentProvider != null)
                {
                    return provider.ParentProvider.IsAccessibleToUser(context, node);
                }
            }

            // If we have roles assigned, check them against the roles defined in the sitemap
            if (node.Roles != null && node.Roles.Count > 0)
            {
                    // if there is an authenticated user and the role allows anyone authenticated ("*"), show it
                if ((context.User.Identity.IsAuthenticated) && node.Roles.Contains("*"))
                {
                    return true;
                }

                    // if there is no user, but the role allows unauthenticated users ("?"), show it
                if ((!context.User.Identity.IsAuthenticated) && node.Roles.Contains("?"))
                    {
                        return true;
                    }

                    // if the user is in one of the listed roles, show it
                if (node.Roles.OfType<string>().Any(role => context.User.IsInRole(role)))
                    {
                        return true;
                    }

                    // if we got this far, deny showing
                    return false;
            }

            // Everything seems OK...
            return true;
        }

        #endregion
    }
}
