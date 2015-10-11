using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract to provide a concrete implementation that tracks all of the registered instances of 
    /// <see cref="T:MvcSiteMapProvider.Builder.ISiteMapBuilderSet"/> and allows the caller to get a specific 
    /// named instance of this interface at runtime.
    /// </summary>
    public interface ISiteMapBuilderSetStrategy
    {
        ISiteMapBuilderSet GetBuilderSet(string builderSetName);
        ISiteMapBuilder GetBuilder(string builderSetName);
        ICacheDetails GetCacheDetails(string builderSetName);
    }
}
