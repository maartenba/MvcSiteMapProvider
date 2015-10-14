using MvcSiteMapProvider.DI;
using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Chains together a group of ISiteMapNodeVisibilityProvider instances so that visibility logic 
    /// for different purposes can be kept in different providers, but still apply to a single node.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class CompositeSiteMapNodeVisibilityProvider
        : ISiteMapNodeVisibilityProvider
    {
        public CompositeSiteMapNodeVisibilityProvider(string instanceName, params ISiteMapNodeVisibilityProvider[] siteMapNodeVisibilityProviders)
        {
            if (string.IsNullOrEmpty(instanceName))
                throw new ArgumentNullException("instanceName");
            if (siteMapNodeVisibilityProviders == null)
                throw new ArgumentNullException("siteMapNodeVisibilityProviders");

            this.instanceName = instanceName;
            this.siteMapNodeVisibilityProviders = siteMapNodeVisibilityProviders;
        }
        private readonly string instanceName;
        private readonly ISiteMapNodeVisibilityProvider[] siteMapNodeVisibilityProviders;

        #region ISiteMapNodeVisibilityProvider Members

        public bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            // Result is always true unless the first provider that returns false is encountered.
            bool result = true;
            foreach (var visibilityProvider in this.siteMapNodeVisibilityProviders)
            {
                result = visibilityProvider.IsVisible(node, sourceMetadata);
                if (result == false)
                    return false;
            }
            return result;
        }

        public bool AppliesTo(string providerName)
        {
            return this.instanceName.Equals(providerName, StringComparison.Ordinal);
        }

        #endregion
    }
}
