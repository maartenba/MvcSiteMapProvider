using System.Collections.Generic;

namespace MvcSiteMapProvider.Core.SiteMap
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
        /// Gets a cache description for the dynamic node collection 
        /// or null if there is none.
        /// </summary>
        /// <returns>
        /// A cache description represented as a <see cref="CacheDescription"/> instance .
        /// </returns>
        CacheDescription GetCacheDescription();

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
