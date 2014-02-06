using System;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Collections.Specialized.AttributeDictionary"/>
    /// at runtime.
    /// </summary>
    public class AttributeDictionaryFactory
        : IAttributeDictionaryFactory
    {
        public AttributeDictionaryFactory(
            IRequestCache requestCache
            )
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        protected readonly IRequestCache requestCache;

        #region IAttributeDictionaryFactory Members

        public virtual IAttributeDictionary Create(ISiteMap siteMap, ILocalizationService localizationService)
        {
            return new AttributeDictionary(siteMap, localizationService, requestCache);
        }

        #endregion
    }
}
