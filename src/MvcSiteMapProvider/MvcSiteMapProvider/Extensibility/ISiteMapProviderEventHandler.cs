namespace MvcSiteMapProvider.Extensibility
{
    /// <summary>
    /// ISiteMapProviderEventHandler contract.
    /// </summary>
    public interface ISiteMapProviderEventHandler
    {
        /// <summary>
        /// Called when adding site map node.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Add the node to sitemap?</returns>
        bool OnAddingSiteMapNode(SiteMapProviderEventContext context);

        /// <summary>
        /// Called when added site map node.
        /// </summary>
        /// <param name="context">The context.</param>
        void OnAddedSiteMapNode(SiteMapProviderEventContext context);
    }
}
