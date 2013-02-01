// -----------------------------------------------------------------------
// <copyright file="DynamicNodeBuilder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using MvcSiteMapProvider.Core;

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

        ///// <summary>
        ///// Determines whether the specified node has dynamic nodes.
        ///// </summary>
        ///// <param name="node">The node.</param>
        ///// <returns>
        ///// 	<c>true</c> if the specified node has dynamic nodes; otherwise, <c>false</c>.
        ///// </returns>
        //protected bool HasDynamicNodes(ISiteMapNode node)
        //{
        //    // Dynamic nodes available?
        //    var mvcNode = node as ISiteMapNode;
        //    if (mvcNode == null || mvcNode.DynamicNodeProvider == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        /// <summary>
        /// Adds the dynamic nodes for node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="parentNode">The parent node.</param>
        public IEnumerable<ISiteMapNode> BuildDynamicNodesFor(ISiteMap siteMap, ISiteMapNode node, ISiteMapNode parentNode)
        {
            // List of dynamic nodes that have been created
            var createdDynamicNodes = new List<ISiteMapNode>();

            // Dynamic nodes available?
            //if (!HasDynamicNodes(node))
            //{
            //    return createdDynamicNodes;
            //}

            if (!node.HasDynamicNodeProvider)
            {
                return createdDynamicNodes;
            }

            // Build dynamic nodes
            var mvcNode = node;
            //foreach (var dynamicNode in mvcNode.DynamicNodeProvider.GetDynamicNodeCollection().ToList())
            foreach (var dynamicNode in mvcNode.GetDynamicNodeCollection().ToList())
            {
                string key = dynamicNode.Key;
                if (string.IsNullOrEmpty(key))
                {
                    key = nodeKeyGenerator.GenerateKey(
                        parentNode == null ? "" : parentNode.Key, 
                        Guid.NewGuid().ToString(), 
                        mvcNode.Url, 
                        mvcNode.Title, 
                        mvcNode.Area, 
                        mvcNode.Controller, 
                        mvcNode.Action,
                        mvcNode.HttpMethod,
                        mvcNode.Clickable);
                }

                //var clone = mvcNode.Clone(key) as SiteMapNode;
                ////foreach (var kvp in dynamicNode.RouteValues)
                ////{
                ////    clone.RouteValues[kvp.Key] = kvp.Value;
                ////}
                ////foreach (var kvp in dynamicNode.Attributes)
                ////{
                ////    clone[kvp.Key] = kvp.Value;
                ////}
                //clone.DynamicNodeProvider = null;
                //clone.IsDynamic = true;

                var clone = siteMapNodeFactory.CreateDynamic(siteMap, mvcNode.ResourceKey);

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
                    //clone.PreservedRouteParameters = String.Join(";", dynamicNode.PreservedRouteParameters.ToArray());
                    string[] parameters = dynamicNode.PreservedRouteParameters.ToArray();
                    foreach (var p in parameters)
                    {
                        clone.PreservedRouteParameters.Add(p);
                    }
                }

                if (dynamicNode.Roles != null && dynamicNode.Roles.Any())
                {
                    clone.Roles = dynamicNode.Roles.ToArray();
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

            //// Insert cache dependency
            //CacheDescription cacheDescription = mvcNode.DynamicNodeProvider.GetCacheDescription();
            //if (cacheDescription != null)
            //{
            //    HttpContext.Current.Cache.Add(
            //        cacheDescription.Key,
            //        "",
            //        cacheDescription.Dependencies,
            //        cacheDescription.AbsoluteExpiration,
            //        cacheDescription.SlidingExpiration,
            //        cacheDescription.Priority,
            //        new CacheItemRemovedCallback(OnSiteMapChanged));
            //}

            // Done!
            return createdDynamicNodes;
        }

    }
}
