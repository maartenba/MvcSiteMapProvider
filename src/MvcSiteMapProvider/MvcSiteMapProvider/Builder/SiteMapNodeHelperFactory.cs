using System;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory that creates instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeHelper"/>.
    /// </summary>
    public class SiteMapNodeHelperFactory
        : ISiteMapNodeHelperFactory
    {
        public SiteMapNodeHelperFactory(
            ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory,
            IDynamicSiteMapNodeBuilderFactory dynamicSiteMapNodeBuilderFactory,
            IReservedAttributeNameProvider reservedAttributeNameProvider,
            ICultureContextFactory cultureContextFactory
            )
        {
            if (siteMapNodeCreatorFactory == null)
                throw new ArgumentNullException("siteMapNodeCreatorFactory");
            if (dynamicSiteMapNodeBuilderFactory == null)
                throw new ArgumentNullException("dynamicSiteMapNodeBuilderFactory");
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (cultureContextFactory == null)
                throw new ArgumentNullException("cultureContextFactory");

            this.siteMapNodeCreatorFactory = siteMapNodeCreatorFactory;
            this.dynamicSiteMapNodeBuilderFactory = dynamicSiteMapNodeBuilderFactory;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.cultureContextFactory = cultureContextFactory;
        }
        protected readonly ISiteMapNodeCreatorFactory siteMapNodeCreatorFactory;
        protected readonly IDynamicSiteMapNodeBuilderFactory dynamicSiteMapNodeBuilderFactory;
        protected readonly IReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly ICultureContextFactory cultureContextFactory;

        #region ISiteMapNodeHelperFactory Members

        public ISiteMapNodeHelper Create(ISiteMap siteMap, ICultureContext cultureContext)
        {
            return new SiteMapNodeHelper(
                siteMap,
                cultureContext,
                this.siteMapNodeCreatorFactory,
                this.dynamicSiteMapNodeBuilderFactory,
                this.reservedAttributeNameProvider,
                this.cultureContextFactory);
        }

        #endregion
    }
}
