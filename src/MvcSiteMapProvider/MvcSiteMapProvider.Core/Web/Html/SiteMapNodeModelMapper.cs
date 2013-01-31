#region Using directives

using System.Collections.Generic;
using MvcSiteMapProvider.Core.Web.Html.Models;
using MvcSiteMapProvider.Core.SiteMap;
using System.Web;

#endregion

namespace MvcSiteMapProvider.Core.Web.Html
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
        public static SiteMapNodeModel MapToSiteMapNodeModel(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            var nodeToAdd = new SiteMapNodeModel
            {
                Area = (node != null ? node.Area : ""),
                Controller = (node != null ? node.Controller : ""),
                Action = (node != null ? node.Action : ""),
                Title = node.Title,
                Description = node.Description,
                TargetFrame = (node == null ? "" : node.TargetFrame),
                ImageUrl = (node == null ? "" : node.ImageUrl),
                Url = node.Url,
                IsCurrentNode = node == node.SiteMap.CurrentNode,
                IsInCurrentPath = node.IsInCurrentPath(),
                IsRootNode = node == node.SiteMap.RootNode,
                IsClickable = (node == null || node.Clickable),
                RouteValues = (node != null ? node.RouteValues : new Dictionary<string, object>()),
                // TODO: rename to Attributes
                MetaAttributes = (node != null ? node.Attributes : new Dictionary<string, string>()),
                SourceMetadata = sourceMetadata
            };
            return nodeToAdd;
        }

    }
}
