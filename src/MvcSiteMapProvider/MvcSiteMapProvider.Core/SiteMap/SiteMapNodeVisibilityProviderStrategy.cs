using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNodeVisibilityProviderStrategy
        : ISiteMapNodeVisibilityProviderStrategy
    {
        public SiteMapNodeVisibilityProviderStrategy(ISiteMapNodeVisibilityProvider[] siteMapNodeVisibilityProviders)
        {
            if (siteMapNodeVisibilityProviders == null)
                throw new ArgumentNullException("siteMapNodeVisibilityProviders");

            this.siteMapNodeVisibilityProviders = siteMapNodeVisibilityProviders;
        }

        private readonly ISiteMapNodeVisibilityProvider[] siteMapNodeVisibilityProviders;


        #region ISiteMapNodeVisibilityProviderStrategy Members

        public ISiteMapNodeVisibilityProvider GetProvider(string providerName)
        {
            return siteMapNodeVisibilityProviders.FirstOrDefault(x => x.AppliesTo(providerName));
        }

        public bool IsVisible(string providerName, ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            var provider = GetProvider(providerName);
            if (provider == null) return true; // If no provider configured, then always visible.
            return provider.IsVisible(node, sourceMetadata);
        }

        #endregion
    }
}
