#region Using directives

using System.Web.Mvc;
using System.Web;

#endregion

namespace MvcSiteMapProvider.Web.Html
{
    /// <summary>
    /// HtmlHelperExtensions class
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a new MvcSiteMapProvider HtmlHelper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <returns>
        /// A <see cref="MvcSiteMapHtmlHelper"/> instance 
        /// </returns>
        public static MvcSiteMapHtmlHelper MvcSiteMap(this HtmlHelper helper)
        {
            return new MvcSiteMapHtmlHelper(helper, SiteMap.Provider);
        }

        /// <summary>
        /// Creates a new MvcSiteMapProvider HtmlHelper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="provider">The sitemap provider.</param>
        /// <returns>
        /// A <see cref="MvcSiteMapHtmlHelper"/> instance
        /// </returns>
        public static MvcSiteMapHtmlHelper MvcSiteMap(this HtmlHelper helper, SiteMapProvider provider)
        {
            return new MvcSiteMapHtmlHelper(helper, provider);
        }

        /// <summary>
        /// Creates a new MvcSiteMapProvider HtmlHelper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="providerName">Name of the sitemap provider.</param>
        /// <returns>
        /// A <see cref="MvcSiteMapHtmlHelper"/> instance
        /// </returns>
        public static MvcSiteMapHtmlHelper MvcSiteMap(this HtmlHelper helper, string providerName)
        {
            SiteMapProvider provider = SiteMap.Providers[providerName];
            if (provider == null)
            {
                throw new UnknownSiteMapProviderException(
                    string.Format(Resources.Messages.UnknownSiteMapProvider, providerName));
            }
            return new MvcSiteMapHtmlHelper(helper, provider);
        }
    }
}
