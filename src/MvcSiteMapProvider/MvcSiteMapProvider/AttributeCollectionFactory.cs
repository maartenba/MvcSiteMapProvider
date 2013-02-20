using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.AttributeCollection"/>
    /// at runtime.
    /// </summary>
    public class AttributeCollectionFactory
        : IAttributeCollectionFactory
    {
        public AttributeCollectionFactory(
            IRequestCache requestCache
            )
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        protected readonly IRequestCache requestCache;

        #region IAttributeCollectionFactory Members

        public virtual IAttributeCollection Create(ISiteMap siteMap, ILocalizationService localizationService)
        {
            return new AttributeCollection(siteMap, localizationService, requestCache);
        }

        #endregion
    }
}
