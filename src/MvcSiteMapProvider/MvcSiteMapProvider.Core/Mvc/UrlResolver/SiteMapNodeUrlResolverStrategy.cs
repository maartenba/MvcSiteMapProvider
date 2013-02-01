// -----------------------------------------------------------------------
// <copyright file="SiteMapNodeUrlResolverStrategy.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.Mvc.UrlResolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MvcSiteMapProvider.Core.SiteMap;

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
            return siteMapUrlResolvers.FirstOrDefault(x => x.AppliesTo(providerName));
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
