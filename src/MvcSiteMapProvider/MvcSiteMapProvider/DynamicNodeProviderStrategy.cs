using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Tracks all of the registered instances of <see cref="T:MvcSiteMapProvider.IDynamicNodeProvider"/> and 
    /// allows the caller to get a specific named instance of <see cref="T:MvcSiteMapProvider.IDynamicNodeProvider"/> at runtime.
    /// </summary>
    public class DynamicNodeProviderStrategy
        : IDynamicNodeProviderStrategy
    {
        public DynamicNodeProviderStrategy(IDynamicNodeProvider[] dynamicNodeProviders)
        {
            if (dynamicNodeProviders == null)
                throw new ArgumentNullException("dynamicNodeProviders");

            this.dynamicNodeProviders = dynamicNodeProviders;
        }

        private readonly IDynamicNodeProvider[] dynamicNodeProviders;

        #region IDynamicNodeProviderStrategy Members

        public IDynamicNodeProvider GetProvider(string providerName)
        {
            var provider = this.dynamicNodeProviders.FirstOrDefault(x => x.AppliesTo(providerName));
            if (provider == null && !string.IsNullOrEmpty(providerName))
            {
                throw new MvcSiteMapException(string.Format(Resources.Messages.NamedDynamicNodeProviderNotFound, providerName));
            }

            return provider;
        }

        public IEnumerable<DynamicNode> GetDynamicNodeCollection(string providerName, ISiteMapNode node)
        {
            var provider = this.GetProvider(providerName);
            if (provider == null) return new List<DynamicNode>(); // No provider, return empty collection
            return provider.GetDynamicNodeCollection(node);
        }

        #endregion
    }
}
