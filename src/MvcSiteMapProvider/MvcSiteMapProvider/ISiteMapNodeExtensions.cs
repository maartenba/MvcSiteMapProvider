using System;
using MvcSiteMapProvider.Web.UrlResolver;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contains extension logic for the ISiteMapNode interface that is not user 
    /// overridable.
    /// </summary>
    public static class ISiteMapNodeExtensions
    {
        public static bool UsesDefaultUrlResolver(this ISiteMapNode node)
        {
            return string.IsNullOrEmpty(node.UrlResolver) ||
                typeof(SiteMapNodeUrlResolver).Equals(Type.GetType(node.UrlResolver, false));
        }
    }
}
