using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// Provides a means to yield control of the lifetime of provider
    /// instances to the DI container, while allowing the right instance
    /// for the job to be selected at runtime.
    /// 
    /// http://stackoverflow.com/questions/1499442/best-way-to-use-structuremap-to-implement-strategy-pattern#1501517
    /// </summary>
    public interface IDynamicNodeProviderStrategy
    {
        IDynamicNodeProvider GetProvider(string providerName);
        IEnumerable<DynamicNode> GetDynamicNodeCollection(string providerName);
        CacheDescription GetCacheDescription(string providerName);
    }
}
