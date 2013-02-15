using System;

namespace MvcSiteMapProvider.Visitor
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNodeVisitor
    {
        void Execute(ISiteMapNode node);
    }
}
