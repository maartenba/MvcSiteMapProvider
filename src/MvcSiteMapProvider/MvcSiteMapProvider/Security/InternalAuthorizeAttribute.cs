using System.Web;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// InternalAuthorize class
    /// </summary>
    public class InternalAuthorizeAttribute 
        : AuthorizeAttribute, IAuthorizeAttribute
    {
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
