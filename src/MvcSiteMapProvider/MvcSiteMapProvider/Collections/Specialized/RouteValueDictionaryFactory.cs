using System;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web.Script.Serialization;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of 
    /// <see cref="T:MvcSiteMapProvider.Collections.Specialized.RouteValueDictionary"/>
    /// at runtime.
    /// </summary>
    public class RouteValueDictionaryFactory
        : IRouteValueDictionaryFactory
    {
        public RouteValueDictionaryFactory(
            IRequestCache requestCache,
            IReservedAttributeNameProvider reservedAttributeNameProvider,
            IJsonToDictionaryDeserializer jsonToDictionaryDeserializer
            )
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (jsonToDictionaryDeserializer == null)
                throw new ArgumentNullException("jsonToDictionaryDeserializer");

            this.requestCache = requestCache;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.jsonToDictionaryDeserializer = jsonToDictionaryDeserializer;
        }

        protected readonly IRequestCache requestCache;
        protected readonly IReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly IJsonToDictionaryDeserializer jsonToDictionaryDeserializer;

        #region IRouteValueDictionaryFactory Members

        public IRouteValueDictionary Create(string siteMapNodeKey, ISiteMap siteMap)
        {
 	        return new RouteValueDictionary(siteMapNodeKey, siteMap, this.reservedAttributeNameProvider, this.jsonToDictionaryDeserializer, this.requestCache);
        }

        [Obsolete("Use the overload that accepts a siteMapNodeKey instead. This overload will be removed in version 5.")]
        public virtual IRouteValueDictionary Create(ISiteMap siteMap)
        {
            return new RouteValueDictionary("NO_KEY", siteMap, this.reservedAttributeNameProvider, this.jsonToDictionaryDeserializer, this.requestCache);
        }

        #endregion
    }
}
