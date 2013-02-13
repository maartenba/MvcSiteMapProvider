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

        public ISiteMapNodeCollection Create()
        {
            return new SiteMapNodeCollection();
        }

        public ISiteMapNodeCollection CreateLockable(ISiteMap siteMap)
        {
            return new LockableSiteMapNodeCollection(siteMap);
        }

        public ISiteMapNodeCollection CreateReadOnly(ISiteMapNodeCollection siteMapNodeCollection)
        {
            return new ReadOnlySiteMapNodeCollection(siteMapNodeCollection);
        }

        public ISiteMapNodeCollection CreateEmptyReadOnly()
        {
            return new ReadOnlySiteMapNodeCollection(new SiteMapNodeCollection());
        }

        #endregion
    }
}
