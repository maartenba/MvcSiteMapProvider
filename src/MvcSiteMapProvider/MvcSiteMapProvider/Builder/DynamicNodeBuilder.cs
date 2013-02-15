using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MvcSiteMapProvider;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// TODO: Update summary.
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
            foreach (var dynamicNode in node.GetDynamicNodeCollection().ToList())
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

                var clone = siteMapNodeFactory.CreateDynamic(siteMap, key, node.ResourceKey);


                // TODO: figure out a better way to create a dynamic node.

                // Begin Clone


                foreach (var attribute in node.Attributes)
                {
                    clone.Attributes[attribute.Key] = attribute.Value;
                }
                foreach (var route in node.RouteValues)
                {
                    clone.RouteValues[route.Key] = route.Value;
                }
                foreach (var param in node.PreservedRouteParameters)
                {
                    clone.PreservedRouteParameters.Add(param);
                }
                foreach (var role in node.Roles)
                {
                    clone.Roles.Add(role);
                }
                clone.Area = node.Area;
                clone.Action = node.Action;
                clone.Controller = node.Controller;
                clone.Route = node.Route;
                clone.VisibilityProvider = node.VisibilityProvider;
                clone.UrlResolver = node.UrlResolver;
                clone.Url = node.UnresolvedUrl;
                clone.DynamicNodeProvider = node.DynamicNodeProvider;
                clone.Clickable = node.Clickable;

                // End Clone



                foreach (var kvp in dynamicNode.RouteValues)
                {
                    clone.RouteValues[kvp.Key] = kvp.Value;
                }
                foreach (var kvp in dynamicNode.Attributes)
                {
                    clone.Attributes[kvp.Key] = kvp.Value;
                }

                if (!string.IsNullOrEmpty(dynamicNode.Title))
                {
                    clone.Title = dynamicNode.Title;
                }
                if (!string.IsNullOrEmpty(dynamicNode.Description))
                {
                    clone.Description = dynamicNode.Description;
                }
                if (!string.IsNullOrEmpty(dynamicNode.TargetFrame))
                {
                    clone.TargetFrame = dynamicNode.TargetFrame;
                }
                if (!string.IsNullOrEmpty(dynamicNode.ImageUrl))
                {
                    clone.ImageUrl = dynamicNode.ImageUrl;
                }

                if (!string.IsNullOrEmpty(dynamicNode.Route))
                {
                    clone.Route = dynamicNode.Route;
                }

                if (dynamicNode.LastModifiedDate.HasValue)
                {
                    clone.LastModifiedDate = dynamicNode.LastModifiedDate.Value;
                }

                if (dynamicNode.ChangeFrequency != ChangeFrequency.Undefined)
                {
                    clone.ChangeFrequency = dynamicNode.ChangeFrequency;
                }

                if (dynamicNode.UpdatePriority != UpdatePriority.Undefined)
                {
                    clone.ChangeFrequency = dynamicNode.ChangeFrequency;
                }

                clone.PreservedRouteParameters.Clear();
                if (dynamicNode.PreservedRouteParameters.Any())
                {
                    string[] parameters = dynamicNode.PreservedRouteParameters.ToArray();
                    foreach (var p in parameters)
                    {
                        clone.PreservedRouteParameters.Add(p);
                    }
                }

                if (dynamicNode.Roles != null && dynamicNode.Roles.Any())
                {
                    foreach (var role in dynamicNode.Roles)
                    {
                        clone.Roles.Add(role);
                    }
                }

                // Optionally, an area, controller and action can be specified. If so, override the clone.
                if (!string.IsNullOrEmpty(dynamicNode.Area))
                {
                    clone.Area = dynamicNode.Area;
                }
                if (!string.IsNullOrEmpty(dynamicNode.Controller))
                {
                    clone.Controller = dynamicNode.Controller;
                }
                if (!string.IsNullOrEmpty(dynamicNode.Action))
                {
                    clone.Action = dynamicNode.Action;
                }

                // If the dynamic node has a parent key set, use that as the parent. Otherwise use the parentNode.
                if (dynamicNode.ParentKey != null && !string.IsNullOrEmpty(dynamicNode.ParentKey))
                {
                    var parent = siteMap.FindSiteMapNodeFromKey(dynamicNode.ParentKey);
                    if (parent != null)
                    {
                        siteMap.AddNode(clone, parent);
                        createdDynamicNodes.Add(clone);
                    }
                }
                else
                {
                    siteMap.AddNode(clone, parentNode);
                    createdDynamicNodes.Add(clone);
                }
            }

            // Done!
            return createdDynamicNodes;
        }

    }
}
