#region Using directives

using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MvcSiteMapProvider.Core.Web.Html.Models;
using MvcSiteMapProvider.Core.SiteMap;
using System.Collections.Specialized;

#endregion

namespace MvcSiteMapProvider.Core.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class SiteMapTitleHelper
    {
        /// <summary>
        /// Source metadata
        /// </summary>
        private static readonly Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(SiteMapTitleHelper).FullName } };

        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>
        /// The title of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper)
        {
            return SiteMapTitle(helper, null);
        }

        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>
        /// The title of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper, string templateName)
        {
            var model = BuildModel(helper.SiteMap.CurrentNode ?? helper.SiteMap.RootNode);
            return helper
                .CreateHtmlHelperForModel(model)
                .DisplayFor(m => model, templateName);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="startingNode">The starting node.</param>
        /// <returns>The model.</returns>
        private static SiteMapTitleHelperModel BuildModel(ISiteMapNode startingNode)
        {
            // Map to model
            //var mvcNode = startingNode as MvcSiteMapNode;
            return new SiteMapTitleHelperModel
            {
                CurrentNode = SiteMapNodeModelMapper.MapToSiteMapNodeModel(startingNode, SourceMetadata)
            };
        }
    }
}
