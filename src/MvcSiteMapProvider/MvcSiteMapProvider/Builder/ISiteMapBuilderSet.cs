using System;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for specifying a named builder set that can be used to build a <see cref="T:MvcSiteMapProvider.ISiteMap"/> for 
    /// a specific type of tenant in a multi-tenant application that contains more than one site structure. These named builder sets
    /// can then be mapped to a request using instances of <see cref="T:MvcSiteMapProvider.Caching.ISiteMapCacheKeyGenerator"/> and 
    /// <see cref="T:MvcSiteMapProvider.Caching.ISiteMapCacheKeyToBuilderSetMapper"/>.
    /// </summary>
    public interface ISiteMapBuilderSet
        : ISiteMapSettings
    {
        ISiteMapBuilder Builder { get; }
        ICacheDetails CacheDetails { get; }

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="builderSetName">The name of the builder set.</param>
        /// <returns>
        /// True if the builder set name matches.
        /// </returns>
        bool AppliesTo(string builderSetName);
    }
}
