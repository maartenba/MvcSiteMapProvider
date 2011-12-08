#region Using directives

using System.Collections.Generic;
using MvcSiteMapProvider.Web.Html.Models;
using System.Web;

#endregion

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// SiteMapNodeModelMapper
    /// </summary>
    public static class SiteMapNodeModelMapper
    {
        /// <summary>
        /// Maps to SiteMapNodeModel.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="mvcNode">The MVC node.</param>
        /// <param name="sourceMetadata">The source metadata provided by the HtmlHelper.</param>
        /// <returns>SiteMapNodeModel instance.</returns>
        public static SiteMapNodeModel MapToSiteMapNodeModel(SiteMapNode node, MvcSiteMapNode mvcNode, IDictionary<string, object> sourceMetadata)
        {
            var nodeToAdd = new SiteMapNodeModel
            {
                Area = (mvcNode != null ? mvcNode.Area : ""),
                Controller = (mvcNode != null ? mvcNode.Controller : ""),
                Action = (mvcNode != null ? mvcNode.Action : ""),
                Title = node.Title,
                Description = node.Description,
                TargetFrame = (mvcNode == null ? "" : mvcNode.TargetFrame),
                ImageUrl = (mvcNode == null ? "" : mvcNode.ImageUrl),
                Url = node.Url,
                IsCurrentNode = node == node.Provider.CurrentNode,
                IsInCurrentPath = node.IsInCurrentPath(),
                IsRootNode = node == node.Provider.RootNode,
                IsClickable = (mvcNode == null || mvcNode.Clickable),
                RouteValues = (mvcNode != null ? mvcNode.RouteValues : new Dictionary<string, object>()),
                MetaAttributes = (mvcNode != null ? mvcNode.MetaAttributes : new Dictionary<string, string>()),
                SourceMetadata = sourceMetadata
            };
            return nodeToAdd;
        }
    }
}
