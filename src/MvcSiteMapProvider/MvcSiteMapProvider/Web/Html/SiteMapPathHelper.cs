using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Html.Models;

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class SiteMapPathHelper
    {
        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper)
        {
            return SiteMapPath(helper, null, string.Empty);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="startingNode">The starting node (the last node in the site map path).</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode)
        {
            return SiteMapPath(helper, null, startingNode);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, object sourceMetadata)
        {
            return SiteMapPath(helper, null, string.Empty, sourceMetadata);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMapPath(helper, null, sourceMetadata);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName)
        {
            return SiteMapPath(helper, templateName, string.Empty);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, object sourceMetadata)
        {
            return SiteMapPath(helper, templateName, string.Empty, sourceMetadata);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMapPath(helper, templateName, string.Empty, sourceMetadata);
        }


        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeKey">The key of the starting node (the last node in the site map path).</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, string startingNodeKey)
        {
            return SiteMapPath(helper, templateName, startingNodeKey, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeKey">The key of the starting node (the last node in the site map path).</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, string startingNodeKey, object sourceMetadata)
        {
            return SiteMapPath(helper, templateName, startingNodeKey, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNodeKey">The key of the starting node (the last node in the site map path).</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, string startingNodeKey, SourceMetadataDictionary sourceMetadata)
        {
            ISiteMapNode startingNode;
            if (string.IsNullOrEmpty(startingNodeKey))
            {
                startingNode = helper.SiteMap.CurrentNode;
            }
            else
            {
                startingNode = helper.SiteMap.FindSiteMapNodeFromKey(startingNodeKey);
            }
            return SiteMapPath(helper, templateName, startingNode, sourceMetadata);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node (the last node in the site map path).</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode)
        {
            return SiteMapPath(helper, templateName, startingNode, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node (the last node in the site map path).</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, object sourceMetadata)
        {
            return SiteMapPath(helper, templateName, startingNode, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node (the last node in the site map path).</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName, ISiteMapNode startingNode, SourceMetadataDictionary sourceMetadata)
        {
            var model = BuildModel(helper, GetSourceMetadata(sourceMetadata), startingNode);
            return helper
                .CreateHtmlHelperForModel(model)
                .DisplayFor(m => model, templateName);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>The model.</returns>
        private static SiteMapPathHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata, ISiteMapNode startingNode)
        {
            // Build model
            var model = new SiteMapPathHelperModel();
            var node = startingNode;
            while (node != null)
            {
                bool nodeVisible = node.IsVisible(sourceMetadata);
                if (nodeVisible && node.IsAccessibleToUser())
                {
                    var nodeToAdd = new SiteMapNodeModel(node, sourceMetadata);
                    model.Nodes.Add(nodeToAdd);
                }
                node = node.ParentNode;
            }
            model.Nodes.Reverse();

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
            if (!result.ContainsKey("HtmlHelper"))
                result.Add("HtmlHelper", typeof(SiteMapPathHelper).FullName);
            return result;
        }
    }
}