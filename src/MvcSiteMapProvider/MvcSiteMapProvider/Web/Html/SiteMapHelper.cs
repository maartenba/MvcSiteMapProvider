﻿using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Html.Models;
using MvcSiteMapProvider.Collections.Specialized;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class SiteMapHelper
    {
        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper)
        {
            return SiteMap(helper, helper.SiteMap.RootNode);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, object sourceMetadata)
        {
            return SiteMap(helper, helper.SiteMap.RootNode, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMap(helper, helper.SiteMap.RootNode, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode)
        {
            return SiteMap(helper, startingNode, false);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, object sourceMetadata)
        {
            return SiteMap(helper, startingNode, false, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMap(helper, startingNode, false, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, bool rootInChildLevel)
        {
            return SiteMap(helper, helper.SiteMap.RootNode, rootInChildLevel);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, bool rootInChildLevel, object sourceMetadata)
        {
            return SiteMap(helper, helper.SiteMap.RootNode, rootInChildLevel, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, bool rootInChildLevel, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMap(helper, helper.SiteMap.RootNode, rootInChildLevel, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool rootInChildLevel)
        {
            return SiteMap(helper, null, startingNode, rootInChildLevel);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool rootInChildLevel, object sourceMetadata)
        {
            return SiteMap(helper, null, startingNode, rootInChildLevel, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool rootInChildLevel, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMap(helper, null, startingNode, rootInChildLevel, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName)
        {
            return SiteMap(helper, templateName, helper.SiteMap.RootNode);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, object sourceMetadata)
        {
            return SiteMap(helper, templateName, helper.SiteMap.RootNode, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMap(helper, templateName, helper.SiteMap.RootNode, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode)
        {
            return SiteMap(helper, templateName, startingNode, false);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, object sourceMetadata)
        {
            return SiteMap(helper, templateName, startingNode, false, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMap(helper, templateName, startingNode, false, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, bool rootInChildLevel)
        {
            return SiteMap(helper, templateName, helper.SiteMap.RootNode, rootInChildLevel);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, bool rootInChildLevel, object sourceMetaData)
        {
            return SiteMap(helper, templateName, helper.SiteMap.RootNode, rootInChildLevel, sourceMetaData);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, bool rootInChildLevel, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMap(helper, templateName, helper.SiteMap.RootNode, rootInChildLevel, sourceMetadata);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool rootInChildLevel)
        {
            return SiteMap(helper, templateName, startingNode, rootInChildLevel, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool rootInChildLevel, object sourceMetadata)
        {
            return SiteMap(helper, templateName, startingNode, rootInChildLevel, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, bool rootInChildLevel, SourceMetadataDictionary sourceMetadata)
        {
            var model = BuildModel(helper, GetSourceMetadata(sourceMetadata), startingNode, rootInChildLevel);
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
        /// <returns>The model.</returns>
        private static SiteMapHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata, ISiteMapNode startingNode, bool startingNodeInChildLevel)
        {
            // Build model
            var model = new SiteMapHelperModel();
            var node = startingNode;

            // Check visibility and ACL
            if (node != null && node.IsVisible(sourceMetadata) && node.IsAccessibleToUser())
            {
                // Add node
                var nodeToAdd = new SiteMapNodeModel(node, sourceMetadata);
                model.Nodes.Add(nodeToAdd);

                // Add child nodes
                if (startingNodeInChildLevel)
                {
                    model.Nodes.AddRange(nodeToAdd.Descendants);
                }
            }

            return model;
        }

        /// <summary>
        /// Gets the source meta data for the current context.
        /// </summary>
        /// <param name="sourceMetadata">User-defined metadata.</param>
        /// <returns>SourceMetadataDictionary for the current request.</returns>
        private static SourceMetadataDictionary GetSourceMetadata(IDictionary<string, object> sourceMetadata)
        {
            var result = new SourceMetadataDictionary(sourceMetadata);
            result.Add("HtmlHelper", typeof(SiteMapHelper).FullName);
            return result;
        }
    }
}
