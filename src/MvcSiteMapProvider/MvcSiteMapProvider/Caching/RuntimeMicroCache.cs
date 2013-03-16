#if !NET35
using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// This class wraps <see cref="T:System.Runtime.Caching.ObjectCache"/> to allow type-safe
    /// interaction when managing cached object instances.
    /// </summary>
    public class RuntimeMicroCache<T>
        : IMicroCache<T>
    {
        public RuntimeMicroCache(
            ObjectCache cache
            )
        {
            if (cache == null)
                throw new ArgumentNullException("cache");

            this.cache = cache;
        }

        private readonly ObjectCache cache;
        private ReaderWriterLockSlim synclock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        public event EventHandler<MicroCacheItemRemovedEventArgs<T>> ItemRemoved;

        #region IMicroCache<T> Members

        public bool Contains(string key)
        {
            synclock.EnterReadLock();
            try
            {
                return cache.Contains(key);
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
            var policy = new CacheItemPolicy();

            // Set timeout
            policy.Priority = CacheItemPriority.NotRemovable;
            if (IsTimespanSet(cacheDetails.AbsoluteCacheExpiration))
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now.Add(cacheDetails.AbsoluteCacheExpiration);
            }
            else if (IsTimespanSet(cacheDetails.SlidingCacheExpiration))
            {
                policy.SlidingExpiration = cacheDetails.SlidingCacheExpiration;
            }

            // Add dependencies
            var dependencies = (IList<ChangeMonitor>)cacheDetails.CacheDependency.Dependency;
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    policy.ChangeMonitors.Add(dependency);
                }
            }

            // Setup callback
            policy.RemovedCallback = CacheItemRemoved;

            cache.Add(key, item, policy);
        }

        private bool IsTimespanSet(TimeSpan timeSpan)
        {
            return (!timeSpan.Equals(TimeSpan.MinValue));
        }

        private void CacheItemRemoved(CacheEntryRemovedArguments arguments)
        {
            var item = arguments.CacheItem;
            var args = new MicroCacheItemRemovedEventArgs<T>(item.Key, ((LazyLock)item.Value).Get<T>(null));
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
#endif