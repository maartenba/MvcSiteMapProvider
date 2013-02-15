using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MvcSiteMapProvider.Core.Web;

namespace MvcSiteMapProvider.Core.Caching
{
    /// <summary>
    /// The default cache key generator. This class generates a unique cache key for each 
    /// DnsSafeHost.
    /// </summary>
    public class SiteMapCacheKeyGenerator
        : ISiteMapCacheKeyGenerator
    {
        public SiteMapCacheKeyGenerator(
            IHttpContextFactory httpContextFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");
            this.httpContextFactory = httpContextFactory;
        }

        protected readonly IHttpContextFactory httpContextFactory;

        #region ISiteMapCacheKeyGenerator Members

        public string GenerateKey()
        {
            var context = httpContextFactory.Create();
            var builder = new StringBuilder();
            builder.Append("sitemap://");
            builder.Append(context.Request.Url.DnsSafeHost);
            builder.Append("/");
            return builder.ToString();
        }

        #endregion
    }
}
