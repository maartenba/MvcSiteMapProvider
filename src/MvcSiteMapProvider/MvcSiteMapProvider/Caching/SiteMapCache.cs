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
    public class SiteMapCache 
        : ISiteMapCache
    {
        public SiteMapCache(
            IHttpContextFactory httpContextFactory,
            ICacheDependencyFactory cacheDependencyFactory
            )
        {
            if (httpContextFactory == null)
                throw new ArgumentNullException("httpContextFactory");
            if (cacheDependencyFactory == null)
                throw new ArgumentNullException("cacheDependencyFactory");

            this.httpContextFactory = httpContextFactory;
            this.cacheDependencyFactory = cacheDependencyFactory;
        }

        protected readonly IHttpContextFactory httpContextFactory;
        protected readonly ICacheDependencyFactory cacheDependencyFactory;

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
            get
            {
                return (ISiteMap)this.Cache[key];
            }
            set
            {
                this.Cache[key] = value;
            }
        }

        public virtual void Insert(string key, ISiteMap siteMap, TimeSpan absoluteExpiration, TimeSpan slidingExpiration)
        {
            this.Insert(key, siteMap, absoluteExpiration, slidingExpiration, null);
        }

        public virtual void Insert(string key, ISiteMap siteMap, TimeSpan absoluteExpiration, TimeSpan slidingExpiration, IEnumerable<string> fileDependencies)
        {
            DateTime absolute = System.Web.Caching.Cache.NoAbsoluteExpiration;
            TimeSpan sliding = System.Web.Caching.Cache.NoSlidingExpiration;
            if (absoluteExpiration != TimeSpan.Zero && absoluteExpiration != TimeSpan.MinValue)
            {
                absolute = DateTime.UtcNow.Add(absoluteExpiration);
            }
            else if (slidingExpiration != TimeSpan.Zero && slidingExpiration != TimeSpan.MinValue)
            {
                sliding = slidingExpiration;
            }
            var dependency = this.GetCacheDependency(fileDependencies);
            this.Cache.Insert(key, siteMap, dependency, absolute, sliding, CacheItemPriority.NotRemovable, OnItemRemoved);
        }

        protected virtual CacheDependency GetCacheDependency(IEnumerable<string> fileDependencies)
        {
            CacheDependency dependency = null;
            if (fileDependencies != null && fileDependencies.Count() > 0)
            {
                if (fileDependencies.Count() > 1)
                {
                    dependency = cacheDependencyFactory.CreateAggregateDependency();
                    foreach (var file in fileDependencies)
                    {
                        var fileDependency = cacheDependencyFactory.CreateFileDependency(file);
                        ((AggregateCacheDependency)dependency).Add(fileDependency);
                    }
                }
                else
                {
                    dependency = new CacheDependency(fileDependencies.First());
                }
            }
            return dependency;
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
                SiteMapRemoved(this, e);
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
    }
}
