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

        #region ISiteMapNodeCreatorFactory Members

        public ISiteMapNodeCreator Create(ISiteMap siteMap)
        {
            return new SiteMapNodeCreator(
                siteMap, 
                this.siteMapNodeFactory, 
                this.nodeKeyGenerator, 
                this.siteMapNodeParentMapFactory);
        }

        #endregion
    }
}
