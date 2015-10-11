using MvcSiteMapProvider.Visitor;
using System;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Provides a means of optimizing <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> instances before they 
    /// are placed in the cahce.
    /// </summary>
    [Obsolete("VisitingSiteMapBuilder has been deprecated and will be removed in version 5. This functionality was merged into the SiteMapBuilder class, as it only makes sense to run this after the sitemap is completely built.")]
    public class VisitingSiteMapBuilder
        : ISiteMapBuilder
    {
        public VisitingSiteMapBuilder(
            ISiteMapNodeVisitor siteMapNodeVisitor
            )
        {
            if (siteMapNodeVisitor == null)
                throw new ArgumentNullException("siteMapNodeVisitor");

            this.siteMapNodeVisitor = siteMapNodeVisitor;
        }

        protected readonly ISiteMapNodeVisitor siteMapNodeVisitor;


        #region ISiteMapBuilder Members

        public virtual ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            if (rootNode == null)
            {
                throw new ArgumentNullException("rootNode", Resources.Messages.VisitingSiteMapBuilderRequiresRootNode);
            }

            VisitNodes(rootNode);
            return rootNode;
        }

        protected virtual void VisitNodes(ISiteMapNode node)
        {
            this.siteMapNodeVisitor.Execute(node);

            if (node.HasChildNodes)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    VisitNodes(childNode);
                }
            }
        }

        #endregion
    }
}
