namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory that provides instances of SiteMapNodeToParentRelation for mapping 
    /// node instances to their parent nodes before they are added to the SiteMap.
    /// </summary>
    public class SiteMapNodeToParentRelationFactory
        : ISiteMapNodeToParentRelationFactory
    {
        #region ISiteMapNodeToParentRelationFactory Members

        public virtual ISiteMapNodeToParentRelation Create(string parentKey, ISiteMapNode node, string sourceName)
        {
            return new SiteMapNodeToParentRelation(parentKey, node, sourceName);
        }

        #endregion
    }
}
