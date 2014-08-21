using System;
using System.Web;
using System.Web.Caching;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// A cache provider that uses the <see cref="T:System.Web.HttpContext.Current.Cache"/> instance to 
    /// cache items that are added.
    /// </summary>
    /// <typeparam name="T">The type of item that will be stored in the cache.</typeparam>
    public class AspNetCacheProvider<T>
        : ICacheProvider<T>
    {
        public AspNetCacheProvider(
            IMvcContextFactory mvcContextFactory
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");
            this.mvcContextFactory = mvcContextFactory;
        }
        private readonly IMvcContextFactory mvcContextFactory;

        protected HttpContextBase Context
        {
            get
            {
                return this.mvcContextFactory.CreateHttpContext();
            }
        }

        #region ICacheProvider<T> Members

        public event EventHandler<MicroCacheItemRemovedEventArgs<T>> ItemRemoved;

        public bool Contains(string key)
        {
            return (Context.Cache[key] != null);
        }

        public LazyLock Get(string key)
        {
            return (LazyLock)Context.Cache.Get(key);
        }

        public bool TryGetValue(string key, out LazyLock value)
        {
            value = this.Get(key);
            if (value != null)
            {
                return true;
            }
            return false;
        }

        public void Add(string key, LazyLock item, ICacheDetails cacheDetails)
        {
            DateTime absolute = System.Web.Caching.Cache.NoAbsoluteExpiration;
            TimeSpan sliding = System.Web.Caching.Cache.NoSlidingExpiration;
            if (IsTimespanSet(cacheDetails.AbsoluteCacheExpiration))
            {
                absolute = DateTime.UtcNow.Add(cacheDetails.AbsoluteCacheExpiration);
            }
            else if (IsTimespanSet(cacheDetails.SlidingCacheExpiration))
            {
                sliding = cacheDetails.SlidingCacheExpiration;
            }
            var dependency = (CacheDependency)cacheDetails.CacheDependency.Dependency;

            Context.Cache.Insert(key, item, dependency, absolute, sliding, CacheItemPriority.NotRemovable, this.OnItemRemoved);
        }

        public void Remove(string key)
        {
            Context.Cache.Remove(key);
        }

        #endregion

        private bool IsTimespanSet(TimeSpan timeSpan)
        {
            return (!timeSpan.Equals(TimeSpan.MinValue));
        }

        /// <summary>
        /// This method is called when an item has been removed from the cache.
        /// </summary>
        /// <param name="key">Cached item key.</param>
        /// <param name="item">Cached item.</param>
        /// <param name="reason">Reason the cached item was removed.</param>
        protected virtual void OnItemRemoved(string key, object item, CacheItemRemovedReason reason)
        {
            var args = new MicroCacheItemRemovedEventArgs<T>(key, ((LazyLock)item).Get<T>(null));
            OnCacheItemRemoved(args);
        }

        protected virtual void OnCacheItemRemoved(MicroCacheItemRemovedEventArgs<T> e)
        {
            if (this.ItemRemoved != null)
            {
                ItemRemoved(this, e);
            }
        }
    }
}
