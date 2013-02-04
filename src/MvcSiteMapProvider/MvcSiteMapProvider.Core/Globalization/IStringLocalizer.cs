using System;
using System.Collections.Specialized;

namespace MvcSiteMapProvider.Core.Globalization
{
    /// <summary>
    /// Contains methods to deal with localization of strings to the current culture.
    /// </summary>
    public interface IStringLocalizer
    {
        string GetResourceString(
            string attributeName, 
            string value, 
            bool enableLocalization, 
            string classKey, 
            string implicitResourceKey, 
            NameValueCollection explicitResourceKeys);
    }
}
