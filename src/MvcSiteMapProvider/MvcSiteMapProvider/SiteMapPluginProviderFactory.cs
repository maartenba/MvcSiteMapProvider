using System;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Security;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.SiteMapPluginProvider"/>
    /// at runtime.
    /// </summary>
    public class SiteMapPluginProviderFactory
        : ISiteMapPluginProviderFactory
    {
        public SiteMapPluginProviderFactory(
            IAclModule aclModule
            )
        {
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");

            this.aclModule = aclModule;
        }

        protected readonly IAclModule aclModule;

        #region ISiteMapPluginProviderFactory Members

        public virtual ISiteMapPluginProvider Create(ISiteMapBuilder siteMapBuilder, IMvcResolver mvcResolver)
        {
            return new SiteMapPluginProvider(mvcResolver, siteMapBuilder, aclModule);
        }

        #endregion
    }
}
