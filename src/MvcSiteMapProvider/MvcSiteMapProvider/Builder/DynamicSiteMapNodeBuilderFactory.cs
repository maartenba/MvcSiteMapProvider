using System;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider.Builder
{
    public class DynamicSiteMapNodeBuilderFactory
        : IDynamicSiteMapNodeBuilderFactory
    {
        public DynamicSiteMapNodeBuilderFactory(
            ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory,
            ICultureContextFactory cultureContextFactory
            )
        {
            if (siteMapNodeCreatorFactory == null)
                throw new ArgumentNullException("siteMapNodeCreatorFactory");
            if (cultureContextFactory == null)
                throw new ArgumentNullException("cultureContextFactory");

            this.siteMapNodeCreatorFactory = siteMapNodeCreatorFactory;
            this.cultureContextFactory = cultureContextFactory;
        }
        protected readonly ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory;
        protected readonly ICultureContextFactory cultureContextFactory;

        #region IDynamicSiteMapNodeBuilderFactory Members

        public IDynamicSiteMapNodeBuilder Create(ISiteMap siteMap, ICultureContext cultureContext)
        {
            var siteMapNodeCreator = this.siteMapNodeCreatorFactory.Create(siteMap);
            return new DynamicSiteMapNodeBuilder(siteMapNodeCreator, cultureContext, this.cultureContextFactory);
        }

        #endregion
    }
}
