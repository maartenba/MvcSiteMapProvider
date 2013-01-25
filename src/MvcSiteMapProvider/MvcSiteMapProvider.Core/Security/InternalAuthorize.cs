#region Using directives

using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc.Infrastructure.Implementation;

#endregion

namespace MvcSiteMapProvider.Core.Security
{
    /// <summary>
    /// InternalAuthorize class
    /// </summary>
    public class InternalAuthorize : AuthorizeAttribute, IAuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalAuthorize"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public InternalAuthorize(AuthorizeAttribute attribute)
        {
            Order = attribute.Order;
            Roles = attribute.Roles;
            Users = attribute.Users;
        }

        /// <summary>
        /// Determines whether the specified HTTP context is authorized.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>
        /// 	<c>true</c> if the specified HTTP context is authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAuthorized(HttpContextBase httpContext)
        {
            return AuthorizeCore(httpContext);
        }
    }
}
