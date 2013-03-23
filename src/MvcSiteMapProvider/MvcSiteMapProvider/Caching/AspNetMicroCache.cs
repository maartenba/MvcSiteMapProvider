using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Caching;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// This class wraps the <see cref="T:System.Web.Caching.Cache"/> object to allow type-safe
    /// interaction when managing cached object instances.
    /// </summary>
    public class AspNetMicroCache<T>
        : IMicroCache<T>
    {
        public AspNetMicroCache(
            IMvcContextFactory mvcContextFactory
            )
        {
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");

            // Set the cache reference
            this.cache = mvcContextFactory.CreateHttpContext().Cache;
        }

        protected readonly System.Web.Caching.Cache cache;
        private ReaderWriterLockSlim synclock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        public event EventHandler<MicroCacheItemRemovedEventArgs<T>> ItemRemoved;

        #region IMicroCache<T> Members

        public bool Contains(string key)
        {
            synclock.EnterReadLock();
            try
            {
                return (cache[key] != null);
            }
            finally
            {
                synclock.ExitReadLock();
            }
        }

        public T GetOrAdd(string key, Func<T> loadFunction, Func<ICacheDetails> getCacheDetailsFunction)
        {
            LazyLock lazy;
            bool success;

            synclock.EnterReadLock();
            try
            {
                success = this.TryGetValue(key, out lazy);
            }
            finally
            {
                synclock.ExitReadLock();
            }

            if (!success)
            {
                synclock.EnterWriteLock();
                try
                {
                    if (!this.TryGetValue(key, out lazy))
                    {
                        lazy = new LazyLock();
                        var cacheDetails = getCacheDetailsFunction();
                        this.Add(key, lazy, cacheDetails);
                    }
                }
                finally
                {
                    synclock.ExitWriteLock();
                }
            }

            return lazy.Get(loadFunction);
        }

        public void Remove(string key)
        {
            synclock.EnterWriteLock();
            try
            {
                cache.Remove(key);
            }
            finally
            {
                synclock.ExitWriteLock();
            }
        }

        #endregion

        private LazyLock Get(string key)
        {
            return (LazyLock)cache.Get(key);
        }

        private bool TryGetValue(string key, out LazyLock value)
        {
            value = this.Get(key);
            if (value != null)
            {
                return true;
            }
            return false;
        }

        private void Add(string key, LazyLock item, ICacheDetails cacheDetails)
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

            cache.Insert(key, item, dependency, absolute, sliding, CacheItemPriority.NotRemovable, this.OnItemRemoved);
        }

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
