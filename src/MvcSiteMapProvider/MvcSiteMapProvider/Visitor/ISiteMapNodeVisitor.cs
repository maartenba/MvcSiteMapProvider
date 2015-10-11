namespace MvcSiteMapProvider.Visitor
{
    /// <summary>
    /// Contract for a site map node visitor.
    /// </summary>
    public interface ISiteMapNodeVisitor
    {
        void Execute(ISiteMapNode node);
    }
}
