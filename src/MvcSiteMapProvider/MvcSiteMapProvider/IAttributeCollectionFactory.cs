using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.IAttributeCollection"/> at runtime.
    /// </summary>
    public interface IAttributeCollectionFactory
    {
        IAttributeCollection Create(ISiteMap siteMap, ILocalizationService localizationService);
    }
}
