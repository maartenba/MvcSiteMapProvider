#region Using directives

using System.Collections.Generic;
using System.Web;
using MvcSiteMapProvider.Extensibility;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Filtered SiteMapNode Visibility Provider.
    /// 
    /// The Web.config setting should specify attributesToIgnore=&quot;visibility&quot; in order to use this class!
    /// 
    /// Rules are parsed left-to-right, first match wins. Asterisk can be used to match any control. Exclamation mark can be used to negate a match.
    /// </summary>
    public class FilteredSiteMapNodeVisibilityProvider
        : ISiteMapNodeVisibilityProvider
    {
        #region ISiteMapNodeVisibilityProvider Members

        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="context">The context.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        public bool IsVisible(SiteMapNode node, HttpContext context, IDictionary<string, object> sourceMetadata)
        {
            // Convert to MvcSiteMapNode
            var mvcNode = node as MvcSiteMapNode;
            if (mvcNode == null)
            {
                return true;
            }

            // Is a visibility attribute specified?
            string visibility = mvcNode["visibility"];
            if (string.IsNullOrEmpty(visibility))
            {
                return true;
            }
            visibility = visibility.Trim();

            // Check for the source HtmlHelper
            if (sourceMetadata["HtmlHelper"] == null)
            {
                return true;
            }
            string htmlHelper = sourceMetadata["HtmlHelper"].ToString();
            htmlHelper = htmlHelper.Substring(htmlHelper.LastIndexOf(".") + 1);

            // All set. Now parse the visibility variable.
            foreach (string visibilityKeyword in visibility.Split(new[] { ',', ';' }))
            {
                if (visibilityKeyword == htmlHelper || visibilityKeyword == "*")
                {
                    return true;
                }
                else if (visibilityKeyword == "!" + htmlHelper || visibilityKeyword == "!*")
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
