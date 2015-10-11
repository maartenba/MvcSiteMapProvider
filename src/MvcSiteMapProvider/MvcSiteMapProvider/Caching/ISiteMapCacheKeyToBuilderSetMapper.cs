namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Implement this interface to control which named builder set is used to build 
    /// a specific sitemap. This can be hard-coded logic, or pulled from a configuration file or database.
    /// </summary>
    public interface ISiteMapCacheKeyToBuilderSetMapper
    {
        string GetBuilderSetName(string cacheKey);
    }
}
