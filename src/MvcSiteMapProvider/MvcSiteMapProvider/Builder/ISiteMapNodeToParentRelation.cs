namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for class that provides a map between a SiteMap node instance and its parent key.
    /// </summary>
    public interface ISiteMapNodeToParentRelation
    {
        string ParentKey { get; }
        ISiteMapNode Node { get; }
        string SourceName { get; }
    }
}
