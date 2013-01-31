// -----------------------------------------------------------------------
// <copyright file="SiteMapCacheKeyGenerator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapCacheKeyGenerator
        : ISiteMapCacheKeyGenerator
    {

        #region ISiteMapCacheKeyGenerator Members

        public string GenerateKey(HttpContext context)
        {
            var builder = new StringBuilder();
            builder.Append("sitemap://");
            builder.Append(context.Request.Url.DnsSafeHost);
            builder.Append("/");
            return builder.ToString();
        }

        #endregion
    }
}
