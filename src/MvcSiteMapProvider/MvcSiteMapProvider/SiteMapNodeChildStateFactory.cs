using System;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Abstract factory for creating new instances of types required by the <see cref="T:MvcSiteMapProvider.SiteMapNode"/>
    /// at runtime.
    /// </summary>
    public class SiteMapNodeChildStateFactory
        : ISiteMapNodeChildStateFactory
    {
        public SiteMapNodeChildStateFactory(
            IAttributeCollectionFactory attributeCollectionFactory,
            IRouteValueCollectionFactory routeValueCollectionFactory
            )
        {
            if (attributeCollectionFactory == null)
                throw new ArgumentNullException("attributeCollectionFactory");
            if (routeValueCollectionFactory == null)
                throw new ArgumentNullException("routeValueCollectionFactory");

            this.attributeCollectionFactory = attributeCollectionFactory;
            this.routeValueCollectionFactory = routeValueCollectionFactory;
        }

        protected readonly IAttributeCollectionFactory attributeCollectionFactory;
        protected readonly IRouteValueCollectionFactory routeValueCollectionFactory;

        #region ISiteMapNodeChildStateFactory Members

        public virtual IAttributeCollection CreateAttributeCollection(ISiteMap siteMap, ILocalizationService localizationService)
        {
            return attributeCollectionFactory.Create(siteMap, localizationService);
        }

        public virtual IRouteValueCollection CreateRouteValueCollection(ISiteMap siteMap)
        {
            return routeValueCollectionFactory.Create(siteMap);
        }

        public virtual IPreservedRouteParameterCollection CreatePreservedRouteParameterCollection(ISiteMap siteMap)
        {
            return new PreservedRouteParameterCollection(siteMap);
        }

        public virtual IRoleCollection CreateRoleCollection(ISiteMap siteMap)
        {
            return new RoleCollection(siteMap);
        }

        public virtual IMetaRobotsValueCollection CreateMetaRobotsValueCollection(ISiteMap siteMap)
        {
            return new MetaRobotsValueCollection(siteMap);
        }

        #endregion
    }
}
