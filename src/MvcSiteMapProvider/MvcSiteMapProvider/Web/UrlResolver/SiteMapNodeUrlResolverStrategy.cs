using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider.Web.UrlResolver
{
    /// <summary>
    /// Tracks all of the registered instances of <see cref="T:MvcSiteMapProvider.Web.UrlResolver.ISiteMapNodeUrlResolver"/> and 
    /// allows the caller to get a specific named instance of <see cref="T:MvcSiteMapProvider.Web.UrlResolver.ISiteMapNodeUrlResolver"/> at runtime.
    /// </summary>
    public class SiteMapNodeUrlResolverStrategy
        : ISiteMapNodeUrlResolverStrategy
    {
        public SiteMapNodeUrlResolverStrategy(ISiteMapNodeUrlResolver[] siteMapUrlResolvers)
        {
            if (siteMapUrlResolvers == null)
                throw new ArgumentNullException("siteMapUrlResolvers");

            this.siteMapUrlResolvers = siteMapUrlResolvers;
        }

        private readonly ISiteMapNodeUrlResolver[] siteMapUrlResolvers;

        #region ISiteMapNodeUrlResolverStrategy Members

        public ISiteMapNodeUrlResolver GetProvider(string providerName)
        {
            var provider = this.siteMapUrlResolvers.FirstOrDefault(x => x.AppliesTo(providerName));
            if (provider == null)
            {
                if (!string.IsNullOrEmpty(providerName))
                {
                    throw new MvcSiteMapException(string.Format(Resources.Messages.NamedUrlResolverNotFound, providerName));
                }
                // Return the SiteMapNodeUrlResolver type by default if the requested type is empty string.
                provider = this.siteMapUrlResolvers.FirstOrDefault(x => x.GetType().Equals(typeof(SiteMapNodeUrlResolver)));
            }
            return provider;
        }

        public string ResolveUrl(string providerName, ISiteMapNode node, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            var provider = this.GetProvider(providerName);
            if (provider == null) return string.Empty;
            return provider.ResolveUrl(node, area, controller, action, routeValues);
        }

        #endregion
    }
}
