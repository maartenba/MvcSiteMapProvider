using System;
using MvcSiteMapProvider.Core.SiteMap;

namespace MvcSiteMapProvider.Core.Globalization
{
    /// <summary>
    /// Provides services to extract meta-keys and to later use the keys to localize text into different cultures. 
    /// </summary>
    public interface ILocalizationService
    {
        string ResourceKey { get; }
        string ExtractExplicitResourceKey(string attributeName, string value);
        string GetResourceString(string attributeName, string value, ISiteMap siteMap);
    }
}
