using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Core;
using MvcSiteMapProvider.Core.Reflection;

namespace MvcSiteMapProvider.Core.Mvc.UrlResolver
{
    /// <summary>
    /// TODO: Update summary.
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
            var provider = siteMapUrlResolvers.FirstOrDefault(x => x.AppliesTo(providerName));
            if (provider == null)
            {
                // Return the SiteMapNodeUrlResolver type by default if the requested type is not found.

                // TODO: evaluate whether it makes sense to return the default only in the case where the provider name is empty string or "default"
                // and throw an exception if resolving fails.
                provider = siteMapUrlResolvers.FirstOrDefault(x => x.GetType().Equals(typeof(SiteMapNodeUrlResolver)));
            }
            return provider;
        }

        public string ResolveUrl(string providerName, ISiteMapNode mvcSiteMapNode, string area, string controller, string action, IDictionary<string, object> routeValues)
        {
            var provider = GetProvider(providerName);
            if (provider == null) return string.Empty;
            return provider.ResolveUrl(mvcSiteMapNode, area, controller, action, routeValues);
        }

        #endregion
    }
}
