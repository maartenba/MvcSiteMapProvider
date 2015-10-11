namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract for a class to provide type-safe access to a cache dictionary.
    /// </summary>
    public interface ICache
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T value);
    }
}