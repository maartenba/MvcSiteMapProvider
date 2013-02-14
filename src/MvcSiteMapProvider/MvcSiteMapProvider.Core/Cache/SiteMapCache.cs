using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using MvcSiteMapProvider.Core.SiteMap;
using MvcSiteMapProvider.Core.Web;

namespace MvcSiteMapProvider.Core.Cache
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapCache 
        : ISiteMapCache
    {
        public SiteMapCache(
            IHttpContextFactory httpContextFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");
            this.httpContextFactory = httpContextFactory;
        }

        private readonly IHttpContextFactory httpContextFactory;

        protected System.Web.Caching.Cache Cache
        {
            get
            {
                var context = httpContextFactory.Create();
                return context.Cache;
            }
        }

        public virtual ISiteMap this[string key]
        {
            get
            {
                return (ISiteMap)this.Cache[key];
            }
            set
            {
                this.Cache[key] = value;
            }
        }

        public virtual void Insert(string key, ISiteMap siteMap, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            this.Cache.Insert(key, siteMap, null, absoluteExpiration, slidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        public virtual int Count
        {
            get { return this.Cache.Count; }
        }

        ///// <summary>
        ///// When using caching, this method is being used to refresh the sitemap when the root sitemap node identifier is removed from cache.
        ///// </summary>
        ///// <param name="key">Cached item key.</param>
        ///// <param name="item">Cached item.</param>
        ///// <param name="reason">Reason the cached item was removed.</param>
        //private void OnSiteMapChanged(string key, object item, CacheItemRemovedReason reason)
        //{

        //}

        public virtual void Remove(string key)
        {
            this.Cache.Remove(key);
        }
    }
}
