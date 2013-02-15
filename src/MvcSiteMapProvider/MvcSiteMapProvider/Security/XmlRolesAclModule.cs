using System;
using System.Linq;
using System.Web;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// XmlRolesAclModule class
    /// </summary>
    public class XmlRolesAclModule
        : IAclModule
    {
        public XmlRolesAclModule(
            IHttpContextFactory httpContextFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");

            this.httpContextFactory = httpContextFactory;
        }

        protected readonly IHttpContextFactory httpContextFactory;

        #region IAclModule Members

        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="controllerTypeResolver">The controller type resolver.</param>
        /// <param name="siteMap">The site map.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAccessibleToUser(ISiteMap siteMap, ISiteMapNode node)
        {
            // Is security trimming enabled?
            if (!siteMap.SecurityTrimmingEnabled)
            {
                return true;
            }

            // Is it a regular node?
            var mvcNode = node;

            // If we have roles assigned, check them against the roles defined in the sitemap
            if (node.Roles != null && node.Roles.Count > 0)
            {
                var context = httpContextFactory.Create();

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
