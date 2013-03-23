using System;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Security;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Provider for plugins used by <see cref="T:MvcSiteMapProvider.SiteMap"/>.
    /// </summary>
    public class SiteMapPluginProvider
        : ISiteMapPluginProvider
    {
        public SiteMapPluginProvider(
            IMvcResolver mvcResolver,
            ISiteMapBuilder siteMapBuilder,
            IAclModule aclModule
            )
        {
            if (siteMapBuilder == null)
                throw new ArgumentNullException("siteMapBuilder");
            if (mvcResolver == null)
                throw new ArgumentNullException("mvcResolver");
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");

            this.siteMapBuilder = siteMapBuilder;
            this.mvcResolver = mvcResolver;
            this.aclModule = aclModule;
        }

        protected readonly ISiteMapBuilder siteMapBuilder;
        protected readonly IMvcResolver mvcResolver;
        protected readonly IAclModule aclModule;

        #region ISiteMapPluginProvider Members

        public virtual ISiteMapBuilder SiteMapBuilder
        {
            get { return siteMapBuilder; }
        }

        public virtual IMvcResolver MvcResolver
        {
            get { return mvcResolver; }
        }

        public virtual IAclModule AclModule
        {
            get { return aclModule; }
        }

        #endregion
    }
}
