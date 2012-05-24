﻿#region Using directives

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcSiteMapProvider.Web.Html.Models;

#endregion

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// Source metadata
        /// </summary>
        private static Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(MenuHelper).FullName } };

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper)
        {
            return Menu(helper, helper.Provider.RootNode, true, true, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool showStartingNode)
        {
            return Menu(helper, helper.Provider.RootNode, true, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode)
        {
            SiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.Provider) : helper.Provider.RootNode;
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToContent">if set to <c>true</c> [drill down to content].</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToContent)
        {
            SiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.Provider), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, startingNode, true, false, maxDepth + 1, drillDownToContent);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth)
        {
            return Menu(helper, startingNodeLevel, maxDepth, false, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToContent">if set to <c>true</c> [drill down to content].</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToContent)
        {
            SiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.Provider), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth + 1, drillDownToContent);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, false, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToContent">if set to <c>true</c> [drill down to content].</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToContent)
        {
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToContent);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToContent">if set to <c>true</c> [drill down to content].</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName)
        {
            return Menu(helper, templateName, helper.Provider.RootNode, true, true, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool showStartingNode)
        {
            return Menu(helper, templateName, helper.Provider.RootNode, true, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode)
        {
            SiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.Provider) : helper.Provider.RootNode;
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent)
        {
            SiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.Provider), startingNodeLevel, false);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, templateName, startingNode, true, true, maxDepth + 1, drillDownToCurrent);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, int maxDepth)
        {
            return Menu(helper, templateName, startingNodeLevel, maxDepth, false, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent)
        {
            SiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.Provider), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth + 1, drillDownToCurrent);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return Menu(helper, templateName, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, false, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent)
        {
            var model = BuildModel(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent);
            return helper
                .CreateHtmlHelperForModel(model)
                .DisplayFor(m => model, templateName);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns>The model.</returns>
        private static MenuHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent)
        {
            // Build model
            var model = new MenuHelperModel();
            var node = startingNode;

            // Check if a starting node has been given
            if (node == null)
            {
                return model;
            }

            var mvcNode = node as MvcSiteMapNode;
            bool continueBuilding = ReachedMaximalNodelevel(maxDepth, node, drillDownToCurrent);

            // Check if maximal node level has not been reached
            if (!continueBuilding)
            {
                return model;
            }

            // Check visibility
            bool nodeVisible = true;
            if (mvcNode != null)
            {
                nodeVisible = mvcNode.VisibilityProvider.IsVisible(
                    node, HttpContext.Current, SourceMetadata);
            }

            // Check ACL
            if (node.IsAccessibleToUser(HttpContext.Current))
            {
                // Add node?
                var nodeToAdd = SiteMapNodeModelMapper.MapToSiteMapNodeModel(node, mvcNode, SourceMetadata);
                if (nodeVisible)
                {
                    if (showStartingNode || !startingNodeInChildLevel)
                    {
                        model.Nodes.Add(nodeToAdd);
                    }
                }

                // Add child nodes
                if (node.HasChildNodes)
                {
                    foreach (SiteMapNode childNode in node.ChildNodes)
                    {
                        var childNodes = BuildModel(helper, childNode, false, true, maxDepth - 1, drillDownToCurrent).Nodes;
                        foreach (var childNodeToAdd in childNodes)
                        {
                            if (!startingNodeInChildLevel)
                            {
                                nodeToAdd.Children.Add(childNodeToAdd);
                            }
                            else
                            {
                                model.Nodes.Add(childNodeToAdd);
                            }
                        }
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>The model.</returns>
        private static MenuHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return BuildModel(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false);
        }

        /// <summary>
        /// This determines the deepest node matching the current HTTP context, so if the current URL describes a location
        /// deeper than the site map designates, it will determine the closest parent to the current URL and return that 
        /// as the current node. This allows menu relevence when navigating deeper than the sitemap structure designates, such
        /// as when navigating to MVC actions, which are not shown in the menus
        /// </summary>
        /// <param name="selectedSiteMapProvider">the current MVC Site Map Provider</param>
        /// <returns></returns>
        public static SiteMapNode GetCurrentNode(SiteMapProvider selectedSiteMapProvider)
        {
            // get the node matching the current URL location
            var currentNode = selectedSiteMapProvider.CurrentNode;

            // if there is no node matching the current URL path, 
            // remove parts until we get a hit
            if (currentNode == null)
            {
                var url = HttpContext.Current.Request.Url.LocalPath;

                while (url.Length > 0)
                {
                    // see if we can find a matching node
                    currentNode = selectedSiteMapProvider.FindSiteMapNode(url);

                    // if we get a hit, stop
                    if (currentNode != null) break;

                    // if not, remove the last path item  
                    var lastSlashlocation = url.LastIndexOf("/");
                    if (lastSlashlocation < 0) break; // protects us from malformed URLs
                    url = url.Remove(lastSlashlocation);
                }
            }

            return currentNode;
        }

        /// <summary>
        /// Gets the starting node.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <returns>The starting node.</returns>
        public static SiteMapNode GetStartingNode(SiteMapNode currentNode, int startingNodeLevel, bool allowForwardSearch)
        {
            SiteMapNode startingNode = GetNodeAtLevel(currentNode, startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return null;
            }
            else if (startingNode.ParentNode != null)
            {
                startingNode = startingNode.ParentNode;
            }
            return startingNode;
        }

        /// <summary>
        /// Gets the starting node.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <returns>The starting node.</returns>
        public static SiteMapNode GetStartingNode(SiteMapNode currentNode, int startingNodeLevel)
        {
            return GetStartingNode(currentNode, startingNodeLevel, false);
        }

        /// <summary>
        /// Gets the node at level.
        /// </summary>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="level">The level.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <returns>The node at level.</returns>
        public static SiteMapNode GetNodeAtLevel(SiteMapNode startingNode, int level, bool allowForwardSearch)
        {
            var startingNodeLevel = startingNode.GetNodeLevel();
            if (startingNodeLevel == level)
            {
                return startingNode;
            }
            else if (startingNodeLevel > level)
            {
                var node = startingNode;
                while (node != null)
                {
                    if (node.GetNodeLevel() == level)
                    {
                        return node;
                    }
                    node = node.ParentNode;
                }
            }
            else if (startingNodeLevel < level && allowForwardSearch)
            {
                var node = startingNode;
                while (node != null)
                {
                    if (node.GetNodeLevel() == level)
                    {
                        return node;
                    }
                    if (node.HasChildNodes)
                    {
                        node = node.ChildNodes[0];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (startingNode != null && startingNode.Provider.RootNode != null && allowForwardSearch)
            {
                return startingNode.Provider.RootNode;
            }

            return null;
        }

        /// <summary>
        /// Test if the maximal nodelevel has not been reached
        /// </summary>
        /// <param name="maxDepth">The normal max depth.</param>
        /// <param name="node">The starting node</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns></returns>
        private static bool ReachedMaximalNodelevel(int maxDepth, SiteMapNode node, bool drillDownToCurrent)
        {
            if (maxDepth > 0)
                return true;
            if (!drillDownToCurrent)
                return false;
            if (node.IsInCurrentPath())
                return true;
            if (node.ParentNode == node.Provider.CurrentNode)
                return true;
            foreach (SiteMapNode sibling in node.ParentNode.ChildNodes)
            {
                if (sibling.IsInCurrentPath())
                    return true;
            }
            return false;
        }
    }
}
