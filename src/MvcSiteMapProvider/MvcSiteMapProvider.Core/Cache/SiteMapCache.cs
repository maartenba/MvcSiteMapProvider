using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Cache
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapCache 
        : ISiteMapCache
    {
        public SiteMapCache(System.Web.Caching.Cache cache)
        {
            if (cache == null)
                throw new ArgumentNullException("cache");
            this.cache = cache;
        }

        private readonly System.Web.Caching.Cache cache;


        public virtual ISiteMap this[string key]
        {
            get
            {
                return (ISiteMap)cache[key];
            }
            set
            {
                cache[key] = value;
            }
        }

        public virtual void Insert(string key, ISiteMap siteMap, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            cache.Insert(key, siteMap, null, absoluteExpiration, slidingExpiration, CacheItemPriority.NotRemovable, null);
        }

        public virtual int Count
        {
            get { return cache.Count; }
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
            cache.Remove(key);
        }
    }
}
