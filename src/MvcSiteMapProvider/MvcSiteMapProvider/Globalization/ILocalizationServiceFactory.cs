using System;

namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Globalization.ILocalizationService"/> 
    /// at runtime.
    /// </summary>
    public interface ILocalizationServiceFactory
    {
        ILocalizationService Create(string implicitResourceKey);
    }
}
