using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// A set of services useful for creating SiteMap nodes.
    /// </summary>
    public class SiteMapNodeCreator
        : ISiteMapNodeCreator
    {
        public SiteMapNodeCreator(
            ISiteMap siteMap,
            ISiteMapNodeFactory siteMapNodeFactory,
            INodeKeyGenerator nodeKeyGenerator,
            ISiteMapNodeParentMapFactory siteMapNodeParentMapFactory)
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (siteMapNodeParentMapFactory == null)
                throw new ArgumentNullException("siteMapNodeParentMapFactory");

            this.siteMap = siteMap;
            this.siteMapNodeFactory = siteMapNodeFactory;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.siteMapNodeParentMapFactory = siteMapNodeParentMapFactory;
        }
        protected readonly ISiteMap siteMap;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly ISiteMapNodeParentMapFactory siteMapNodeParentMapFactory;

        #region ISiteMapNodeService Members

        public ISiteMapNodeParentMap CreateSiteMapNode(string key, string parentKey, string sourceName, string implicitResourceKey)
        {
            var node = this.siteMapNodeFactory.Create(this.siteMap, key, implicitResourceKey);
            return this.siteMapNodeParentMapFactory.Create(parentKey, node, sourceName);
        }

        public ISiteMapNodeParentMap CreateDynamicSiteMapNode(string key, string parentKey, string sourceName, string implicitResourceKey)
        {
            var node = this.siteMapNodeFactory.CreateDynamic(this.siteMap, key, implicitResourceKey);
            return this.siteMapNodeParentMapFactory.Create(parentKey, node, sourceName);
        }

        public virtual string GenerateSiteMapNodeKey(string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable)
        {
            return this.nodeKeyGenerator.GenerateKey(null, key, url, title, area, controller, action, httpMethod, clickable);
        }

        #endregion
    }
}
