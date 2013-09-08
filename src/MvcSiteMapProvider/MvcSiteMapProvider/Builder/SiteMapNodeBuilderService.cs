using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// A set of services useful for building SiteMap nodes, including dynamic nodes.
    /// </summary>
    public class SiteMapNodeBuilderService
        : ISiteMapNodeBuilderService
    {
        public SiteMapNodeBuilderService(
            ISiteMapNodeCreationService siteMapNodeCreationService,
            IDynamicNodeParentMapBuilder dynamicNodeParentMapBuilder
            )
        {
            if (siteMapNodeCreationService == null)
                throw new ArgumentNullException("siteMapNodeCreationService");
            if (dynamicNodeParentMapBuilder == null)
                throw new ArgumentNullException("dynamicNodeParentMapBuilder");

            this.siteMapNodeCreationService = siteMapNodeCreationService;
            this.dynamicNodeParentMapBuilder = dynamicNodeParentMapBuilder;
        }
        protected readonly ISiteMapNodeCreationService siteMapNodeCreationService;
        protected readonly IDynamicNodeParentMapBuilder dynamicNodeParentMapBuilder;

        #region ISiteMapNodeBuilderService Members

        public ISiteMapNodeCreationService SiteMapNodeCreationService
        {
            get { return this.siteMapNodeCreationService; }
        }

        public IDynamicNodeParentMapBuilder DynamicNodeParentMapBuilder
        {
            get { return this.dynamicNodeParentMapBuilder; }
        }

        #endregion
    }
}
