using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    public class DynamicSiteMapNodeBuilderFactory
        : IDynamicSiteMapNodeBuilderFactory
    {
        public DynamicSiteMapNodeBuilderFactory(
            ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory
            )
        {
            if (siteMapNodeCreatorFactory == null)
                throw new ArgumentNullException("siteMapNodeCreatorFactory");
            this.siteMapNodeCreatorFactory = siteMapNodeCreatorFactory;
        }
        protected readonly ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory;

        #region IDynamicSiteMapNodeBuilderFactory Members

        public IDynamicSiteMapNodeBuilder Create(ISiteMap siteMap)
        {
            var siteMapNodeCreator = this.siteMapNodeCreatorFactory.Create(siteMap);
            return new DynamicSiteMapNodeBuilder(siteMapNodeCreator);
        }

        #endregion
    }
}
