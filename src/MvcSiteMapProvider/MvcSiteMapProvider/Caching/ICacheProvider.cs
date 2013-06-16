using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Caching
{
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
