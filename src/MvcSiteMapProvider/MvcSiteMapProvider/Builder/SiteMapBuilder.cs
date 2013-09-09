using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Visitor;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// The default implementation of SiteMapBuilder. Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree
    /// based on a <see cref="T:MvcSiteMapProvider.ISiteMapNodeProvider"/> and then runs a <see cref="T:MvcSiteMapProvider.Visitor.ISiteMapNodeVisitor"/>
    /// to optimize the nodes.
    /// </summary>
    public class SiteMapBuilder
        : ISiteMapBuilder
    {
        public SiteMapBuilder(
            ISiteMapNodeProvider siteMapNodeProvider,
            ISiteMapNodeVisitor siteMapNodeVisitor,
            ISiteMapHierarchyBuilder siteMapHierarchyBuilder,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator,
            ISiteMapNodeHelperFactory siteMapNodeHelperFactory
            )
        {
            if (siteMapNodeProvider == null)
                throw new ArgumentNullException("siteMapNodeProvider");
            if (siteMapNodeVisitor == null)
                throw new ArgumentNullException("siteMapNodeVisitor");
            if (siteMapHierarchyBuilder == null)
                throw new ArgumentNullException("siteMapHierarchyBuilder");
            if (siteMapCacheKeyGenerator == null)
                throw new ArgumentNullException("siteMapCacheKeyGenerator");
            if (siteMapNodeHelperFactory == null)
                throw new ArgumentNullException("siteMapNodeHelperFactory");
            
            this.siteMapNodeProvider = siteMapNodeProvider;
            this.siteMapHierarchyBuilder = siteMapHierarchyBuilder;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;
            this.siteMapNodeHelperFactory = siteMapNodeHelperFactory;
            this.siteMapNodeVisitor = siteMapNodeVisitor;
        }
        protected readonly ISiteMapNodeProvider siteMapNodeProvider;
        protected readonly ISiteMapHierarchyBuilder siteMapHierarchyBuilder;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        protected readonly ISiteMapNodeHelperFactory siteMapNodeHelperFactory;
        protected readonly ISiteMapNodeVisitor siteMapNodeVisitor;

        // TODO: In version 5, we need to have the siteMapCacheKey passed from the
        // caller or set to the sitemap itself so we can access it directly from the builder.

        #region ISiteMapBuilder Members

        public ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            var sourceNodes = new List<ISiteMapNodeParentMap>();
            var siteMapCacheKey = this.siteMapCacheKeyGenerator.GenerateKey();

            LoadSourceNodes(siteMap, siteMapCacheKey, sourceNodes);

            var root = GetRootNode(siteMapCacheKey, sourceNodes);

            // Add the root node to the sitemap
            if (root != null)
            {
                siteMap.AddNode(root);
            }

            var orphans = this.siteMapHierarchyBuilder.BuildHierarchy(siteMap, sourceNodes);

            if (orphans.Count() > 0)
            {
                // We have orphaned nodes - throw an exception.
                var names = String.Join(Environment.NewLine + Environment.NewLine, orphans.Select(x => String.Format(Resources.Messages.SiteMapNodeFormatWithParentKey, x.ParentKey, x.Node.Controller, x.Node.Action, x.Node.Area, x.Node.Url, x.Node.Key, x.SourceName)));
                throw new MvcSiteMapException(String.Format(Resources.Messages.XmlSiteMapBuilderOrphanedNodes, siteMapCacheKey, names));
            }

            // Run our visitors
            VisitNodes(root);

            // Done!
            return root;
        }

        #endregion

        protected virtual void LoadSourceNodes(ISiteMap siteMap, string siteMapCacheKey, List<ISiteMapNodeParentMap> sourceNodes)
        {
            var siteMapNodeHelper = this.siteMapNodeHelperFactory.Create(siteMap, siteMapCacheKey);
            sourceNodes.AddRange(this.siteMapNodeProvider.GetSiteMapNodes(siteMapNodeHelper));
        }

        protected virtual ISiteMapNode GetRootNode(string siteMapCacheKey, IList<ISiteMapNodeParentMap> sourceNodes)
        {
            var rootNodes = sourceNodes.Where(x => String.IsNullOrEmpty(x.ParentKey) || x.ParentKey.Trim() == String.Empty);

            // Check if we have more than one root node defined or no root defined
            if (rootNodes.Count() > 1)
            {
                var names = String.Join(Environment.NewLine + Environment.NewLine, rootNodes.Select(x => String.Format(Resources.Messages.SiteMapNodeFormatWithParentKey, x.ParentKey, x.Node.Controller, x.Node.Action, x.Node.Area, x.Node.Url, x.Node.Key, x.SourceName)));
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapBuilderRootKeyAmbiguous, siteMapCacheKey, names));
            }
            else if (rootNodes.Count() == 0)
            {
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapBuilderRootNodeNotDefined, siteMapCacheKey));
            }

            var root = rootNodes.Single();

            // Remove the root node from the sourceNodes
            sourceNodes.Remove(root);

            return root.Node;
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
    }
}
