using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Web.UrlResolver;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Provider for plugins used by <see cref="T:MvcSiteMapProvider.SiteMapNode"/>.
    /// </summary>
    public class SiteMapNodePluginProvider
        : ISiteMapNodePluginProvider
    {
        public SiteMapNodePluginProvider(
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy,
            ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy
            )
        {
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");
            if (siteMapNodeVisibilityProviderStrategy == null)
                throw new ArgumentNullException("siteMapNodeVisibilityProviderStrategy");

            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
            this.siteMapNodeVisibilityProviderStrategy = siteMapNodeVisibilityProviderStrategy;
        }

        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;
        protected readonly ISiteMapNodeVisibilityProviderStrategy siteMapNodeVisibilityProviderStrategy;

        #region ISiteMapNodePluginService Members

        public virtual IDynamicNodeProviderStrategy DynamicNodeProviderStrategy
        {
            get { return this.dynamicNodeProviderStrategy; }
        }

        public virtual ISiteMapNodeUrlResolverStrategy UrlResolverStrategy
        {
            get { return this.siteMapNodeUrlResolverStrategy; }
        }

        public virtual ISiteMapNodeVisibilityProviderStrategy VisibilityProviderStrategy
        {
            get { return this.siteMapNodeVisibilityProviderStrategy; }
        }

        #endregion
    }
}
