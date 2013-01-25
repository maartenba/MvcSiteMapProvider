#region Using directives

using System.Collections.Generic;

#endregion

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
    }
}
