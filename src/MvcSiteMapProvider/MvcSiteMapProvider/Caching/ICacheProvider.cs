using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract for cache provider. Implement this interface to provide an alternate cache management
    /// system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICacheProvider<T>
    {
        bool Contains(string key);
        LazyLock Get(string key);
        bool TryGetValue(string key, out LazyLock value);
        void Add(string key, LazyLock item, ICacheDetails cacheDetails);
        void Remove(string key);
        event EventHandler<MicroCacheItemRemovedEventArgs<T>> ItemRemoved;
    }
}
