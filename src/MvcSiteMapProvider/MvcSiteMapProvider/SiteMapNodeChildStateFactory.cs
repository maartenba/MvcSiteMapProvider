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

        public virtual IAttributeDictionary CreateAttributeDictionary(string siteMapNodeKey, ISiteMap siteMap, ILocalizationService localizationService)
        {
            return attributeDictionaryFactory.Create(siteMapNodeKey, siteMap, localizationService);
        }

        [Obsolete("Use the overload that accepts a siteMapNodeKey instead. This overload will be removed in version 5.")]
        public virtual IAttributeDictionary CreateAttributeDictionary(ISiteMap siteMap, ILocalizationService localizationService)
        {
            return attributeDictionaryFactory.Create(string.Empty, siteMap, localizationService);
        }

        //public virtual IRouteValueDictionary CreateRouteValueDictionary(string siteMapNodeKey, ISiteMap siteMap)
        //{
        //    return routeValueDictionaryFactory.Create(siteMapNodeKey, siteMap);
        //}

        //[Obsolete("Use the overload that accepts a siteMapNodeKey instead. This overload will be removed in version 5.")]
        public virtual IRouteValueDictionary CreateRouteValueDictionary(ISiteMap siteMap)
        {
            //return routeValueDictionaryFactory.Create(string.Empty, siteMap);
            return routeValueDictionaryFactory.Create(siteMap);
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
