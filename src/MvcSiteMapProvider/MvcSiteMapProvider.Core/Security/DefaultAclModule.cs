using System.Collections.Generic;
using System.Web;
using MvcSiteMapProvider.Core.Mvc;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Security
{
    /// <summary>
    /// DefaultAclModule class
    /// </summary>
    public class DefaultAclModule
        : IAclModule
    {
        /// <summary>
        /// Gets or sets the child modules.
        /// </summary>
        /// <value>The child modules.</value>
        protected List<IAclModule> ChildModules { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAclModule"/> class.
        /// </summary>
        public DefaultAclModule()
        {
            ChildModules = new List<IAclModule>
            {
                new XmlRolesAclModule(),
                new AuthorizeAttributeAclModule()
            };
        }

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
        public virtual bool IsAccessibleToUser(IControllerTypeResolver controllerTypeResolver, ISiteMap provider, HttpContext context, ISiteMapNode node)
        {
            // Is security trimming enabled?
            if (!provider.SecurityTrimmingEnabled)
            {
                return true;
            }

            // Use child modules
            bool result = true;
            foreach (var module in ChildModules)
            {
                try
                {
                    result &= module.IsAccessibleToUser(controllerTypeResolver, provider, context, node);
                }
                catch (AclModuleNotSupportedException)
                {
                    result &= true; // Convention throughout the provider: if the IAclModule can not authenticate a user, true is returned.
                }
                if (result == false)
                {
                    return false;
                }
            }

            // Return
            return result;
        }

        #endregion
    }
}
