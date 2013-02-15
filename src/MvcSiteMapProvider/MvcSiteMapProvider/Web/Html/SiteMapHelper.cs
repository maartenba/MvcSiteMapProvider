using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Html.Models;
using MvcSiteMapProvider;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class SiteMapHelper
    {
        /// <summary>
        /// Source metadata
        /// </summary>
        private static readonly Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(SiteMapHelper).FullName } };

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
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, bool rootInChildLevel)
        {
            return SiteMap(helper, templateName, helper.SiteMap.RootNode, false);
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
            var model = BuildModel(helper, startingNode, rootInChildLevel);
            return helper
                .CreateHtmlHelperForModel(model)
                .DisplayFor(m => model, templateName);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="startingNodeInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>The model.</returns>
        private static SiteMapHelperModel BuildModel(MvcSiteMapHtmlHelper helper, ISiteMapNode startingNode, bool startingNodeInChildLevel)
        {
            // Build model
            var model = new SiteMapHelperModel();
            var node = startingNode;

            // Check visibility
            bool nodeVisible = true;
            if (node != null)
            {
                nodeVisible = node.IsVisible(SourceMetadata);
            }

            // Check ACL
            if (nodeVisible && node.IsAccessibleToUser())
            {
                // Add node
                var nodeToAdd = SiteMapNodeModelMapper.MapToSiteMapNodeModel(node, SourceMetadata);
                model.Nodes.Add(nodeToAdd);

                // Add child nodes
                if (node.HasChildNodes) {
                    foreach (ISiteMapNode childNode in node.ChildNodes)
                    {
                        foreach (var childNodeToAdd in BuildModel(helper, childNode, false).Nodes)
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
    }
}
