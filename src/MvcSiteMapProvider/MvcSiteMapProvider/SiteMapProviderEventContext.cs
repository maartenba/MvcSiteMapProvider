#region Using directives

using System.Web;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// SiteMapProviderEventContext
    /// </summary>
    public class SiteMapProviderEventContext
    {
        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public SiteMapProvider Provider { get; set; }

        /// <summary>
        /// Gets the current node.
        /// </summary>
        /// <value>The current node.</value>
        public SiteMapNode CurrentNode { get; private set; }

        /// <summary>
        /// Gets the parent node.
        /// </summary>
        /// <value>The parent node.</value>
        public SiteMapNode ParentNode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapProviderEventContext"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="currentNode">The current node.</param>
        /// <param name="parentNode">The parent node.</param>
        public SiteMapProviderEventContext(SiteMapProvider provider, SiteMapNode currentNode, SiteMapNode parentNode)
        {
            Provider = provider;
            CurrentNode = currentNode;
            ParentNode = parentNode;
        }
    }
}
