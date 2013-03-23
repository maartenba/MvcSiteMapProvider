using System;
using System.Web;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// IAclModule contract
    /// </summary>
    public interface IAclModule
    {
        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        bool IsAccessibleToUser(ISiteMap siteMap, ISiteMapNode node);
    }
}