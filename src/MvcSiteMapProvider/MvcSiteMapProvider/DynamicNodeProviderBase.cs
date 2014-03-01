using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Reflection;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// DynamicNodeProviderBase class
    /// </summary>
    public abstract class DynamicNodeProviderBase
        : IDynamicNodeProvider
    {
        #region IDynamicNodeProvider Members

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <returns>A dynamic node collection.</returns>
        public abstract IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node);

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name is used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        public virtual bool AppliesTo(string providerName)
        {
            if (string.IsNullOrEmpty(providerName))
                return false;

            return this.GetType().Equals(Type.GetType(providerName, false));
        }

        #endregion

    }
}
