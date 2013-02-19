using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.AttributeCollection"/>
    /// at runtime.
    /// </summary>
    public class AttributeCollectionFactory
        : IAttributeCollectionFactory
    {
        #region IAttributeCollectionFactory Members

        public virtual IAttributeCollection Create(ISiteMap siteMap, ILocalizationService localizationService)
        {
            return new AttributeCollection(siteMap, localizationService);
        }

        #endregion
    }
}
