using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Tracks all of the registered instances of <see cref="T:MvcSiteMapProvider.ISiteMapNodeVisiblityProvider"/> and 
    /// allows the caller to get a specific named instance of this interface at runtime.
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
            ISiteMapNodeVisibilityProvider provider = null;
            if (!String.IsNullOrEmpty(providerName))
            {
                provider = siteMapNodeVisibilityProviders.FirstOrDefault(x => x.AppliesTo(providerName));
                if (provider == null)
                {
                    throw new MvcSiteMapException(String.Format(Resources.Messages.NamedSiteMapNodeVisibilityProviderNotFound, providerName));
                }
            }
            else if (!string.IsNullOrEmpty(defaultProviderName))
            {
                // Return the configured default provider
                provider = siteMapNodeVisibilityProviders.FirstOrDefault(x => x.AppliesTo(defaultProviderName));
            }
            return provider;
        }

        public bool IsVisible(string providerName, ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            var provider = GetProvider(providerName);
            if (provider == null) return true; // If no default provider configured, then always visible.
            return provider.IsVisible(node, sourceMetadata);
        }

        #endregion
    }
}
