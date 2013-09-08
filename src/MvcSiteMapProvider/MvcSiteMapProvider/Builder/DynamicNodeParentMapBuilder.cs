using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Creates nodes with a map to their parent node dynamically based on an implemenation of <see cref="T:MvcSiteMapProvider.IDynamicNodeProvider"/>.
    /// </summary>
    public class DynamicNodeParentMapBuilder
        : IDynamicNodeParentMapBuilder
    {
        public DynamicNodeParentMapBuilder(
            ISiteMapNodeCreationService siteMapNodeCreationService
            )
        {
            if (siteMapNodeCreationService == null)
                throw new ArgumentNullException("siteMapNodeCreationService");
            this.siteMapNodeCreationService = siteMapNodeCreationService;
        }
        protected readonly ISiteMapNodeCreationService siteMapNodeCreationService;


        /// <summary>
        /// Gets the dynamic nodes for node.
        /// </summary>
        /// <param name="siteMap">The site map.</param>
        /// <param name="node">The node.</param>
        /// <param name="parentKey">The key of the parent node.</param>
        public virtual IEnumerable<ISiteMapNodeParentMap> BuildDynamicNodeParentMaps(ISiteMap siteMap, ISiteMapNode node, string parentKey)
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
                var dynamicNodeParentKey = !String.IsNullOrEmpty(dynamicNode.ParentKey) ? dynamicNode.ParentKey : parentKey;
                var key = dynamicNode.Key;

                if (string.IsNullOrEmpty(key))
                {
                    key = this.siteMapNodeCreationService.GenerateSiteMapNodeKey(
                        dynamicNodeParentKey,
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
                var newNode = this.siteMapNodeCreationService.CreateSiteMapNode(siteMap, key, node.ResourceKey);

                // Copy the values from the original node to the new one
                node.CopyTo(newNode);

                // Remove the Dynamic Node Provider
                newNode.DynamicNodeProvider = String.Empty;

                // Copy any values that were set in the dynamic node and overwrite the new node.
                dynamicNode.SafeCopyTo(newNode);

                result.Add(this.siteMapNodeCreationService.CreateSiteMapNodeParentMap(dynamicNodeParentKey, newNode, node.DynamicNodeProvider));
            }
            return result;
        }
    }
}