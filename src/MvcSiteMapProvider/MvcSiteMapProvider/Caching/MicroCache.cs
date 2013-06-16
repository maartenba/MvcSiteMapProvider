using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;

namespace MvcSiteMapProvider.Caching
{
    public class MicroCache<T>
        : IMicroCache<T>
    {
        public MicroCache(
            ICacheProvider<T> cacheProvider
            )
        {
            if (cacheProvider == null)
                throw new ArgumentNullException("cacheProvider");
            this.cacheProvider = cacheProvider;

            // Attach our event so we can recieve notifications when objects are removed
            this.cacheProvider.ItemRemoved += cacheProvider_ItemRemoved;
        }
        protected readonly ICacheProvider<T> cacheProvider;
        private ReaderWriterLockSlim synclock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        #region IMicroCache<T> Members

        public event EventHandler<MicroCacheItemRemovedEventArgs<T>> ItemRemoved;

        public bool Contains(string key)
        {
            synclock.EnterReadLock();
            try
            {
                return this.cacheProvider.Contains(key);
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
                success = this.cacheProvider.TryGetValue(key, out lazy);
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
                    if (!this.cacheProvider.TryGetValue(key, out lazy))
                    {
                        lazy = new LazyLock();
                        var cacheDetails = getCacheDetailsFunction();
                        this.cacheProvider.Add(key, lazy, cacheDetails);
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
                this.cacheProvider.Remove(key);
            }
            finally
            {
                synclock.ExitWriteLock();
            }
        }

        #endregion

        private void cacheProvider_ItemRemoved(object sender, MicroCacheItemRemovedEventArgs<T> e)
        {
            // Cascade the event
            OnCacheItemRemoved(e);
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
