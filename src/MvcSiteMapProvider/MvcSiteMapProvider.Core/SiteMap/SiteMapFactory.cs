using System;
using MvcSiteMapProvider.Core.SiteMap.Builder;
using MvcSiteMapProvider.Core.Security;
using MvcSiteMapProvider.Core.Mvc;
using MvcSiteMapProvider.Core.Collections;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapFactory
        : ISiteMapFactory
    {
        public SiteMapFactory(
            IAclModule aclModule,
            IControllerTypeResolver controllerTypeResolver,
            ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory,
            IGenericDictionaryFactory genericDictionaryFactory
            )
        {
            if (aclModule == null)
                throw new ArgumentNullException("aclModule");
            if (controllerTypeResolver == null)
                throw new ArgumentNullException("controllerTypeResolver");
            if (siteMapNodeCollectionFactory == null)
                throw new ArgumentNullException("siteMapNodeCollectionFactory");
            if (genericDictionaryFactory == null)
                throw new ArgumentNullException("genericDictionaryFactory");

            this.aclModule = aclModule;
            this.controllerTypeResolver = controllerTypeResolver;
            this.siteMapNodeCollectionFactory = siteMapNodeCollectionFactory;
            this.genericDictionaryFactory = genericDictionaryFactory;
        }

        private readonly IAclModule aclModule;
        private readonly IControllerTypeResolver controllerTypeResolver;
        private readonly ISiteMapNodeCollectionFactory siteMapNodeCollectionFactory;
        private readonly IGenericDictionaryFactory genericDictionaryFactory;

        #region ISiteMapFactory Members

        public ISiteMap Create(ISiteMapBuilder siteMapBuilder)
        {
            return new SiteMap(
                siteMapBuilder, 
                aclModule, 
                controllerTypeResolver, 
                siteMapNodeCollectionFactory,
                genericDictionaryFactory);
        }

        #endregion
    }
}
