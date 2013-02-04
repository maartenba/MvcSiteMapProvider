using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AttributeCollectionFactory
        : IAttributeCollectionFactory
    {
        #region IAttributeCollectionFactory Members

        public IDictionary<string, string> Create(ISiteMap siteMap, Globalization.ILocalizationService localizationService)
        {
            return new AttributeCollection(siteMap, localizationService);
        }

        #endregion
    }
}
