using System;
using System.Globalization;

namespace MvcSiteMapProvider.Globalization
{
    /// <summary>
    /// Contract for a class that can track the current culture while changing to
    /// another culture. Implement Dispose() to change back to the current culture.
    /// </summary>
    public interface ICultureContext
        : IDisposable
    {
        CultureInfo OriginalCulture { get; }
        CultureInfo OriginalUICulture { get; }
    }
}
