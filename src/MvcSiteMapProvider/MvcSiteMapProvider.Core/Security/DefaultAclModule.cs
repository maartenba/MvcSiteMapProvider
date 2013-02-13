using System;
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
        protected IEnumerable<IAclModule> ChildModules { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAclModule"/> class.
        /// </summary>
        public DefaultAclModule(
            params IAclModule[] aclModules
            )
        {
            if (aclModules == null)
                throw new ArgumentNullException("aclModules");

            this.ChildModules = aclModules;
        }


        ///// <summary>
        ///// Initializes a new instance of the <see cref="DefaultAclModule"/> class.
        ///// </summary>
        //public DefaultAclModule(
        //    IControllerTypeResolver controllerTypeResolver
        //    )
        //{
        //    if (controllerTypeResolver == null)
        //        throw new ArgumentNullException("controllerTypeResolver");

        //    // TODO: Make factory so these can be injected via DI.
        //    ChildModules = new List<IAclModule>
        //    {
        //        new XmlRolesAclModule(),
        //        new AuthorizeAttributeAclModule(controllerTypeResolver)
        //    };
        //}

        #region IAclModule Members

        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsAccessibleToUser(ISiteMap siteMap, HttpContext context, ISiteMapNode node)
        {
            // Is security trimming enabled?
            if (!siteMap.SecurityTrimmingEnabled)
            {
                return true;
            }

            // Use child modules
            bool result = true;
            foreach (var module in ChildModules)
            {
                try
                {
                    result &= module.IsAccessibleToUser(siteMap, context, node);
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
