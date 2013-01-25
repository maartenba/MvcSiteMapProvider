// (c) Copyright Telerik Corp. 
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.

#region Using directives

using System.Web;

#endregion

namespace Telerik.Web.Mvc.Infrastructure.Implementation
{
    /// <summary>
    /// IAuthorizeAttribute contract
    /// </summary>
    public interface IAuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        int Order { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        string Roles { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        string Users { get; set; }

        /// <summary>
        /// Determines whether the specified HTTP context is authorized.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>
        /// 	<c>true</c> if the specified HTTP context is authorized; otherwise, <c>false</c>.
        /// </returns>
        bool IsAuthorized(HttpContextBase httpContext);
    }
}
