using System;

namespace MvcSiteMapProvider.Core.RequestCache
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRequestCache
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T value);
    }
}
