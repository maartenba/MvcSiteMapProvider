using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.DI;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// A set of services useful for creating SiteMap nodes.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class SiteMapNodeCreator
        : ISiteMapNodeCreator
    {
        public SiteMapNodeCreator(
            ISiteMap siteMap,
            ISiteMapNodeFactory siteMapNodeFactory,
            INodeKeyGenerator nodeKeyGenerator,
            ISiteMapNodeToParentRelationFactory siteMapNodeToParentRelationFactory)
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (siteMapNodeToParentRelationFactory == null)
                throw new ArgumentNullException("siteMapNodeToParentRelationFactory");

            this.siteMap = siteMap;
            this.siteMapNodeFactory = siteMapNodeFactory;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.siteMapNodeToParentRelationFactory = siteMapNodeToParentRelationFactory;
        }
        protected readonly ISiteMap siteMap;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly ISiteMapNodeToParentRelationFactory siteMapNodeToParentRelationFactory;

        #region ISiteMapNodeService Members

        public ISiteMapNodeToParentRelation CreateSiteMapNode(string key, string parentKey, string sourceName, string implicitResourceKey)
        {
            var node = this.siteMapNodeFactory.Create(this.siteMap, key, implicitResourceKey);
            return this.siteMapNodeToParentRelationFactory.Create(parentKey, node, sourceName);
        }

        public ISiteMapNodeToParentRelation CreateDynamicSiteMapNode(string key, string parentKey, string sourceName, string implicitResourceKey)
        {
            var node = this.siteMapNodeFactory.CreateDynamic(this.siteMap, key, implicitResourceKey);
            return this.siteMapNodeToParentRelationFactory.Create(parentKey, node, sourceName);
        }

        public virtual string GenerateSiteMapNodeKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable)
        {
            return this.nodeKeyGenerator.GenerateKey(parentKey, key, url, title, area, controller, action, httpMethod, clickable);
        }

        #endregion
    }
}
