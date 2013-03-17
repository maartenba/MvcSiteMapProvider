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
    public static class CanonicalHelper
    {
        /// <summary>
        /// Source metadata
        /// </summary>
        private static readonly Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(CanonicalHelper).FullName } };

        /// <summary>
        /// Gets the CanonicalUrl of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>
        /// The CanonicalUrl of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString CanonicalTag(this MvcSiteMapHtmlHelper helper)
        {
            return CanonicalTag(helper, null);
        }

        /// <summary>
        /// Gets the CanonicalUrl of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>
        /// The CanonicalUrl of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString CanonicalTag(this MvcSiteMapHtmlHelper helper, string templateName)
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
        private static CanonicalHelperModel BuildModel(ISiteMapNode startingNode)
        {
            // Map to model
            return new CanonicalHelperModel
            {
                CurrentNode = SiteMapNodeModelMapper.MapToSiteMapNodeModel(startingNode, SourceMetadata)
            };
        }
    }
}
