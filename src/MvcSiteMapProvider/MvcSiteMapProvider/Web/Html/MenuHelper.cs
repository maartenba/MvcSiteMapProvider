﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcSiteMapProvider.Web.Html.Models;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper)
        {
            return Menu(helper, helper.SiteMap.RootNode, true, true, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, object sourceMetadata)
        {
            return Menu(helper, helper.SiteMap.RootNode, true, true, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, helper.SiteMap.RootNode, true, true, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool showStartingNode)
        {
            return Menu(helper, helper.SiteMap.RootNode, true, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool showStartingNode, object sourceMetadata)
        {
            return Menu(helper, helper.SiteMap.RootNode, true, showStartingNode, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool showStartingNode, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, helper.SiteMap.RootNode, true, showStartingNode, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, object sourceMetadata)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, sourceMetadata);
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
            return Menu(helper, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, object sourceMetadata)
        {
            return Menu(helper, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.SiteMap) : helper.SiteMap.RootNode;
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, sourceMetadata);
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
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToContent, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToContent">if set to <c>true</c> [drill down to content].</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToContent, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToContent, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToContent">if set to <c>true</c> [drill down to content].</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToContent, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, startingNode, true, false, maxDepth + 1, drillDownToContent, sourceMetadata);
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, maxDepth, false, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, maxDepth, false, false, sourceMetadata);
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
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, false, sourceMetadata);
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
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToContent, new SourceMetadataDictionary());
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToContent, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToContent, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToContent, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth + 1, drillDownToContent, sourceMetadata);
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
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, false, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, false, false, sourceMetadata);
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
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToContent)
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToContent, object sourceMetadata)
        {
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToContent, sourceMetadata);
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToContent, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToContent, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, object sourceMetadata)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName)
        {
            return Menu(helper, templateName, helper.SiteMap.RootNode, true, true, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, object sourceMetadata)
        {
            return Menu(helper, templateName, helper.SiteMap.RootNode, true, true, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, templateName, helper.SiteMap.RootNode, true, true, Int32.MaxValue, false, sourceMetadata);
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
            return Menu(helper, templateName, helper.SiteMap.RootNode, true, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool showStartingNode, object sourceMetadata)
        {
            return Menu(helper, templateName, helper.SiteMap.RootNode, true, showStartingNode, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool showStartingNode, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, templateName, helper.SiteMap.RootNode, true, showStartingNode, Int32.MaxValue, false, sourceMetadata);
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
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, sourceMetadata);
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
            return Menu(helper, templateName, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, object sourceMetadata)
        {
            return Menu(helper, templateName, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.SiteMap) : helper.SiteMap.RootNode;
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, sourceMetadata);
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
            return Menu(helper, templateName, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary());
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap), startingNodeLevel, false);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, templateName, startingNode, true, true, maxDepth + 1, drillDownToCurrent, sourceMetadata);
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
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, int maxDepth, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNodeLevel, maxDepth, false, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, int maxDepth, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, templateName, startingNodeLevel, maxDepth, false, false, sourceMetadata);
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
            return Menu(helper, templateName, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary());
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth + 1, drillDownToCurrent, sourceMetadata);
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
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, false, false, sourceMetadata);
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, templateName, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, false, false, sourceMetadata);
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
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false);
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false, sourceMetadata);
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false, sourceMetadata);
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
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, new SourceMetadataDictionary());
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, SourceMetadataDictionary sourceMetadata)
        {
            var model = BuildModel(helper, GetSourceMetadata(sourceMetadata), startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent);
            return helper
                .CreateHtmlHelperForModel(model)
                .DisplayFor(m => model, templateName);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node</param>
        /// <returns>The model.</returns>
        private static MenuHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent)
        {
            // Build model
            var model = new MenuHelperModel();
            var node = startingNode;

            // Check if a starting node has been given
            if (node == null)
            {
                return model;
            }

            bool continueBuilding = ReachedMaximalNodelevel(maxDepth, node, drillDownToCurrent);

            // Check if maximal node level has not been reached
            if (!continueBuilding)
            {
                return model;
            }

            // Check visibility
            bool nodeVisible = true;
            if (node != null)
            {
                nodeVisible = node.IsVisible(sourceMetadata);
            }

            // Check ACL
            if (node.IsAccessibleToUser())
            {
                // Add node?
                var nodeToAdd = SiteMapNodeModelMapper.MapToSiteMapNodeModel(node, sourceMetadata);
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
                    foreach (ISiteMapNode childNode in node.ChildNodes)
                    {
                        var childNodes = BuildModel(helper, sourceMetadata, childNode, false, true, maxDepth - 1, drillDownToCurrent).Nodes;
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
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <returns>The model.</returns>
        private static MenuHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth)
        {
            return BuildModel(helper, sourceMetadata, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false);
        }

        /// <summary>
        /// This determines the deepest node matching the current HTTP context, so if the current URL describes a location
        /// deeper than the site map designates, it will determine the closest parent to the current URL and return that 
        /// as the current node. This allows menu relevence when navigating deeper than the sitemap structure designates, such
        /// as when navigating to MVC actions, which are not shown in the menus
        /// </summary>
        /// <param name="selectedSiteMapProvider">the current MVC Site Map Provider</param>
        /// <returns></returns>
        public static ISiteMapNode GetCurrentNode(ISiteMap selectedSiteMap)
        {
            // get the node matching the current URL location
            var currentNode = selectedSiteMap.CurrentNode;

            // if there is no node matching the current URL path, 
            // remove parts until we get a hit
            if (currentNode == null)
            {
                var url = HttpContext.Current.Request.Url.LocalPath;

                while (url.Length > 0)
                {
                    // see if we can find a matching node
                    currentNode = selectedSiteMap.FindSiteMapNode(url);

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
        public static ISiteMapNode GetStartingNode(ISiteMapNode currentNode, int startingNodeLevel, bool allowForwardSearch)
        {
            ISiteMapNode startingNode = GetNodeAtLevel(currentNode, startingNodeLevel, allowForwardSearch);
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
        public static ISiteMapNode GetStartingNode(ISiteMapNode currentNode, int startingNodeLevel)
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
        public static ISiteMapNode GetNodeAtLevel(ISiteMapNode startingNode, int level, bool allowForwardSearch)
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

            if (startingNode != null && startingNode.SiteMap.RootNode != null && allowForwardSearch)
            {
                return startingNode.SiteMap.RootNode;
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
        private static bool ReachedMaximalNodelevel(int maxDepth, ISiteMapNode node, bool drillDownToCurrent)
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
        /// Gets the source meta data for the current context.
        /// </summary>
        /// <param name="sourceMetadata">User-defined metadata.</param>
        /// <returns>SourceMetadataDictionary for the current request.</returns>
        private static SourceMetadataDictionary GetSourceMetadata(IDictionary<string, object> sourceMetadata)
        {
            var result = new SourceMetadataDictionary(sourceMetadata);
            result.Add("HtmlHelper", typeof(MenuHelper).FullName);
            return result;
        }
    }
}
