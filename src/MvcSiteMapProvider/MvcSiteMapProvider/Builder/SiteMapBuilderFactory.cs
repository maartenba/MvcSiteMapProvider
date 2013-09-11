using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Visitor;

namespace MvcSiteMapProvider.Builder
{
    public class SiteMapBuilderFactory
    {
        public SiteMapBuilderFactory(
            ISiteMapNodeVisitor siteMapNodeVisitor,
            ISiteMapHierarchyBuilder siteMapHierarchyBuilder,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator,
            ISiteMapNodeHelperFactory siteMapNodeHelperFactory
            )
        {
            if (siteMapNodeVisitor == null)
                throw new ArgumentNullException("siteMapNodeVisitor");
            if (siteMapHierarchyBuilder == null)
                throw new ArgumentNullException("siteMapHierarchyBuilder");
            if (siteMapCacheKeyGenerator == null)
                throw new ArgumentNullException("siteMapCacheKeyGenerator");
            if (siteMapNodeHelperFactory == null)
                throw new ArgumentNullException("siteMapNodeHelperFactory");

            this.siteMapHierarchyBuilder = siteMapHierarchyBuilder;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;
            this.siteMapNodeHelperFactory = siteMapNodeHelperFactory;
            this.siteMapNodeVisitor = siteMapNodeVisitor;
        }
        protected readonly ISiteMapHierarchyBuilder siteMapHierarchyBuilder;
        protected readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        protected readonly ISiteMapNodeHelperFactory siteMapNodeHelperFactory;
        protected readonly ISiteMapNodeVisitor siteMapNodeVisitor;

        public virtual ISiteMapBuilder Create(ISiteMapNodeProvider siteMapNodeProvider)
        {
            return new SiteMapBuilder(
                siteMapNodeProvider, 
                this.siteMapNodeVisitor, 
                this.siteMapHierarchyBuilder, 
                this.siteMapCacheKeyGenerator, 
                this.siteMapNodeHelperFactory);
        }
    }
}
