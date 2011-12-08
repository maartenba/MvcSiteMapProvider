#region Using directives

using MvcSiteMapProvider.Extensibility;

#endregion

namespace MvcSiteMapProvider
{
    /// <summary>
    /// DefaultSiteMapProviderEventHandler
    /// </summary>
    public class DefaultSiteMapProviderEventHandler
        : ISiteMapProviderEventHandler
    {
        #region ISiteMapProviderEventHandler Members

        /// <summary>
        /// Called when adding site map node.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Add the node to sitemap?</returns>
        public bool OnAddingSiteMapNode(SiteMapProviderEventContext context)
        {
            return true;
        }

        /// <summary>
        /// Called when added site map node.
        /// </summary>
        /// <param name="context">The context.</param>
        public void OnAddedSiteMapNode(SiteMapProviderEventContext context)
        {
        }

        #endregion
    }
}
