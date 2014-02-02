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
            IAttributeDictionaryFactory attributeDictionaryFactory,
            IRouteValueDictionaryFactory routeValueDictionaryFactory
            )
        {
            if (attributeDictionaryFactory == null)
                throw new ArgumentNullException("attributeDictionaryFactory");
            if (routeValueDictionaryFactory == null)
                throw new ArgumentNullException("routeValueDictionaryFactory");

            this.attributeDictionaryFactory = attributeDictionaryFactory;
            this.routeValueDictionaryFactory = routeValueDictionaryFactory;
        }

        protected readonly IAttributeDictionaryFactory attributeDictionaryFactory;
        protected readonly IRouteValueDictionaryFactory routeValueDictionaryFactory;

        #region ISiteMapNodeChildStateFactory Members

        public virtual IAttributeDictionary CreateAttributeDictionary(string siteMapNodeKey, string memberName, ISiteMap siteMap, ILocalizationService localizationService)
        {
            return attributeDictionaryFactory.Create(siteMapNodeKey, memberName, siteMap, localizationService);
        }

        public virtual IRouteValueDictionary CreateRouteValueDictionary(string siteMapNodeKey, string memberName, ISiteMap siteMap)
        {
            return routeValueDictionaryFactory.Create(siteMapNodeKey, memberName, siteMap);
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
