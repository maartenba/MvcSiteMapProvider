using System;
using System.Text;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// The default cache key generator. This class generates a unique cache key for each 
    /// DnsSafeHost.
    /// </summary>
    public class SiteMapCacheKeyGenerator
        : ISiteMapCacheKeyGenerator
    {
        public SiteMapCacheKeyGenerator(
            IMvcContextFactory mvcContextFactory
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            this.mvcContextFactory = mvcContextFactory;
        }

        protected readonly IMvcContextFactory mvcContextFactory;

        #region ISiteMapCacheKeyGenerator Members

        public virtual string GenerateKey()
        {
            var context = mvcContextFactory.CreateHttpContext();
            var builder = new StringBuilder();
            builder.Append("sitemap://");
            builder.Append(context.Request.Url.DnsSafeHost);
            builder.Append("/");
            return builder.ToString();
        }

        #endregion
    }
}
