using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Html.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class SiteMapTitleHelper
    {
        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>The title of the CurrentNode or the RootNode (if CurrentNode is null)</returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper)
        {
            return SiteMapTitle(helper, null, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>The title of the CurrentNode or the RootNode (if CurrentNode is null)</returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper, object sourceMetadata)
        {
            return SiteMapTitle(helper, null, sourceMetadata);
        }

        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>The title of the CurrentNode or the RootNode (if CurrentNode is null)</returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata)
        {
            return SiteMapTitle(helper, null, sourceMetadata);
        }

        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>The title of the CurrentNode or the RootNode (if CurrentNode is null)</returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper, string templateName)
        {
            return SiteMapTitle(helper, templateName, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>The title of the CurrentNode or the RootNode (if CurrentNode is null)</returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper, string templateName, object sourceMetadata)
        {
            return SiteMapTitle(helper, templateName, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Gets the title of SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>The title of the CurrentNode or the RootNode (if CurrentNode is null)</returns>
        public static MvcHtmlString SiteMapTitle(this MvcSiteMapHtmlHelper helper, string templateName, SourceMetadataDictionary sourceMetadata)
        {
            var model = BuildModel(GetSourceMetadata(sourceMetadata), helper.SiteMap.CurrentNode ?? helper.SiteMap.RootNode);
            return helper
                .CreateHtmlHelperForModel(model)
                .DisplayFor(m => model, templateName);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="sourceMetadata">User-defined meta data.</param>
        /// <returns>The model.</returns>
        private static SiteMapTitleHelperModel BuildModel(SourceMetadataDictionary sourceMetadata, ISiteMapNode startingNode)
        {
            // Map to model
            return new SiteMapTitleHelperModel
            {
                CurrentNode = new SiteMapNodeModel(startingNode, sourceMetadata)
            };
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
                result.Add("HtmlHelper", typeof(SiteMapTitleHelper).FullName);
            return result;
        }
    }
}
