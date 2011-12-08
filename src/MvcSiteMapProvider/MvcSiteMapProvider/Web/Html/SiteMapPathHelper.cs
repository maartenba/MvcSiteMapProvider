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
    public static class SiteMapPathHelper
    {
        /// <summary>
        /// Source metadata
        /// </summary>
        private static Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(SiteMapPathHelper).FullName } };

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper)
        {
            return SiteMapPath(helper, null);
        }

        /// <summary>
        /// Gets SiteMap path for the current request
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>SiteMap path for the current request</returns>
        public static MvcHtmlString SiteMapPath(this MvcSiteMapHtmlHelper helper, string templateName)
        {
            var model = BuildModel(helper, helper.Provider.CurrentNode);
            return helper
                .CreateHtmlHelperForModel(model)
                .DisplayFor(m => model, templateName);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>The model.</returns>
        private static SiteMapPathHelperModel BuildModel(MvcSiteMapHtmlHelper helper, SiteMapNode startingNode)
        {
            // Build model
            var model = new SiteMapPathHelperModel();
            var node = startingNode;
            while (node != null)
            {
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
                }
                node = node.ParentNode;
            }
            model.Nodes.Reverse();

            return model;
        }
    }
}