namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Implement this interface to make a custom implementation that will build a sitemap.
    /// </summary>
    public interface ISiteMapBuilder
    {
        ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode);
    }
}
