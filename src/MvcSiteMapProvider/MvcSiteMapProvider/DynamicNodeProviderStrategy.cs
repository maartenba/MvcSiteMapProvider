﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Tracks all of the registered instances of <see cref="T:MvcSiteMapProvider.IDynamicNodeProvider"/> and 
    /// allows the caller to get a specific named instance of this interface at runtime.
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
            return dynamicNodeProviders.FirstOrDefault(x => x.AppliesTo(providerName));
        }

        public IEnumerable<DynamicNode> GetDynamicNodeCollection(string providerName, ISiteMapNode node)
        {
            var provider = GetProvider(providerName);
            if (provider == null) return new List<DynamicNode>();
            return provider.GetDynamicNodeCollection(node);
        }

        #endregion
    }
}
