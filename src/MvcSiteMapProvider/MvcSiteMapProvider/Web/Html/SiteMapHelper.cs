#region Using directives

using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Html.Models;
using System.Collections.Specialized;

#endregion

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
        private static Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(SiteMapHelper).FullName } };

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper)
        {
            return SiteMap(helper, helper.Provider.RootNode);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, SiteMapNode startingNode)
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
            return SiteMap(helper, helper.Provider.RootNode, rootInChildLevel);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, SiteMapNode startingNode, bool rootInChildLevel)
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
            return SiteMap(helper, templateName, helper.Provider.RootNode);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, SiteMapNode startingNode)
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
            return SiteMap(helper, templateName, helper.Provider.RootNode, false);
        }

        /// <summary>
        /// Build a sitemap tree, based on the MvcSiteMap
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="rootInChildLevel">Renders startingNode in child level if set to <c>true</c>.</param>
        /// <returns>Html markup</returns>
        public static MvcHtmlString SiteMap(this MvcSiteMapHtmlHelper helper, string templateName, SiteMapNode startingNode, bool rootInChildLevel)
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
        private static SiteMapHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SiteMapNode startingNode, bool startingNodeInChildLevel)
        {
            // Build model
            var model = new SiteMapHelperModel();
            var node = startingNode;

                var mvcNode = node as MvcSiteMapNode;

                // Check visibility
                bool nodeVisible = true;
                if (mvcNode != null)
                {
                    nodeVisible = mvcNode.VisibilityProvider.IsVisible(
                        node, HttpContext.Current, SourceMetadata);
                }

                // Check ACL
                if (nodeVisible && node.IsAccessibleToUser(HttpContext.Current))
                {
                    // Add node
                    var nodeToAdd = SiteMapNodeModelMapper.MapToSiteMapNodeModel(node, mvcNode, SourceMetadata);
                    model.Nodes.Add(nodeToAdd);

                    // Add child nodes
                    if (node.HasChildNodes) {
                        foreach (SiteMapNode childNode in node.ChildNodes)
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
