using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcSiteMapProvider.Web.Html.Models;

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class MetaRobotsHelper
    {
        /// <summary>
        /// Source metadata
        /// </summary>
        private static readonly Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(MetaRobotsHelper).FullName } };

        /// <summary>
        /// Gets the content attribute value of the meta robots tag for the SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>
        /// The content attribute value for the meta robots tag of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString MetaRobotsTag(this MvcSiteMapHtmlHelper helper)
        {
            return MetaRobotsTag(helper, null);
        }

        /// <summary>
        /// Gets the content attribute value of the meta robots tag for the SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>
        /// The content attribute value for the meta robots tag of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString MetaRobotsTag(this MvcSiteMapHtmlHelper helper, string templateName)
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
        private static MetaRobotsHelperModel BuildModel(ISiteMapNode startingNode)
        {
            // Map to model
            //var mvcNode = startingNode as MvcSiteMapNode;
            return new MetaRobotsHelperModel
            {
                CurrentNode = SiteMapNodeModelMapper.MapToSiteMapNodeModel(startingNode, SourceMetadata)
            };
        }
    }
}
