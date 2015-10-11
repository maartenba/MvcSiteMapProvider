namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract to provide caching-technology agnostic access to a specific type of cache.
    /// </summary>
    public interface ISiteMapCache
        : IMicroCache<ISiteMap>
    {
    }
}
