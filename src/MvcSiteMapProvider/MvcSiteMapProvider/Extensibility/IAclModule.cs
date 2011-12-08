#region Using directives

using System.Web;

#endregion

namespace MvcSiteMapProvider.Extensibility
{
    /// <summary>
    /// IAclModule contract
    /// </summary>
    public interface IAclModule
    {
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
        bool IsAccessibleToUser(IControllerTypeResolver controllerTypeResolver, DefaultSiteMapProvider provider, HttpContext context, SiteMapNode node);
    }
}