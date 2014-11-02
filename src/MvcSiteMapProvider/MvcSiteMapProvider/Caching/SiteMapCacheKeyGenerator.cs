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
            var builder = new StringBuilder();
            builder.Append("sitemap://");
            builder.Append(this.GetHostName());
            builder.Append("/");

            return builder.ToString();
        }

        #endregion

        protected virtual string GetHostName()
        {
            var context = this.mvcContextFactory.CreateHttpContext();
            var request = context.Request;

            // In a cloud or web farm environment, use the HTTP_HOST 
            // header to derive the host name.
            if (request.ServerVariables["HTTP_HOST"] != null)
            {
                return request.ServerVariables["HTTP_HOST"];
            }

            return request.Url.DnsSafeHost;
        }
    }
}
