using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Core.Globalization;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAttributeCollectionFactory
    {
        IDictionary<string, string> Create(ISiteMap siteMap, ILocalizationService localizationService);
    }
}
