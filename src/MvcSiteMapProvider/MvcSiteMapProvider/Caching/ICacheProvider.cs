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

        // NOTE: In version 5, this should be changed to a ProviderItemRemovedEventArgs to pass the LazyLock
        // instead of T back to the MicroCache to make it consistently deal with a LazyLock and the
        // MicroCache consistently deal with a T.
        event EventHandler<MicroCacheItemRemovedEventArgs<T>> ItemRemoved;
    }
}
