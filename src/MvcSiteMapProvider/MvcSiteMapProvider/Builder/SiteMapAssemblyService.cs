using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// A set of services useful for building a SiteMap and its nodes.
    /// </summary>
    public class SiteMapAssemblyService
        : ISiteMapAssemblyService
    {
        public SiteMapAssemblyService(
            ISiteMapNodeBuilderService siteMapNodeBuilderService,
            ISiteMapHierarchyBuilder siteMapHierarchyBuilder,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator
            )
        {
            if (siteMapNodeBuilderService == null)
                throw new ArgumentNullException("siteMapNodeBuilderService");
            if (siteMapHierarchyBuilder == null)
                throw new ArgumentNullException("siteMapHierarchyBuilder");
            if (siteMapCacheKeyGenerator == null)
                throw new ArgumentNullException("siteMapCacheKeyGenerator");
            
            this.siteMapNodeBuilderService = siteMapNodeBuilderService;
            this.siteMapHierarchyBuilder = siteMapHierarchyBuilder;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;
        }
        protected readonly ISiteMapNodeBuilderService siteMapNodeBuilderService;
        protected readonly ISiteMapHierarchyBuilder siteMapHierarchyBuilder;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        
        #region ISiteMapAssemblyService Members

        public ISiteMapNode CreateSiteMapNode(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return this.siteMapNodeBuilderService.SiteMapNodeCreationService
                .CreateSiteMapNode(siteMap, key, implicitResourceKey);
        }

        public ISiteMapNodeParentMap CreateSiteMapNodeParentMap(string parentKey, ISiteMapNode node, string sourceName)
        {
            return this.siteMapNodeBuilderService.SiteMapNodeCreationService
                .CreateSiteMapNodeParentMap(parentKey, node, sourceName);
        }

        public IEnumerable<ISiteMapNodeParentMap> BuildDynamicNodeParentMaps(ISiteMap siteMap, ISiteMapNode node, string parentKey)
        {
            return this.siteMapNodeBuilderService.DynamicNodeParentMapBuilder
                .BuildDynamicNodeParentMaps(siteMap, node, parentKey);
        }

        public string GenerateSiteMapNodeKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable)
        {
            return this.siteMapNodeBuilderService.SiteMapNodeCreationService
                .GenerateSiteMapNodeKey(parentKey, key, url, title, area, controller, action, httpMethod, clickable);
        }

        public IEnumerable<ISiteMapNodeParentMap> BuildHierarchy(ISiteMap siteMap, IEnumerable<ISiteMapNodeParentMap> nodes)
        {
            return this.siteMapHierarchyBuilder.BuildHierarchy(siteMap, nodes);
        }

        public string SiteMapCacheKey
        {
            get { return this.siteMapCacheKeyGenerator.GenerateKey(); }
        }

        #endregion

    }
}
