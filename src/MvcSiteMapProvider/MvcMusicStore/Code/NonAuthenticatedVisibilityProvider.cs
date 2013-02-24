using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Reflection;

namespace MvcMusicStore.Code
{
    /// <summary>
    /// Only displays nodes when a user is not authenticated.
    /// </summary>
    public class NonAuthenticatedVisibilityProvider
        : SiteMapNodeVisibilityProviderBase
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
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            return !HttpContext.Current.Request.IsAuthenticated;
        }

        #endregion
    }
}