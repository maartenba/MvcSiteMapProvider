using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// This class wraps the <see cref="T:System.Web.Caching.Cache"/> object to allow type-safe
    /// interaction when managing <see cref="T:MvcSiteMapProvider.ISiteMap"/> instances.
    /// </summary>
    public class AspNetSiteMapCache 
        : ISiteMapCache
    {
        public AspNetSiteMapCache(
            IHttpContextFactory httpContextFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");

            this.httpContextFactory = httpContextFactory;
        }

        protected readonly IHttpContextFactory httpContextFactory;

        public event EventHandler<SiteMapCacheItemRemovedEventArgs> SiteMapRemoved;


        protected virtual System.Web.Caching.Cache Cache
        {
            get
            {
                var context = httpContextFactory.Create();
                return context.Cache;
            }
        }

        public virtual ISiteMap this[string key]
        {
            get { return (ISiteMap)this.Cache[key]; }
            set { this.Cache[key] = value; }
        }

        public virtual void Insert(string key, ISiteMap siteMap, ICacheDetails cacheDetails)
        {
            DateTime absolute = System.Web.Caching.Cache.NoAbsoluteExpiration;
            TimeSpan sliding = System.Web.Caching.Cache.NoSlidingExpiration;
            if (!this.IsDefaultTimeSpanValue(cacheDetails.AbsoluteCacheExpiration))
            {
                absolute = DateTime.UtcNow.Add(cacheDetails.AbsoluteCacheExpiration);
            }
            else if (!this.IsDefaultTimeSpanValue(cacheDetails.SlidingCacheExpiration))
            {
                sliding = cacheDetails.SlidingCacheExpiration;
            }
            CacheDependency dependency = null;
            ICacheDependency passedDependencies = cacheDetails.CacheDependencyFactory.Create();
            if (passedDependencies != null)
            {
                dependency = (CacheDependency)passedDependencies.Dependency;
            }

            this.Cache.Insert(key, siteMap, dependency, absolute, sliding, CacheItemPriority.NotRemovable, this.OnItemRemoved);
        }

        public virtual int Count
        {
            get { return this.Cache.Count; }
        }

        /// <summary>
        /// This method is called when a sitemap has been removed from the cache.
        /// </summary>
        /// <param name="key">Cached item key.</param>
        /// <param name="item">Cached item.</param>
        /// <param name="reason">Reason the cached item was removed.</param>
        protected virtual void OnItemRemoved(string key, object item, CacheItemRemovedReason reason)
        {
            var args = new SiteMapCacheItemRemovedEventArgs() { SiteMap = (ISiteMap)item };
            OnSiteMapRemoved(args);
        }

        protected virtual void OnSiteMapRemoved(SiteMapCacheItemRemovedEventArgs e)
        {
            if (this.SiteMapRemoved != null)
            {
                SiteMapRemoved(this, e);
            }
        }

        public virtual void Remove(string key)
        {
            this.Cache.Remove(key);
        }

        public virtual bool TryGetValue(string key, out ISiteMap value)
        {
            value = (ISiteMap)this.Cache.Get(key);
            if (value != null)
            {
                return true;
            }
            return false;
        }

        protected virtual bool IsDefaultTimeSpanValue(TimeSpan timeSpan)
        {
            return (timeSpan.Equals(TimeSpan.Zero) || timeSpan.Equals(TimeSpan.MinValue));
        }
    }
}
