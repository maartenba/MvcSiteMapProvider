using System;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory that provides instances of SiteMapNodeParentMap for mapping 
    /// node instances to their parent nodes before they are added to the SiteMap.
    /// </summary>
    public class SiteMapNodeParentMapFactory
        : ISiteMapNodeParentMapFactory
    {
        #region ISiteMapNodeParentMapFactory Members

        public virtual ISiteMapNodeParentMap Create(string parentKey, ISiteMapNode node, string sourceName)
        {
            return new SiteMapNodeParentMap(parentKey, node, sourceName);
        }

        #endregion
    }
}
