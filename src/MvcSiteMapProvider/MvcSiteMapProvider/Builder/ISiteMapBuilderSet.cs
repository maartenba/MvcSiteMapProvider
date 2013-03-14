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
    {
        ISiteMapBuilder Builder { get; }
        ICacheDetails CacheDetails { get; }

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        bool AppliesTo(string builderSetName);
    }
}
