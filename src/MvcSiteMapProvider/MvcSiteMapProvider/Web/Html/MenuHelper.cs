using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Html.Models;

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
            ISiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.SiteMap, true) : helper.SiteMap.RootNode;
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, bool visibilityAffectsDescendants)
        {
            return Menu(helper, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, visibilityAffectsDescendants, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, bool visibilityAffectsDescendants, object sourceMetadata)
        {
            return Menu(helper, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, visibilityAffectsDescendants, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, bool visibilityAffectsDescendants, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.SiteMap, true) : helper.SiteMap.RootNode;
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, visibilityAffectsDescendants, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap, true), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, startingNode, true, false, maxDepth, drillDownToCurrent, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescendants)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToCurrent, visibilityAffectsDescendants, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescendants, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, maxDepth, allowForwardSearch, drillDownToCurrent, visibilityAffectsDescendants, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNodeLevel">The starting node level.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="allowForwardSearch">if set to <c>true</c> allow forward search. Forward search will search all parent nodes and child nodes, where in other circumstances only parent nodes are searched.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescendants, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap, true), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, null, startingNode, true, false, maxDepth, drillDownToCurrent, visibilityAffectsDescendants, sourceMetadata);
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary());
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap, true), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, sourceMetadata);
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescenants)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, visibilityAffectsDescenants, new SourceMetadataDictionary());
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescenants, object sourceMetadata)
        {
            return Menu(helper, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, visibilityAffectsDescenants, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescenants, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap, true), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, visibilityAffectsDescenants, sourceMetadata);
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent)
        {
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, object sourceMetadata)
        {
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, sourceMetadata);
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="maxDepth">The max depth.</param>
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, SourceMetadataDictionary sourceMetadata)
        {
            return Menu(helper, null, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, sourceMetadata);
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
            ISiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.SiteMap, true) : helper.SiteMap.RootNode;
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
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, bool visibilityAffectsDescendants)
        {
            return Menu(helper, templateName, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, visibilityAffectsDescendants, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, bool visibilityAffectsDescendants, object sourceMetadata)
        {
            return Menu(helper, templateName, startFromCurrentNode, startingNodeInChildLevel, showStartingNode, visibilityAffectsDescendants, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a menu, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startFromCurrentNode">Start from current node if set to <c>true</c>.</param>
        /// <param name="startingNodeInChildLevel">Show starting node in child level if set to <c>true</c>.</param>
        /// <param name="showStartingNode">Show starting node if set to <c>true</c>.</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, bool startFromCurrentNode, bool startingNodeInChildLevel, bool showStartingNode, bool visibilityAffectsDescendants, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = startFromCurrentNode ? GetCurrentNode(helper.SiteMap, true) : helper.SiteMap.RootNode;
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, Int32.MaxValue, false, visibilityAffectsDescendants, sourceMetadata);
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
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap, true), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, templateName, startingNode, true, true, maxDepth, drillDownToCurrent, sourceMetadata);
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
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap, true), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, sourceMetadata);
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
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescendants)
        {
            return Menu(helper, templateName, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, visibilityAffectsDescendants, new SourceMetadataDictionary());
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
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescendants, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNodeLevel, startingNodeInChildLevel, showStartingNode, maxDepth, allowForwardSearch, drillDownToCurrent, visibilityAffectsDescendants, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, int startingNodeLevel, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool allowForwardSearch, bool drillDownToCurrent, bool visibilityAffectsDescendants, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode = GetStartingNode(GetCurrentNode(helper.SiteMap, true), startingNodeLevel, allowForwardSearch);
            if (startingNode == null)
            {
                return MvcHtmlString.Empty;
            }
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, visibilityAffectsDescendants, sourceMetadata);
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
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, helper.SiteMap.VisibilityAffectsDescendants, sourceMetadata);
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
        /// <param name="visibilityAffectsDescendants"><c>true</c> if the visibility provider should affect the visibility of descendant nodes, otherwise <c>false</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, bool visibilityAffectsDescendants)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, visibilityAffectsDescendants, new SourceMetadataDictionary());
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
        /// <param name="visibilityAffectsDescendants"><c>true</c> if the visibility provider should affect the visibility of descendant nodes, otherwise <c>false</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, bool visibilityAffectsDescendants, object sourceMetadata)
        {
            return Menu(helper, templateName, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, visibilityAffectsDescendants, new SourceMetadataDictionary(sourceMetadata));
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
        /// <param name="visibilityAffectsDescendants"><c>true</c> if the visibility provider should affect the visibility of descendant nodes, otherwise <c>false</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString Menu(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, bool visibilityAffectsDescendants, SourceMetadataDictionary sourceMetadata)
        {
            var model = BuildModel(helper, GetSourceMetadata(sourceMetadata), startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, drillDownToCurrent, visibilityAffectsDescendants);
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
        /// <param name="drillDownToCurrent">Should the model exceed the maxDepth to reach the current node?</param>
        /// <param name="visibilityAffectsDescendants"><b>true</b> if the visibility provider should affect the current node as well as all descendant nodes; otherwise <b>false</b>.</param>
        /// <returns>The model.</returns>
        internal static MenuHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata, ISiteMapNode startingNode, bool startingNodeInChildLevel, bool showStartingNode, int maxDepth, bool drillDownToCurrent, bool visibilityAffectsDescendants)
        {
            // Build model
            var model = new MenuHelperModel();
            var node = startingNode;

            // Check if a starting node has been given
            if (node == null)
            {
                return model;
            }

            // Check ACL
            if (node.IsAccessibleToUser())
            {
                // Add node?
                var nodeToAdd = new SiteMapNodeModel(node, sourceMetadata, maxDepth, drillDownToCurrent, startingNodeInChildLevel, visibilityAffectsDescendants);

                // Check visibility
                if (node.IsVisible(sourceMetadata))
                {
                    if (showStartingNode || !startingNodeInChildLevel)
                    {
                        model.Nodes.Add(nodeToAdd);
                    }
                    // Add child nodes
                    if (visibilityAffectsDescendants && startingNodeInChildLevel)
                    {
                        model.Nodes.AddRange(nodeToAdd.Children);
                    }
                }
                // Add child nodes
                if (!visibilityAffectsDescendants && startingNodeInChildLevel)
                {
                    model.Nodes.AddRange(nodeToAdd.Children);
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
            return BuildModel(helper, sourceMetadata, startingNode, startingNodeInChildLevel, showStartingNode, maxDepth, false, helper.SiteMap.VisibilityAffectsDescendants);
        }

        /// <summary>
        /// This determines the deepest node matching the current HTTP context, so if the current URL describes a location
        /// deeper than the site map designates, it will determine the closest parent to the current URL and return that 
        /// as the current node. This allows menu relevance when navigating deeper than the sitemap structure designates, such
        /// as when navigating to MVC actions, which are not shown in the menus
        /// </summary>
        /// <param name="selectedSiteMap">The current SiteMap.</param>
        /// <returns></returns>
        public static ISiteMapNode GetCurrentNode(ISiteMap selectedSiteMap)
        {
            return GetCurrentNode(selectedSiteMap, false);
        }

        /// <summary>
        /// This determines the deepest node matching the current HTTP context, so if the current URL describes a location
        /// deeper than the site map designates, it will determine the closest parent to the current URL and return that 
        /// as the current node. This allows menu relevance when navigating deeper than the sitemap structure designates, such
        /// as when navigating to MVC actions, which are not shown in the menus
        /// </summary>
        /// <param name="selectedSiteMap">The current SiteMap.</param>
        /// <param name="returnRootNodeIfNotFound">Whether to return the root node if the current node is null</param>
        /// <returns></returns>
        public static ISiteMapNode GetCurrentNode(ISiteMap selectedSiteMap, bool returnRootNodeIfNotFound)
        {
            // get the node matching the current URL location
            var currentNode = selectedSiteMap.CurrentNode;

            // if there is no node matching the current URL path, 
            // remove parts until we get a hit
            if (currentNode == null)
            {
                var url = HttpContext.Current.Request.Url.LocalPath;
                var queryString = HttpContext.Current.Request.Url.Query;

                while (url.Length > 0)
                {
                    // see if we can find a matching node
                    currentNode = selectedSiteMap.FindSiteMapNode(url + queryString);

                    // if we get a hit, stop
                    if (currentNode != null) break;

                    // if not, remove the last path item  
                    var lastSlashlocation = url.LastIndexOf("/");
                    if (lastSlashlocation < 0) break; // protects us from malformed URLs
                    url = url.Remove(lastSlashlocation);
                }
            }

            // If the current node is still null, return the root node.
            // This is the same way the SiteMap.FindSiteMapNode(rawUrl) method worked in v3.
            if (currentNode == null && returnRootNodeIfNotFound)
            {
                currentNode = selectedSiteMap.RootNode;
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
            if (startingNode == null)
            {
                return null;
            }
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

            return null;
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
