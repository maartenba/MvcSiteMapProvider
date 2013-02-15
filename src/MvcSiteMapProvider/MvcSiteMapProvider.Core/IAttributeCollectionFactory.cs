using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Core.Globalization;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAttributeCollectionFactory
    {
        IAttributeCollection Create(ISiteMap siteMap, ILocalizationService localizationService);
    }
}
