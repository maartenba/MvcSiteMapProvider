namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> at runtime.
    /// </summary>
    public interface ISiteMapNodeFactory
    {
        ISiteMapNode Create(ISiteMap siteMap, string key, string implicitResourceKey);
        ISiteMapNode CreateDynamic(ISiteMap siteMap, string key, string implicitResourceKey);
    }
}
