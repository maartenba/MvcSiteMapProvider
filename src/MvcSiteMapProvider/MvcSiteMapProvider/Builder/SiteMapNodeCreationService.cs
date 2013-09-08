using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// A set of services useful for creating SiteMap nodes.
    /// </summary>
    public class SiteMapNodeCreationService
        : ISiteMapNodeCreationService
    {
        public SiteMapNodeCreationService(
            ISiteMapNodeFactory siteMapNodeFactory,
            INodeKeyGenerator nodeKeyGenerator,
            ISiteMapNodeParentMapFactory siteMapNodeParentMapFactory)
        {
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (siteMapNodeParentMapFactory == null)
                throw new ArgumentNullException("siteMapNodeParentMapFactory");

            this.siteMapNodeFactory = siteMapNodeFactory;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.siteMapNodeParentMapFactory = siteMapNodeParentMapFactory;
        }
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly ISiteMapNodeParentMapFactory siteMapNodeParentMapFactory;

        #region ISiteMapNodeService Members

        public virtual ISiteMapNode CreateSiteMapNode(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return this.siteMapNodeFactory.Create(siteMap, key, implicitResourceKey);
        }

        public virtual ISiteMapNode CreateDynamicSiteMapNode(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return this.siteMapNodeFactory.CreateDynamic(siteMap, key, implicitResourceKey);
        }

        public virtual string GenerateSiteMapNodeKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable)
        {
            return this.nodeKeyGenerator.GenerateKey(parentKey, key, url, title, area, controller, action, httpMethod, clickable);
        }

        public virtual ISiteMapNodeParentMap CreateSiteMapNodeParentMap(string parentKey, ISiteMapNode node, string sourceName)
        {
            return this.siteMapNodeParentMapFactory.Create(parentKey, node, sourceName);
        }

        #endregion
    }
}
