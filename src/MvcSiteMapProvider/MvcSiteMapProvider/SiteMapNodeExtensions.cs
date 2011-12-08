#region Using directives

using System.Web;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// SiteMapNode extensions
    /// </summary>
    public static class SiteMapNodeExtensions
    {
        /// <summary>
        /// Gets the level of the current SiteMapNode
        /// </summary>
        /// <param name="current">The current SiteMapNode</param>
        /// <returns>The level of the current SiteMapNode</returns>
        public static int GetNodeLevel(this SiteMapNode current)
        {
            var level = 0;
            var node = current;

            if (node != null)
            {
                while (node.ParentNode != null)
                {
                    level++;
                    node = node.ParentNode;
                }
            }

            return level;
        }

        // TODO: rework...

        /// <summary>
        /// Determines whether the specified node is in current path.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is in current path; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInCurrentPath(this SiteMapNode current)
        {
            var node = current;
            return (current.Provider.CurrentNode != null && (node == current.Provider.CurrentNode || current.Provider.CurrentNode.IsDescendantOf(node)));
        }
    }
}
