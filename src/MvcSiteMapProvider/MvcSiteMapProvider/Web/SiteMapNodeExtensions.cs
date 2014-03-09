using System;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// Extension methods for getting attribute values from a <see cref="T:System.Web.SiteMapNode"/> instance.
    /// </summary>
    public static class SiteMapNodeExtensions
    {
        public static string GetAttributeValue(this System.Web.SiteMapNode node, string key)
        {
            return node[key];
        }

        public static string GetAttributeValueOrFallback(this System.Web.SiteMapNode node, string key, string fallbackValue)
        {
            return string.IsNullOrEmpty(node[key]) ? fallbackValue : node[key];
        }
    }
}
