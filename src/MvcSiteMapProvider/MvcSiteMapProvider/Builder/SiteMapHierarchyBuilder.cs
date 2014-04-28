using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// A service that changes a list of ISiteMapNodeToParentRelation instances into a hierarchy
    /// by mapping each node to its parent node and adds the hierarchy to the SiteMap.
    /// </summary>
    public class SiteMapHierarchyBuilder
        : ISiteMapHierarchyBuilder
    {
        #region ISiteMapHierarchyBuilder Members

        public IEnumerable<ISiteMapNodeToParentRelation> BuildHierarchy(ISiteMap siteMap, IEnumerable<ISiteMapNodeToParentRelation> nodes)
        {
            var sourceNodesByParent = nodes.ToLookup(n => n.ParentKey);
            var sourceNodes = new List<ISiteMapNodeToParentRelation>(nodes);
            var nodesAddedThisIteration = 0;
            do
            {
                var nodesAlreadyAdded = new HashSet<string>();
                nodesAddedThisIteration = 0;
                foreach (var node in sourceNodes.OrderBy(x => x.Node.Order).ToArray())
                {
                    if (nodesAlreadyAdded.Contains(node.Node.Key))
                    {
                        continue;
                    }

                    var parentNode = siteMap.FindSiteMapNodeFromKey(node.ParentKey);
                    if (parentNode != null)
                    {
                        this.AddAndTrackNode(siteMap, node, parentNode, sourceNodes, nodesAlreadyAdded);
                        nodesAddedThisIteration += 1;

                        // Add the rest of the tree branch below the current node
                        this.AddDescendantNodes(siteMap, node.Node, sourceNodes, sourceNodesByParent, nodesAlreadyAdded);
                    }
                }
            }
            while (nodesAddedThisIteration > 0 && sourceNodes.Count > 0);

            return sourceNodes;
        }

        #endregion
        
        protected virtual void AddDescendantNodes(
            ISiteMap siteMap,
            ISiteMapNode currentNode,
            IList<ISiteMapNodeToParentRelation> sourceNodes,
            ILookup<string, ISiteMapNodeToParentRelation> sourceNodesByParent,
            HashSet<string> nodesAlreadyAdded)
        {
            if (sourceNodes.Count == 0)
            {
                return;
            }

            var children = sourceNodesByParent[currentNode.Key].OrderBy(x => x.Node.Order).ToArray();
            if (children.Count() == 0)
            {
                return;
            }

            foreach (var child in children)
            {
                if (sourceNodes.Count == 0)
                {
                    return;
                }

                this.AddAndTrackNode(siteMap, child, currentNode, sourceNodes, nodesAlreadyAdded);

                if (sourceNodes.Count == 0)
                {
                    return;
                }

                this.AddDescendantNodes(siteMap, child.Node, sourceNodes, sourceNodesByParent, nodesAlreadyAdded);
            }
        }

        protected virtual void AddAndTrackNode(
            ISiteMap siteMap,
            ISiteMapNodeToParentRelation nodeParentMap,
            ISiteMapNode parentNode,
            IList<ISiteMapNodeToParentRelation> sourceNodes,
            HashSet<string> nodesAlreadyAdded)
        {
            siteMap.AddNode(nodeParentMap.Node, parentNode);
            nodesAlreadyAdded.Add(nodeParentMap.Node.Key);
            sourceNodes.Remove(nodeParentMap);
        }
    }
}
