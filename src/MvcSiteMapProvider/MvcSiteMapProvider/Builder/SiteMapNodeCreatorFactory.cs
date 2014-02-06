using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    public class SiteMapNodeCreatorFactory
        : ISiteMapNodeCreatorFactory
    {
        public SiteMapNodeCreatorFactory(
            ISiteMapNodeFactory siteMapNodeFactory,
            INodeKeyGenerator nodeKeyGenerator,
            ISiteMapNodeToParentRelationFactory siteMapNodeToParentRelationFactory)
        {
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (siteMapNodeToParentRelationFactory == null)
                throw new ArgumentNullException("siteMapNodeToParentRelationFactory");

            this.siteMapNodeFactory = siteMapNodeFactory;
            this.nodeKeyGenerator = nodeKeyGenerator;
            this.siteMapNodeToParentRelationFactory = siteMapNodeToParentRelationFactory;
        }
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly ISiteMapNodeToParentRelationFactory siteMapNodeToParentRelationFactory;

        #region ISiteMapNodeCreatorFactory Members

        public ISiteMapNodeCreator Create(ISiteMap siteMap)
        {
            return new SiteMapNodeCreator(
                siteMap, 
                this.siteMapNodeFactory, 
                this.nodeKeyGenerator, 
                this.siteMapNodeToParentRelationFactory);
        }

        #endregion
    }
}
