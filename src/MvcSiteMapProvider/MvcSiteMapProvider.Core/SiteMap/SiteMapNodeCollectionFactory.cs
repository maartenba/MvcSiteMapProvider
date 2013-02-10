using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNodeCollectionFactory
        : ISiteMapNodeCollectionFactory
    {
        #region ISiteMapNodeCollectionFactory Members

        public ISiteMapNodeCollection Create(ISiteMap siteMap)
        {
            return new SiteMapNodeCollection(siteMap);
        }

        public ISiteMapNodeCollection CreateReadOnly(ISiteMapNodeCollection siteMapNodeCollection)
        {
            return new ReadOnlySiteMapNodeCollection(siteMapNodeCollection);
        }

        public ISiteMapNodeCollection CreateEmptyReadOnly(ISiteMap siteMap)
        {
            return new ReadOnlySiteMapNodeCollection(new SiteMapNodeCollection(siteMap));
        }

        #endregion
    }
}
