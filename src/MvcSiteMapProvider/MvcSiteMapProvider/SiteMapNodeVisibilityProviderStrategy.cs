using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Tracks all of the registered instances of <see cref="T:MvcSiteMapProvider.ISiteMapNodeVisiblityProvider"/> and 
    /// allows the caller to get a specific named instance of <see cref="T:MvcSiteMapProvider.ISiteMapNodeVisiblityProvider"/> at runtime.
    /// </summary>
    public class SiteMapNodeVisibilityProviderStrategy
        : ISiteMapNodeVisibilityProviderStrategy
    {
        public SiteMapNodeVisibilityProviderStrategy(ISiteMapNodeVisibilityProvider[] siteMapNodeVisibilityProviders, string defaultProviderName)
        {
            if (siteMapNodeVisibilityProviders == null)
                throw new ArgumentNullException("siteMapNodeVisibilityProviders");

            this.siteMapNodeVisibilityProviders = siteMapNodeVisibilityProviders;
            this.defaultProviderName = defaultProviderName;
        }

        private readonly ISiteMapNodeVisibilityProvider[] siteMapNodeVisibilityProviders;
        private readonly string defaultProviderName;


        #region ISiteMapNodeVisibilityProviderStrategy Members

        public ISiteMapNodeVisibilityProvider GetProvider(string providerName)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                // Get the configured default provider
                providerName = this.defaultProviderName;
            }

            var provider = this.siteMapNodeVisibilityProviders.FirstOrDefault(x => x.AppliesTo(providerName));
            if (provider == null && !string.IsNullOrEmpty(providerName))
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.NamedSiteMapNodeVisibilityProviderNotFound, providerName));
            }

            return provider;
        }

        public bool IsVisible(string providerName, ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            var provider = this.GetProvider(providerName);
            if (provider == null) return true; // If no provider configured, then always visible.
            return provider.IsVisible(node, sourceMetadata);
        }

        #endregion
    }
}
