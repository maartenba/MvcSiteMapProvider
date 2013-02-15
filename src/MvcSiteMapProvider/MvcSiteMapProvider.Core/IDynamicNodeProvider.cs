using System.Collections.Generic;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// IDynamicNodeProvider contract.
    /// </summary>
    public interface IDynamicNodeProvider
    {
        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <returns>A dynamic node collection.</returns>
        IEnumerable<DynamicNode> GetDynamicNodeCollection();

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        bool AppliesTo(string providerName);
    }
}
