using System;
using System.Collections.Generic;
using System.Web;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Filtered SiteMapNode Visibility Provider.
    /// 
    /// Rules are parsed left-to-right, first match wins. Asterisk can be used to match any control. Exclamation mark can be used to negate a match.
    /// </summary>
    public class FilteredSiteMapNodeVisibilityProvider
        : SiteMapNodeVisibilityProviderBase
    {
        #region ISiteMapNodeVisibilityProvider Members

        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            // Is a visibility attribute specified?
            string visibility = string.Empty;
            if (node.Attributes.ContainsKey("visibility"))
            {
                visibility = node.Attributes["visibility"].GetType().Equals(typeof(string)) ? node.Attributes["visibility"].ToString() : string.Empty;
            }
            if (string.IsNullOrEmpty(visibility))
            {
                return true;
            }
            visibility = visibility.Trim();

            string name = string.Empty;
            string htmlHelper = string.Empty;
            if (sourceMetadata.ContainsKey("name"))
            {
                name = Convert.ToString(sourceMetadata["name"]);
            }
            if (sourceMetadata.ContainsKey("HtmlHelper"))
            {
                htmlHelper = Convert.ToString(sourceMetadata["HtmlHelper"]);
            }

            // Check for the source HtmlHelper or given name. If neither are configured,
            // then always visible.
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(htmlHelper))
            {
                return true;
            }

            // Chop off the namespace
            htmlHelper = htmlHelper.Substring(htmlHelper.LastIndexOf(".") + 1);

            // Get the keywords
            var visibilityKeywords = visibility.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            // All set. Now parse the visibility variable.
            foreach (string visibilityKeyword in visibilityKeywords)
            {
                if (visibilityKeyword == htmlHelper || visibilityKeyword == name || visibilityKeyword == "*")
                {
                    return true;
                }
                else if ((visibilityKeyword == htmlHelper + "IfSelected" || 
                    visibilityKeyword == name + "IfSelected" ||
                    visibilityKeyword == "IfSelected") && 
                    node.IsInCurrentPath())
                {
                    return true;
                }
                else if (visibilityKeyword == "!" + htmlHelper || visibilityKeyword == "!" + name || visibilityKeyword == "!*")
                {
                    return false;
                }
            }

            // Still nothing? Then it's OK!
            return true;
        }

        #endregion
    }
}

