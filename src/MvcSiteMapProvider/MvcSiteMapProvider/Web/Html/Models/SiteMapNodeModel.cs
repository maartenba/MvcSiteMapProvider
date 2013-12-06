﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Web.Html.Models
{
    /// <summary>
    /// SiteMapNodeModel
    /// </summary>
    public class SiteMapNodeModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeModel"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata provided by the HtmlHelper.</param>
        public SiteMapNodeModel(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
            : this(node, sourceMetadata, Int32.MaxValue, true, false, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeModel"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata provided by the HtmlHelper.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <param name="startingNodeInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        public SiteMapNodeModel(ISiteMapNode node, IDictionary<string, object> sourceMetadata, int maxDepth, bool drillDownToCurrent, bool startingNodeInChildLevel, bool visibilityAffectsDescendants)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (sourceMetadata == null)
                throw new ArgumentNullException("sourceMetadata");
            if (maxDepth < 0)
                throw new ArgumentOutOfRangeException("maxDepth");

            this.node = node;
            this.maxDepth = maxDepth;
            this.startingNodeInChildLevel = startingNodeInChildLevel;
            this.drillDownToCurrent = drillDownToCurrent;
            this.SourceMetadata = sourceMetadata;

            Key = node.Key;
            Area = node.Area;
            Controller = node.Controller;
            Action = node.Action;
            Title = node.Title;
            Description = node.Description;
            TargetFrame = node.TargetFrame;
            ImageUrl = node.ImageUrl;
            Url = node.Url;
            CanonicalUrl = node.CanonicalUrl;
            MetaRobotsContent = node.GetMetaRobotsContentString();
            IsCurrentNode = (node == node.SiteMap.CurrentNode);
            IsInCurrentPath = node.IsInCurrentPath();
            IsRootNode = (node == node.SiteMap.RootNode);
            IsClickable = node.Clickable;
            VisibilityAffectsDescendants = visibilityAffectsDescendants;
            RouteValues = node.RouteValues;
            Attributes = node.Attributes;
            Order = node.Order;
        }

        protected readonly ISiteMapNode node;
        protected int maxDepth;
        protected readonly bool startingNodeInChildLevel;
        protected readonly bool drillDownToCurrent;
        protected SiteMapNodeModelList descendants;
        protected SiteMapNodeModelList ancestors;
        protected SiteMapNodeModelList children;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; protected set; }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        /// <value>The area.</value>
        public string Area { get; protected set; }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public string Controller { get; protected set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; protected set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; protected set; }

        /// <summary>
        /// Gets or sets the canonical URL.
        /// </summary>
        /// <value>The canonical URL.</value>
        public string CanonicalUrl { get; protected set; }

        /// <summary>
        /// Gets or sets the content value of the meta robots tag.
        /// </summary>
        /// <value>The content value of the meta robots tag.</value>
        public string MetaRobotsContent { get; protected set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; protected set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; protected set; }

        /// <summary>
        /// Gets or sets the target frame.
        /// </summary>
        /// <value>The target frame.</value>
        public string TargetFrame { get; protected set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is current node.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is current node; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrentNode { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in current path.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is in current path; otherwise, <c>false</c>.
        /// </value>
        public bool IsInCurrentPath { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is root node.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is root node; otherwise, <c>false</c>.
        /// </value>
        public bool IsRootNode { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is clickable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is clickable; otherwise, <c>false</c>.
        /// </value>
        public bool IsClickable { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the visibility property of the current node
        /// will affect the descendant nodes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if visibility should affect descendants; otherwise, <c>false</c>.
        /// </value>
        public bool VisibilityAffectsDescendants { get; protected set; }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        /// <value>The route values.</value>
        public IDictionary<string, object> RouteValues { get; protected set; }

        /// <summary>
        /// Gets or sets the meta attributes.
        /// </summary>
        /// <value>The meta attributes.</value>
        public IDictionary<string, object> Attributes { get; protected set; }

        /// <summary>
        /// Gets or sets the source metadata generated by the HtmlHelper.
        /// </summary>
        /// <value>The source metadata.</value>
        public IDictionary<string, object> SourceMetadata { get; protected set; }

        /// <summary>
        /// Gets the order of the node relative to its sibling nodes.
        /// </summary>
        public int Order { get; protected set; }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public SiteMapNodeModelList Children
        {
            get
            {
                if (children == null)
                {
                    children = new SiteMapNodeModelList();
                    if (ReachedMaximalNodelevel(maxDepth, node, drillDownToCurrent) && node.HasChildNodes)
                    {
                        IEnumerable<ISiteMapNode> sortedNodes;
                        if (node.ChildNodes.Any(x => x.Order != 0))
                        {
                            sortedNodes = node.ChildNodes.OrderBy(x => x.Order);
                        }
                        else
                        {
                            sortedNodes = node.ChildNodes;
                        }

                        if (VisibilityAffectsDescendants)
                        {
                            foreach (var child in sortedNodes)
                            {
                                if (child.IsAccessibleToUser() && child.IsVisible(SourceMetadata) && maxDepth > 0)
                                {
                                    children.Add(new SiteMapNodeModel(child, SourceMetadata, maxDepth - 1, drillDownToCurrent, false, VisibilityAffectsDescendants));
                                }
                            }
                        }
                        else
                        {
                            nearestVisibleDescendantsList = new SiteMapNodeModelList();
                            foreach (var child in sortedNodes)
                            {
                                if (child.IsAccessibleToUser())
                                {
                                    if (child.IsVisible(SourceMetadata) && maxDepth > 0)
                                    {
                                        children.Add(new SiteMapNodeModel(child, SourceMetadata, maxDepth - 1, drillDownToCurrent, false, VisibilityAffectsDescendants));
                                    }
                                    else if (maxDepth > 1)//maxDepth should be greater then 1 to be allowed to descent another level
                                    {
                                        FindNearestVisibleDescendants(child, maxDepth - 1);

                                        IEnumerable<SiteMapNodeModel> sortedDescendants;
                                        if (nearestVisibleDescendantsList.Any(x => x.Order != 0))
                                        {
                                            sortedDescendants = nearestVisibleDescendantsList.OrderBy(x => x.Order);
                                        }
                                        else
                                        {
                                            sortedDescendants = nearestVisibleDescendantsList;
                                        }
                                        children.AddRange(sortedDescendants);
                                        nearestVisibleDescendantsList = new SiteMapNodeModelList();
                                    }
                                }
                            }
                        }
                    }
                }
                if (startingNodeInChildLevel)
                {
                    var children_res = children;
                    children = new SiteMapNodeModelList();
                    maxDepth = 0;
                    return children_res;
                }
                else return children;
            }
        }

        private SiteMapNodeModelList nearestVisibleDescendantsList;
        private void FindNearestVisibleDescendants(ISiteMapNode node, int maxDepth)
        {
            foreach (var child in node.ChildNodes)
            {
                if (child.IsAccessibleToUser())
                {
                    if (child.IsVisible(SourceMetadata))
                    {
                        nearestVisibleDescendantsList.Add(new SiteMapNodeModel(child, SourceMetadata, maxDepth - 1, drillDownToCurrent, false, VisibilityAffectsDescendants));
                    }
                    else if (maxDepth > 1)//maxDepth should be greater then 1 to be allowed to descent another level
                    {
                        FindNearestVisibleDescendants(child, maxDepth - 1);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the parent
        /// </summary>
        public SiteMapNodeModel Parent
        {
            get
            {
                return node.ParentNode == null
                    ? null
                    : new SiteMapNodeModel(node.ParentNode, SourceMetadata, maxDepth == Int32.MaxValue ? Int32.MaxValue : maxDepth + 1, drillDownToCurrent, startingNodeInChildLevel, VisibilityAffectsDescendants);
            }
        }

        /// <summary>
        /// Gets the descendants.
        /// </summary>
        public SiteMapNodeModelList Descendants
        {
            get
            {
                if (descendants == null)
                {
                    descendants = new SiteMapNodeModelList();
                    GetDescendants(this);
                }
                return descendants;
            }
        }

        /// <summary>
        /// Gets the ancestors.
        /// </summary>
        public SiteMapNodeModelList Ancestors
        {
            get
            {
                if (ancestors == null)
                {
                    ancestors = new SiteMapNodeModelList();
                    GetAncestors(this);
                }
                return ancestors;
            }
        }

        /// <summary>
        /// Test if the maximal nodelevel has not been reached
        /// </summary>
        /// <param name="maxDepth">The normal max depth.</param>
        /// <param name="node">The starting node</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns></returns>
        private bool ReachedMaximalNodelevel(int maxDepth, ISiteMapNode node, bool drillDownToCurrent)
        {
            if (maxDepth > 0)
                return true;
            if (!drillDownToCurrent)
                return false;
            if (node.IsInCurrentPath())
                return true;
            if (node.ParentNode == node.SiteMap.CurrentNode)
                return true;
            foreach (ISiteMapNode sibling in node.ParentNode.ChildNodes)
            {
                if (sibling.IsInCurrentPath())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieve all descendants    
        /// </summary>
        /// <param name="node">the node</param>
        /// <returns></returns>
        private void GetDescendants(SiteMapNodeModel node)
        {
            IEnumerable<SiteMapNodeModel> sortedNodes;
            if (node.Children.Any(x => x.Order != 0))
            {
                sortedNodes = node.Children.OrderBy(x => x.Order);
            }
            else
            {
                sortedNodes = node.Children;
            }
            foreach (var child in sortedNodes)
            {
                descendants.Add(child);
                GetDescendants(child);
            }
        }

        /// <summary>
        /// Retrieve all ancestors  
        /// </summary>
        /// <param name="node">the node</param>
        /// <returns></returns>
        private void GetAncestors(SiteMapNodeModel node)
        {
            if (node.Parent != null)
            {
                ancestors.Add(node.Parent);
                GetAncestors(node.Parent);
            }
        }
    }
}
