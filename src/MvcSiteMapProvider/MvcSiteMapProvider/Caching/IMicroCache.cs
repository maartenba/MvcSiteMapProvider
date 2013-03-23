using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract for micro cache.
    /// </summary>
    public interface IMicroCache<T>
    {
        bool Contains(string key);
        T GetOrAdd(string key, Func<T> loadFunction, Func<ICacheDetails> getCacheDetailsFunction);
        void Remove(string key);
        event EventHandler<MicroCacheItemRemovedEventArgs<T>> ItemRemoved;
    }
}
