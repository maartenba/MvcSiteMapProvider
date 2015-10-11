namespace MvcSiteMapProvider.Visitor
{
    /// <summary>
    /// A visitor class that implements the null object pattern. Use this class when you don't
    /// want any visitors to run.
    /// </summary>
    public class NullSiteMapNodeVisitor
        : ISiteMapNodeVisitor
    {
        #region ISiteMapNodeVisitor Members

        public void Execute(ISiteMapNode node)
        {
            // No Implementation
        }

        #endregion
    }
}
