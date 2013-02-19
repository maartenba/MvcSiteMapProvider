using System;
using System.Linq;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Tracks all of the registered instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapBuilderSet"/> and 
    /// allows the caller to get a specific named instance of this interface at runtime.
    /// </summary>
    public class SiteMapBuilderSetStrategy
        : ISiteMapBuilderSetStrategy
    {
        public SiteMapBuilderSetStrategy(
            ISiteMapBuilderSet[] siteMapBuilderSets
            )
        {
            if (siteMapBuilderSets == null)
                throw new ArgumentNullException("siteMapBuilderSets");
            this.siteMapBuilderSets = siteMapBuilderSets;
        }

        protected readonly ISiteMapBuilderSet[] siteMapBuilderSets;

        #region ISiteMapBuilderSetStrategy Members

        public virtual ISiteMapBuilderSet GetBuilderSet(string builderSetName)
        {
            var builderSet = siteMapBuilderSets.FirstOrDefault(x => x.AppliesTo(builderSetName));
            if (builderSet == null && siteMapBuilderSets.Count() > 0)
            {
                builderSet = this.GetBuilderSet("default");
            }
            if (builderSet == null)
                throw new MvcSiteMapException(Resources.Messages.SiteMapNoDefaultBuilderSetConfigured);
            return builderSet;
        }

        public virtual ISiteMapBuilder GetBuilder(string builderSetName)
        {
            var builderSet = this.GetBuilderSet(builderSetName);
            return builderSet.Builder;
        }

        public virtual ICacheDependency CreateCacheDependency(string builderSetName)
        {
            var builderSet = this.GetBuilderSet(builderSetName);
            return builderSet.CreateCacheDependency();
        }

        #endregion
    }
}
