using System;

namespace MvcSiteMapProvider.Web
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SiteMapNodeExtensions
    {
        public static string GetAttributeValue(this System.Web.SiteMapNode node, string key)
        {
            return node[key];
        }

        public static string GetAttributeValueOrFallback(this System.Web.SiteMapNode node, string key, string fallbackValue)
        {
            return String.IsNullOrEmpty(node[key]) ? fallbackValue : node[key];
        }
    }
}
