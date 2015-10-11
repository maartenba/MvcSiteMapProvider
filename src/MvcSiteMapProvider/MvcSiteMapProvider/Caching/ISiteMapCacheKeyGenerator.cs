namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// ISiteMapCacheKeyGenerator interface. This interface allows the semantics of when a new sitemap
    /// is generated vs when the sitemap is stored to be changed. Each unique sitemap key that is generated
    /// causes a new sitemap to be stored in the cache. This can be used to control how incoming requests 
    /// map to a specific sitemap.
    /// </summary>
    public interface ISiteMapCacheKeyGenerator
    {
        string GenerateKey();
    }
}
