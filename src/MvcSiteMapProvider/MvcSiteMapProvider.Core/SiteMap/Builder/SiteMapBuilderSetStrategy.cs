// -----------------------------------------------------------------------
// <copyright file="SiteMapBuilderSetStrategy.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
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

        private readonly ISiteMapBuilderSet[] siteMapBuilderSets;



        #region ISiteMapBuilderSetStrategy Members

        public ISiteMapBuilder GetBuilder(string name)
        {
            var builderSet = siteMapBuilderSets.FirstOrDefault(x => x.Name == name);
            if (builderSet == null)
            {
                builderSet = siteMapBuilderSets[0];
            }
            if (builderSet == null)
                throw new MvcSiteMapException(Resources.Messages.SiteMapNoDefaultBuilderSetConfigured);
            return builderSet.Builder;
        }

        #endregion
    }
}
