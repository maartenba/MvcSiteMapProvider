using System;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Core.Globalization
{
    /// <summary>
    /// IExplicitResourceKeyParser interface. Provides a way to insert parsing logic 
    /// to convert a localization string into its explicit resource keys and values.
    /// </summary>
    public interface IExplicitResourceKeyParser
    {
        void HandleResourceAttribute(string attributeName, ref string value, ref NameValueCollection explicitResourceKeys);
    }
}
