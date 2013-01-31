// -----------------------------------------------------------------------
// <copyright file="SiteMapBuilderSet.cs" company="">
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
    public class SiteMapBuilderSet
        : ISiteMapBuilderSet
    {
        public SiteMapBuilderSet(
            string name,
            ISiteMapBuilder siteMapBuilder
            )
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (siteMapBuilder == null)
                throw new ArgumentNullException("siteMapBuilder");
            this.name = name;
            this.siteMapBuilder = siteMapBuilder;
        }

        private readonly string name;
        private readonly ISiteMapBuilder siteMapBuilder;

        #region ISiteMapBuilderSet Members

        public string Name
        {
            get { return this.name; }
        }

        public ISiteMapBuilder Builder
        {
            get { return this.siteMapBuilder; }
        }

        #endregion
    }
}
