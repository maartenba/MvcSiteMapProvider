using System;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// CompositeAclModule class
    /// </summary>
    public class CompositeAclModule
        : IAclModule
    {
        /// <summary>
        /// Used to chain several <see cref="T:MvcSiteMapProvider.Security.IAclModule"/> instances in succession. 
        /// The builders will be processed in the same order as they are specified in the constructor.
        /// </summary>
        public CompositeAclModule(
            params IAclModule[] aclModules
            )
        {
            if (aclModules == null)
                throw new ArgumentNullException("aclModules");

            this.aclModules = aclModules;
        }

        protected readonly IAclModule[] aclModules;

        #region IAclModule Members

        /// <summary>
        /// Determines whether node is accessible to user.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if accessible to user; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsAccessibleToUser(ISiteMap siteMap, ISiteMapNode node)
        {
            // Is security trimming enabled?
            if (!siteMap.SecurityTrimmingEnabled)
            {
                return true;
            }

            // Use child modules
            bool result = true;
            foreach (var module in aclModules)
            {
                try
                {
                    result &= module.IsAccessibleToUser(siteMap, node);
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
