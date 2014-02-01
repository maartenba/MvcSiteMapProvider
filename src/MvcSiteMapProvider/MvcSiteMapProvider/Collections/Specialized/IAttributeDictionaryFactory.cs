using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Collections.Specialized.IAttributeDictionary"/> 
    /// at runtime.
    /// </summary>
    public interface IAttributeDictionaryFactory
    {
        IAttributeDictionary Create(string siteMapNodeKey, ISiteMap siteMap, ILocalizationService localizationService);

        [Obsolete("Use the overload that accepts a siteMapNodeKey instead. This overload will be removed in version 5.")]
        IAttributeDictionary Create(ISiteMap siteMap, ILocalizationService localizationService);
    }
}
