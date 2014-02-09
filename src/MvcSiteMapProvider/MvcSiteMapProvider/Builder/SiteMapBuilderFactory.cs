using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Visitor;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory that creates instances of <see cref="T:MvcSiteMapProvider.Builder.SiteMapBuilder"/>.
    /// This factory can be used during DI configuration for DI containers that don't support a way to 
    /// supply partial lists of constructor parameters. This enables us to create the type without tightly
    /// binding to a specific constructor signature, which makes the DI configuration brittle.
    /// </summary>
    public class SiteMapBuilderFactory
    {
        public SiteMapBuilderFactory(
            ISiteMapNodeVisitor siteMapNodeVisitor,
            ISiteMapHierarchyBuilder siteMapHierarchyBuilder,
            ISiteMapNodeHelperFactory siteMapNodeHelperFactory
            )
        {
            if (siteMapNodeVisitor == null)
                throw new ArgumentNullException("siteMapNodeVisitor");
            if (siteMapHierarchyBuilder == null)
                throw new ArgumentNullException("siteMapHierarchyBuilder");
            if (siteMapNodeHelperFactory == null)
                throw new ArgumentNullException("siteMapNodeHelperFactory");

            this.siteMapHierarchyBuilder = siteMapHierarchyBuilder;
            this.siteMapNodeHelperFactory = siteMapNodeHelperFactory;
            this.siteMapNodeVisitor = siteMapNodeVisitor;
        }
        protected readonly ISiteMapHierarchyBuilder siteMapHierarchyBuilder;
        protected readonly ISiteMapNodeHelperFactory siteMapNodeHelperFactory;
        protected readonly ISiteMapNodeVisitor siteMapNodeVisitor;

        public virtual ISiteMapBuilder Create(ISiteMapNodeProvider siteMapNodeProvider)
        {
            return new SiteMapBuilder(
                siteMapNodeProvider, 
                this.siteMapNodeVisitor, 
                this.siteMapHierarchyBuilder, 
                this.siteMapNodeHelperFactory);
        }
    }
}
