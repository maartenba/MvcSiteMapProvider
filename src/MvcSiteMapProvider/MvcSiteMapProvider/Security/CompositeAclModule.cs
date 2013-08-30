using System;

namespace MvcSiteMapProvider.Security
{
    /// <summary>
    /// Used to chain multiple <see cref="T:MvcSiteMapProvider.Security.IAclModule"/> instances in succession. 
    /// The builders will be processed in the same order as they are specified in the constructor.
    /// </summary>
    public class CompositeAclModule
        : IAclModule
    {
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
            foreach (var module in aclModules)
            {
                var authorized = module.IsAccessibleToUser(siteMap, node);
                if (authorized == false)
                    return false;
            }
            // Convention throughout the provider: if the IAclModule can not authenticate a user, true is returned.
            return true;
        }

        #endregion
    }
}