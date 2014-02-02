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
        IAttributeDictionary Create(string siteMapNodeKey, string memberName, ISiteMap siteMap, ILocalizationService localizationService);
    }
}
