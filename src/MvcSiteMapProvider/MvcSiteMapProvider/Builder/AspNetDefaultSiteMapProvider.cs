using System;
using System.Web;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Provider for ASP.NET classic SiteMapProvider. Use this class to 
    /// get the default provider configured in the sitemap/providers section of 
    /// Web.config.
    /// </summary>
    public class AspNetDefaultSiteMapProvider
        : IAspNetSiteMapProvider
    {
        #region IAspNetStaticSiteMapProvider Members

        public SiteMapProvider GetProvider()
        {
            return System.Web.SiteMap.Provider;
        }

        #endregion
    }
}
