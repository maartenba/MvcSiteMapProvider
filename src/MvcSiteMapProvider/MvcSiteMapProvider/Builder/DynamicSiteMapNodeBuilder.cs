using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.DI;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Creates nodes with a map to their parent node dynamically based on an implementation of <see cref="T:MvcSiteMapProvider.IDynamicNodeProvider"/>.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class DynamicSiteMapNodeBuilder
        : IDynamicSiteMapNodeBuilder
    {
        public DynamicSiteMapNodeBuilder(
            ISiteMapNodeCreator siteMapNodeCreator,
            ICultureContext cultureContext,
            ICultureContextFactory cultureContextFactory
            )
        {
            if (siteMapNodeCreator == null)
                throw new ArgumentNullException("siteMapNodeCreator");
            if (cultureContext == null)
                throw new ArgumentNullException("cultureContext");
            if (cultureContextFactory == null)
                throw new ArgumentNullException("cultureContextFactory");

            this.siteMapNodeCreator = siteMapNodeCreator;
            this.cultureContext = cultureContext;
            this.cultureContextFactory = cultureContextFactory;
        }
        protected readonly ISiteMapNodeCreator siteMapNodeCreator;
        protected readonly ICultureContext cultureContext;
        protected readonly ICultureContextFactory cultureContextFactory;

        /// <summary>
        /// Gets the dynamic nodes for node.
        /// </summary>
        /// <param name="node">The SiteMap node.</param>
        /// <param name="defaultParentKey">The key of the parent node.</param>
        public virtual IEnumerable<ISiteMapNodeToParentRelation> BuildDynamicNodes(ISiteMapNode node, string defaultParentKey)
        {
            var result = new List<ISiteMapNodeToParentRelation>();

            if (!node.HasDynamicNodeProvider)
            {
                return result;
            }

            // Get the dynamic nodes using the request's culture context.
            // NOTE: In version 5, we need to use the invariant context and pass a reference to it
            // into the dynamic node provider. This would be a breaking change, so for now we are 
            // swapping the context back to the state of the current request. This way, the end user
            // still can opt to change to invariant culture, but in the reverse situation there would 
            // be no way to identify the culture of the current request without a reference to the 
            // cultureContext object.
            IEnumerable<DynamicNode> dynamicNodes;
            using (var originalCultureContext = this.cultureContextFactory.Create(this.cultureContext.OriginalCulture, this.cultureContext.OriginalUICulture))
            {
                dynamicNodes = node.GetDynamicNodeCollection();
            }

            // Build dynamic nodes
            foreach (var dynamicNode in dynamicNodes)
            {
                // If the dynamic node has a parent key set, use that as the parent. Otherwise use the parentNode.
                var parentKey = !string.IsNullOrEmpty(dynamicNode.ParentKey) ? dynamicNode.ParentKey : defaultParentKey;
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