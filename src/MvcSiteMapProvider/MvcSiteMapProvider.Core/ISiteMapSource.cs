//using System.ComponentModel;
//using System.Text;
//using System.Web;

//namespace MvcSiteMapProvider.Core
//{
//    /// <summary>
//    /// ISiteMapSource interface.
//    /// </summary>
//    public interface ISiteMapSource
//    {
//        ///// <summary>
//        ///// Determines whether the site map source has data for site map provider with the specified provider name.
//        ///// </summary>
//        ///// <param name="providerName">The provider name.</param>
//        ///// <returns>
//        /////   <c>true</c> if the site map source has data for site map provider with the specified provider name; otherwise, <c>false</c>.
//        ///// </returns>
//        //bool HasDataForSiteMapProvider(string providerName);

//        ///// <summary>
//        ///// Provides the base data on which the context-aware provider can generate a full tree.
//        ///// </summary>
//        ///// <param name="rootNode">The root node (can be null).</param>
//        ///// <returns></returns>
//        //XSiteMapNode ProvideBaseData(XSiteMapNode rootNode);

//        /// <summary>
//        /// Provides the base data on which the context-aware provider can generate a full tree.
//        /// </summary>
//        /// <param name="providerName">The provider name.</param>
//        /// <param name="context">The context of the current request to identify the sitemap to retrieve.</param>
//        /// <returns>A XSiteMapNode tree.</returns>
//        XSiteMapNode ProvideBaseData(string providerName, ISiteMapRequestContext context);
//    }
//}