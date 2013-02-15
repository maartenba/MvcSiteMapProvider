using System;
using MvcSiteMapProvider.Core;

namespace MvcSiteMapProvider.Core.Globalization
{
    /// <summary>
    /// Provides services to extract meta-keys and to later use the keys to localize text into different cultures. 
    /// </summary>
    public interface ILocalizationService
    {
        string ResourceKey { get; }
        string ExtractExplicitResourceKey(string attributeName, string value);
        void RemoveResourceKey(string attributeName);
        string GetResourceString(string attributeName, string value, ISiteMap siteMap);
    }
}
