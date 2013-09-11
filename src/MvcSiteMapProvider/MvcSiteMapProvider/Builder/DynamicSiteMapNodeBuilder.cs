using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Creates nodes with a map to their parent node dynamically based on an implemenation of <see cref="T:MvcSiteMapProvider.IDynamicNodeProvider"/>.
    /// </summary>
    public class DynamicSiteMapNodeBuilder
        : IDynamicSiteMapNodeBuilder
    {
        public DynamicSiteMapNodeBuilder(
            ISiteMapNodeCreator siteMapNodeCreator
            )
        {
            if (siteMapNodeCreator == null)
                throw new ArgumentNullException("siteMapNodeCreator");
            this.siteMapNodeCreator = siteMapNodeCreator;
        }
        protected readonly ISiteMapNodeCreator siteMapNodeCreator;


        /// <summary>
        /// Gets the dynamic nodes for node.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="node">The node.</param>
        /// <param name="parentKey">The key of the parent node.</param>
        public virtual IEnumerable<ISiteMapNodeParentMap> BuildDynamicNodes(ISiteMapNode node, string defaultParentKey)
        {
            var result = new List<ISiteMapNodeParentMap>();

            if (!node.HasDynamicNodeProvider)
            {
                return result;
            }

            // Build dynamic nodes
            foreach (var dynamicNode in node.GetDynamicNodeCollection())
            {
                // If the dynamic node has a parent key set, use that as the parent. Otherwise use the parentNode.
                var parentKey = !String.IsNullOrEmpty(dynamicNode.ParentKey) ? dynamicNode.ParentKey : defaultParentKey;
                var key = dynamicNode.Key;

                if (string.IsNullOrEmpty(key))
                {
                    key = this.siteMapNodeCreator.GenerateSiteMapNodeKey(
                        parentKey,
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
                var nodeParentMap = this.siteMapNodeCreator.CreateDynamicSiteMapNode(key, parentKey, node.DynamicNodeProvider, node.ResourceKey);
                var newNode = nodeParentMap.Node;

                // Copy the values from the original node to the new one
                node.CopyTo(newNode);

                // Copy any values that were set in the dynamic node and overwrite the new node.
                dynamicNode.SafeCopyTo(newNode);

                result.Add(nodeParentMap);
            }
            return result;
        }
    }
}