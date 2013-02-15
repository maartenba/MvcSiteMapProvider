using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttributeCollectionFactory
        : IAttributeCollectionFactory
    {
        #region IAttributeCollectionFactory Members

        public IAttributeCollection Create(ISiteMap siteMap, ILocalizationService localizationService)
        {
            return new AttributeCollection(siteMap, localizationService);
        }

        #endregion
    }
}
