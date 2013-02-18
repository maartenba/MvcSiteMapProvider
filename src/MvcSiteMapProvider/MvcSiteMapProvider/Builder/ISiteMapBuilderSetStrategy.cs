using System;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapBuilderSetStrategy
    {
        ISiteMapBuilderSet GetBuilderSet(string builderSetName);
        ISiteMapBuilder GetBuilder(string builderSetName);
        ICacheDependency GetCacheDependency(string builderSetName);
    }
}
