using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using MvcSiteMapProvider.Extensibility;
using MvcSiteMapProvider.Core.SiteMap;
using MvcSiteMapProvider.Core.Reflection;

namespace MvcMusicStore.Code
{
    /// <summary>
    /// Only displays nodes when a user is not authenticated.
    /// </summary>
    public class NonAuthenticatedVisibilityProvider
        : ISiteMapNodeVisibilityProvider
    {
        #region ISiteMapNodeVisibilityProvider Members

        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        public bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            return !HttpContext.Current.Request.IsAuthenticated;
        }

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        public bool AppliesTo(string providerName)
        {
            return this.GetType().ShortAssemblyQualifiedName().Equals(providerName);
        }

        #endregion
    }
}