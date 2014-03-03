using System;
using System.Globalization;

namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Globalization.ICultureContext"/> 
    /// at runtime.
    /// </summary>
    public interface ICultureContextFactory
    {
        ICultureContext CreateInvariant();
        ICultureContext Create(string cultureName, string uiCultureName);
        ICultureContext Create(CultureInfo culture, CultureInfo uiCulture);
    }
}
