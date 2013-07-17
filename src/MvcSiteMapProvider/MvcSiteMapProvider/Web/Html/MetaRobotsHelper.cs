using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcSiteMapProvider.Web.Html.Models;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// MvcSiteMapHtmlHelper extension methods
    /// </summary>
    public static class MetaRobotsHelper
    {
        /// <summary>
        /// Gets the content attribute value of the meta robots tag for the SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>
        /// The content attribute value for the meta robots tag of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString MetaRobotsTag(this MvcSiteMapHtmlHelper helper)
        {
            return MetaRobotsTag(helper, null, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Gets the content attribute value of the meta robots tag for the SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>
        /// The content attribute value for the meta robots tag of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString MetaRobotsTag(this MvcSiteMapHtmlHelper helper, object sourceMetadata)
        {
            return MetaRobotsTag(helper, null, sourceMetadata);
        }

        /// <summary>
        /// Gets the content attribute value of the meta robots tag for the SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <returns>
        /// The content attribute value for the meta robots tag of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString MetaRobotsTag(this MvcSiteMapHtmlHelper helper, SourceMetadataDictionary sourceMetadata)
        {
            return MetaRobotsTag(helper, null, sourceMetadata);
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
            return MetaRobotsTag(helper, templateName, new SourceMetadataDictionary());
        }

        /// <summary>
        /// Gets the content attribute value of the meta robots tag for the SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>
        /// The content attribute value for the meta robots tag of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString MetaRobotsTag(this MvcSiteMapHtmlHelper helper, string templateName, object sourceMetadata)
        {
            return MetaRobotsTag(helper, templateName, new SourceMetadataDictionary(sourceMetadata));
        }

        /// <summary>
        /// Gets the content attribute value of the meta robots tag for the SiteMap.CurrentNode
        /// </summary>
        /// <param name="helper">MvcSiteMapHtmlHelper instance</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>
        /// The content attribute value for the meta robots tag of the CurrentNode or the RootNode (if CurrentNode is null)
        /// </returns>
        public static MvcHtmlString MetaRobotsTag(this MvcSiteMapHtmlHelper helper, string templateName, SourceMetadataDictionary sourceMetadata)
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
        /// <returns>The model.</returns>
        private static MetaRobotsHelperModel BuildModel(SourceMetadataDictionary sourceMetadata, ISiteMapNode startingNode)
        {
            // Map to model
            return new MetaRobotsHelperModel
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
            result.Add("HtmlHelper", typeof(MetaRobotsHelper).FullName);
            return result;
        }
    }
}
