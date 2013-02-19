using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract for a class to provide type-safe access to a request-level cache.
    /// </summary>
    public interface IRequestCache
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T value);
    }
}
