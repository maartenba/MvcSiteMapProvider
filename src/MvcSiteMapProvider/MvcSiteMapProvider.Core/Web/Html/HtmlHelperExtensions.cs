#region Using directives

using System.Web.Mvc;
using System.Web;
using MvcSiteMapProvider.Core.SiteMap;

#endregion

namespace MvcSiteMapProvider.Core.Web.Html
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
            //return new MvcSiteMapHtmlHelper(helper, SiteMap.Provider);
            return new MvcSiteMapHtmlHelper(helper, SiteMaps.Current);
        }

        /// <summary>
        /// Creates a new MvcSiteMapProvider HtmlHelper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="provider">The sitemap provider.</param>
        /// <returns>
        /// A <see cref="MvcSiteMapHtmlHelper"/> instance
        /// </returns>
        public static MvcSiteMapHtmlHelper MvcSiteMap(this HtmlHelper helper, ISiteMap siteMap)
        {
            //return new MvcSiteMapHtmlHelper(helper, provider);
            return new MvcSiteMapHtmlHelper(helper, siteMap);
        }

        /// <summary>
        /// Creates a new MvcSiteMapProvider HtmlHelper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="siteMapKey">The SiteMap Cache Key.</param>
        /// /// <param name="siteMapKey">Name of the sitemap builder set.</param>
        /// <returns>
        /// A <see cref="MvcSiteMapHtmlHelper"/> instance
        /// </returns>
        public static MvcSiteMapHtmlHelper MvcSiteMap(this HtmlHelper helper, string siteMapKey, string builderSetName)
        {
            ISiteMap siteMap = SiteMaps.GetSiteMap(siteMapKey, builderSetName);
            if (siteMap == null)
                throw new UnknownSiteMapException();
            return MvcSiteMap(helper, siteMap);

            //SiteMapProvider provider = SiteMap.Providers[providerName];
            //if (provider == null)
            //{
            //    throw new UnknownSiteMapProviderException(
            //        string.Format(Resources.Messages.UnknownSiteMapProvider, providerName));
            //}
            //return new MvcSiteMapHtmlHelper(helper, provider);
        }

        /// <summary>
        /// Creates a new MvcSiteMapProvider HtmlHelper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="builderSetName">Name of the sitemap builder set.</param>
        /// <returns>
        /// A <see cref="MvcSiteMapHtmlHelper"/> instance
        /// </returns>
        public static MvcSiteMapHtmlHelper MvcSiteMap(this HtmlHelper helper, string builderSetName)
        {
            ISiteMap siteMap = SiteMaps.GetSiteMap(builderSetName);
            if (siteMap == null)
                throw new UnknownSiteMapException();
            return MvcSiteMap(helper, siteMap);
        }
    }
}
