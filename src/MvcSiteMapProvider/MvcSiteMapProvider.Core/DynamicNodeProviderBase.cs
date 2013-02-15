using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MvcSiteMapProvider.Core.Reflection;

namespace MvcSiteMapProvider.Core
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
        /// <returns>A dynamic node collection.</returns>
        public abstract IEnumerable<DynamicNode> GetDynamicNodeCollection();

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        public virtual bool AppliesTo(string providerName)
        {
            return this.GetType().ShortAssemblyQualifiedName().Equals(providerName);
        }

        #endregion

    }
}
