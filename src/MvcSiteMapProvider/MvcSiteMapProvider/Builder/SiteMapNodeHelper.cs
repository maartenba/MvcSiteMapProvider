using System;
using System.Collections.Generic;
using System.Globalization;
using MvcSiteMapProvider.DI;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// A set of services useful for building SiteMap nodes, including dynamic nodes.
    /// </summary>
    [ExcludeFromAutoRegistration]
    public class SiteMapNodeHelper
        : ISiteMapNodeHelper
    {
        public SiteMapNodeHelper(
            ISiteMap siteMap,
            ICultureContext cultureContext,
            ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory,
            IDynamicSiteMapNodeBuilderFactory dynamicSiteMapNodeBuilderFactory,
            IReservedAttributeNameProvider reservedAttributeNameProvider,
            ICultureContextFactory cultureContextFactory
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (cultureContext == null)
                throw new ArgumentNullException("cultureContext");
            if (siteMapNodeCreatorFactory == null)
                throw new ArgumentNullException("siteMapNodeCreatorFactory");
            if (dynamicSiteMapNodeBuilderFactory == null)
                throw new ArgumentNullException("dynamicSiteMapNodeBuilderFactory");
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (cultureContextFactory == null)
                throw new ArgumentNullException("cultureContextFactory");

            this.siteMap = siteMap;
            this.cultureContext = cultureContext;
            this.siteMapNodeCreatorFactory = siteMapNodeCreatorFactory;
            this.dynamicSiteMapNodeBuilderFactory = dynamicSiteMapNodeBuilderFactory;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.cultureContextFactory = cultureContextFactory;
        }

        protected readonly ISiteMap siteMap;
        protected readonly ICultureContext cultureContext;
        protected readonly ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory;
        protected readonly IDynamicSiteMapNodeBuilderFactory dynamicSiteMapNodeBuilderFactory;
        protected readonly IReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly ICultureContextFactory cultureContextFactory;

        #region ISiteMapNodeHelper Members

        public virtual string CreateNodeKey(string parentKey, string key, string url, string title, string area, string controller, string action, string httpMethod, bool clickable)
        {
            var siteMapNodeCreator = this.siteMapNodeCreatorFactory.Create(this.siteMap);
            return siteMapNodeCreator.GenerateSiteMapNodeKey(parentKey, key, url, title, area, controller, action, httpMethod, clickable);
        }

        public ISiteMapNodeToParentRelation CreateNode(string key, string parentKey, string sourceName)
        {
            return this.CreateNode(key, parentKey, sourceName, null);
        }

        public ISiteMapNodeToParentRelation CreateNode(string key, string parentKey, string sourceName, string implicitResourceKey)
        {
            var siteMapNodeCreator = this.siteMapNodeCreatorFactory.Create(this.siteMap);
            return siteMapNodeCreator.CreateSiteMapNode(key, parentKey, sourceName, implicitResourceKey);
        }

        public IEnumerable<ISiteMapNodeToParentRelation> CreateDynamicNodes(ISiteMapNodeToParentRelation node)
        {
            return this.CreateDynamicNodes(node, node.ParentKey);
        }

        public IEnumerable<ISiteMapNodeToParentRelation> CreateDynamicNodes(ISiteMapNodeToParentRelation node, string defaultParentKey)
        {
            var dynamicSiteMapNodeBuilder = this.dynamicSiteMapNodeBuilderFactory.Create(this.siteMap, this.CultureContext);
            return dynamicSiteMapNodeBuilder.BuildDynamicNodes(node.Node, defaultParentKey);
        }

        public IReservedAttributeNameProvider ReservedAttributeNames
        {
            get { return this.reservedAttributeNameProvider; }
        }

        public string SiteMapCacheKey
        {
            get { return this.siteMap.CacheKey; }
        }

        public ICultureContext CultureContext
        {
            get { return this.cultureContext; }
        }

        public ICultureContext CreateCultureContext(string cultureName, string uiCultureName)
        {
            return this.cultureContextFactory.Create(cultureName, uiCultureName);
        }

        public ICultureContext CreateCultureContext(CultureInfo culture, CultureInfo uiCulture)
        {
            return this.cultureContextFactory.Create(culture, uiCulture);
        }

        public ICultureContext CreateInvariantCultureContext()
        {
            return this.cultureContextFactory.CreateInvariant();
        }

        #endregion
    }
}
