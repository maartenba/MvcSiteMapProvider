using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Creates nodes dynamically based on an implemenation of <see cref="T:MvcSiteMapProvider.IDynamicNodeProvider"/>.
    /// </summary>
    public class DynamicNodeBuilder
        : IDynamicNodeBuilder
    {
        public DynamicNodeBuilder(
            INodeKeyGenerator nodeKeyGenerator,
            ISiteMapNodeFactory siteMapNodeFactory
            )
        {
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");

            this.nodeKeyGenerator = nodeKeyGenerator;
            this.siteMapNodeFactory = siteMapNodeFactory;
        }

        private readonly INodeKeyGenerator nodeKeyGenerator;
        private readonly ISiteMapNodeFactory siteMapNodeFactory;

        /// <summary>
        /// Adds the dynamic nodes for node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="parentNode">The parent node.</param>
        public IEnumerable<ISiteMapNode> BuildDynamicNodesFor(ISiteMap siteMap, ISiteMapNode node, ISiteMapNode parentNode)
        {
            // List of dynamic nodes that have been created
            var createdDynamicNodes = new List<ISiteMapNode>();

            if (!node.HasDynamicNodeProvider)
            {
                return createdDynamicNodes;
            }

            // Build dynamic nodes
            foreach (var dynamicNode in node.GetDynamicNodeCollection())
            {
                string key = dynamicNode.Key;
                if (string.IsNullOrEmpty(key))
                {
                    key = nodeKeyGenerator.GenerateKey(
                        parentNode == null ? "" : parentNode.Key, 
                        Guid.NewGuid().ToString(), 
                        node.Url, 
                        node.Title, 
                        node.Area, 
                        node.Controller, 
                        node.Action,
                        node.HttpMethod,
                        node.Clickable);
                }

                // Create a new node
                var newNode = siteMapNodeFactory.CreateDynamic(siteMap, key, node.ResourceKey);

                // Copy the values from the original node to the new one
                node.CopyTo(newNode);

                // Clear preserved route params - we don't need that
                newNode.PreservedRouteParameters.Clear();

                // Copy any values that were set in the dynamic node and overwrite the new node.
                dynamicNode.SafeCopyTo(newNode);

                // If the dynamic node has a parent key set, use that as the parent. Otherwise use the parentNode.
                if (!string.IsNullOrEmpty(dynamicNode.ParentKey))
                {
                    var parent = siteMap.FindSiteMapNodeFromKey(dynamicNode.ParentKey);
                    if (parent != null)
                    {
                        siteMap.AddNode(newNode, parent);
                        createdDynamicNodes.Add(newNode);
                    }
                }
                else
                {
                    siteMap.AddNode(newNode, parentNode);
                    createdDynamicNodes.Add(newNode);
                }
            }

            // Done!
            return createdDynamicNodes;
        }
    }
}