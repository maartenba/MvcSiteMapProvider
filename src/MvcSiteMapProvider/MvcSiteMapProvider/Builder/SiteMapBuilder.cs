using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.DI;
using MvcSiteMapProvider.Visitor;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// The default implementation of SiteMapBuilder. Builds a <see cref="T:MvcSiteMapProvider.ISiteMapNode"/> tree
    /// based on a <see cref="T:MvcSiteMapProvider.ISiteMapNodeProvider"/> and then runs a <see cref="T:MvcSiteMapProvider.Visitor.ISiteMapNodeVisitor"/>
    /// to optimize the nodes.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class SiteMapBuilder
        : ISiteMapBuilder
    {
        public SiteMapBuilder(
            ISiteMapNodeProvider siteMapNodeProvider,
            ISiteMapNodeVisitor siteMapNodeVisitor,
            ISiteMapHierarchyBuilder siteMapHierarchyBuilder,
            ISiteMapNodeHelperFactory siteMapNodeHelperFactory
            )
        {
            if (siteMapNodeProvider == null)
                throw new ArgumentNullException("siteMapNodeProvider");
            if (siteMapNodeVisitor == null)
                throw new ArgumentNullException("siteMapNodeVisitor");
            if (siteMapHierarchyBuilder == null)
                throw new ArgumentNullException("siteMapHierarchyBuilder");
            if (siteMapNodeHelperFactory == null)
                throw new ArgumentNullException("siteMapNodeHelperFactory");
            
            this.siteMapNodeProvider = siteMapNodeProvider;
            this.siteMapHierarchyBuilder = siteMapHierarchyBuilder;
            this.siteMapNodeHelperFactory = siteMapNodeHelperFactory;
            this.siteMapNodeVisitor = siteMapNodeVisitor;
        }
        protected readonly ISiteMapNodeProvider siteMapNodeProvider;
        protected readonly ISiteMapHierarchyBuilder siteMapHierarchyBuilder;
        protected readonly ISiteMapNodeHelperFactory siteMapNodeHelperFactory;
        protected readonly ISiteMapNodeVisitor siteMapNodeVisitor;

        #region ISiteMapBuilder Members

        public ISiteMapNode BuildSiteMap(ISiteMap siteMap, ISiteMapNode rootNode)
        {
            // Load the source nodes
            var sourceNodes = new List<ISiteMapNodeToParentRelation>();
            LoadSourceNodes(siteMap, sourceNodes);

            // Add the root node to the sitemap
            var root = GetRootNode(siteMap, sourceNodes);
            if (root != null)
            {
                siteMap.AddNode(root);
            }

            var orphans = this.siteMapHierarchyBuilder.BuildHierarchy(siteMap, sourceNodes);

            if (orphans.Count() > 0)
            {
                // We have orphaned nodes - filter to remove the matching descendants of the mismatched keys.
                var mismatched = from parent in orphans
                                 where !(from child in orphans
                                         select child.Node.Key)
                                         .Contains(parent.ParentKey)
                                 select parent;

                var names = String.Join(Environment.NewLine + Environment.NewLine, mismatched.Select(x => String.Format(Resources.Messages.SiteMapNodeFormatWithParentKey, x.ParentKey, x.Node.Controller, x.Node.Action, x.Node.Area, x.Node.Url, x.Node.Key, x.SourceName)).ToArray());
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapBuilderOrphanedNodes, siteMap.CacheKey, names));
            }

            // Run our visitors
            VisitNodes(root);

            // Done!
            return root;
        }

        #endregion

        protected virtual void LoadSourceNodes(ISiteMap siteMap, List<ISiteMapNodeToParentRelation> sourceNodes)
        {
            var siteMapNodeHelper = this.siteMapNodeHelperFactory.Create(siteMap);
            sourceNodes.AddRange(this.siteMapNodeProvider.GetSiteMapNodes(siteMapNodeHelper));
        }

        protected virtual ISiteMapNode GetRootNode(ISiteMap siteMap, IList<ISiteMapNodeToParentRelation> sourceNodes)
        {
            var rootNodes = sourceNodes.Where(x => String.IsNullOrEmpty(x.ParentKey) || x.ParentKey.Trim() == String.Empty);

            // Check if we have more than one root node defined or no root defined
            if (rootNodes.Count() > 1)
            {
                var names = String.Join(Environment.NewLine + Environment.NewLine, rootNodes.Select(x => String.Format(Resources.Messages.SiteMapNodeFormatWithParentKey, x.ParentKey, x.Node.Controller, x.Node.Action, x.Node.Area, x.Node.Url, x.Node.Key, x.SourceName)).ToArray());
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapBuilderRootKeyAmbiguous, siteMap.CacheKey, names));
            }
            else if (rootNodes.Count() == 0)
            {
                throw new MvcSiteMapException(String.Format(Resources.Messages.SiteMapBuilderRootNodeNotDefined, siteMap.CacheKey));
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
